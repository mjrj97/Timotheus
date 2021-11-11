using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Timotheus.Schedule;
using Timotheus.Utility;

namespace Timotheus
{
    public partial class MainWindow : Window
    {
        public MainController data;
        
        public MainWindow()
        {
            data = new();
            AvaloniaXamlLoader.Load(this);
            DataContext = data;
            this.Find<DataGrid>("Files").AddHandler(
                DoubleTappedEvent,
                (s, e) =>
                {
                    var row = ((IControl)e.Source).GetSelfAndVisualAncestors()
                                .OfType<DataGridRow>()
                                .FirstOrDefault();

                    if (row != null)
                        data.GoToDirectory(row.GetIndex());
                },
                handledEventsToo: true);
        }

        private async void StartUpKey()
        {
            try
            {
                string keyPath = Timotheus.Registry.Get("KeyPath");
                if (!File.Exists(keyPath))
                {
                    Timotheus.Registry.Remove("KeyPath");
                }
                switch (Path.GetExtension(keyPath))
                {
                    case ".tkey":
                        string encodedPassword = Timotheus.Registry.Get("KeyPassword");
                        string password;
                        if (encodedPassword != string.Empty)
                            password = Cipher.Decrypt(encodedPassword);
                        else
                            password = await PasswordDialog.Show(this);
                        data.LoadKey(keyPath, password);
                        break;
                    case ".txt":
                        data.LoadKey(keyPath);
                        break;
                }
            }
            catch (Exception e)
            {
                await MessageBox.Show(this, e.Message, Localization.Localization.Exception_NoKeys, MessageBox.MessageBoxButtons.OkCancel);
            }
        }

        public override void Show()
        {
            base.Show();
            if (!isShown)
            {
                StartUpKey();
                isShown = true;
                File.AppendAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/Test.txt", "Test");
            }
        }
        private static bool isShown = false;

        /// <summary>
        /// Changes the selected year and calls UpdateTable.
        /// </summary>
        private void Period_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                Button button = (Button)sender;
                if (button.Name == "+")
                    data.UpdatePeriod(true);
                else if (button.Name == "-")
                    data.UpdatePeriod(false);
            }
        }

        private async void OpenCalendar_Click(object sender, RoutedEventArgs e)
        {
            Schedule.Calendar calendar = await OpenCalendar.Show(this);
            if (calendar != null)
                data.Calendar = calendar;
        }

        private async void SaveCalendar_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new();
            FileDialogFilter filter = new();
            filter.Extensions.Add("ics");
            filter.Name = "Calendar (.ics)";

            saveFileDialog.Filters = new();
            saveFileDialog.Filters.Add(filter);

            string result = await saveFileDialog.ShowAsync(this);
            if (result != null)
            {
                try
                {
                    data.Calendar.Save(result);
                }
                catch (Exception ex)
                {
                    await MessageBox.Show(this, ex.Message, Localization.Localization.Exception_Saving, MessageBox.MessageBoxButtons.OkCancel);
                }
            }
        }

        private void SyncCalendar_Click(object sender, RoutedEventArgs e)
        {
            SyncCalendar.Show(this, data.Calendar, data.calendarPeriod);
            data.UpdateCalendarTable();
        }

        private async void AddEvent_Click(object sender, RoutedEventArgs e)
        {
            Event ev = await AddEvent.Show(this);
            if (ev != null)
            {
                data.Calendar.Events.Add(ev);
                data.UpdateCalendarTable();
            }
        }

        private void RemoveEvent_Click(object sender, RoutedEventArgs e)
        {
            Event ev = (Event)((Button)e.Source).DataContext;
            if (ev != null)
            {
                data.Remove(ev);
            }
        }

        private async void ExportPDF_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new();
            FileDialogFilter filter = new();
            filter.Extensions.Add("pdf");
            filter.Name = "PDF Files (.pdf)";

            saveFileDialog.Filters = new();
            saveFileDialog.Filters.Add(filter);

            string result = await saveFileDialog.ShowAsync(this);
            if (result != null)
            {
                try
                {
                    FileInfo file = new(result);
                    List<Event> events = new();
                    for (int i = 0; i < data.Events.Count; i++)
                    {
                        events.Add(data.Events[i]);
                    }
                    PDF.ExportCalendar(events, file.DirectoryName, file.Name, data.keys.Get("Settings-Name"), data.keys.Get("Settings-Address"), data.keys.Get("Settings-Image"), data.PeriodText);
                }
                catch (Exception ex)
                {
                    await MessageBox.Show(this, ex.Message, Localization.Localization.Exception_Saving, MessageBox.MessageBoxButtons.OkCancel);
                }
            }
        }

        private void PeriodBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                data.calendarPeriod.SetPeriod(((TextBox)sender).Text);
                data.UpdateCalendarTable();
            }
        }

        private void UpDirectory_Click(object sender, RoutedEventArgs e)
        {
            data.GoUpDirectory();
        }

        private void SyncFiles_Click(object sender, RoutedEventArgs e)
        {
            data.Directory.Synchronize();
        }

        private async void SetupFiles_Click(object sender, RoutedEventArgs e)
        {
            data.Directory = await SetupSFTP.Show(this);
        }

        private async void SaveKey_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new();
            
            FileDialogFilter txtFilter = new();
            txtFilter.Extensions.Add("txt");
            txtFilter.Name = "Text files (.txt)";

            FileDialogFilter tkeyFilter = new();
            tkeyFilter.Extensions.Add("tkey");
            tkeyFilter.Name = "Encrypted key (.tkey)";

            saveFileDialog.Filters = new();
            saveFileDialog.Filters.Add(txtFilter);
            saveFileDialog.Filters.Add(tkeyFilter);

            string result = await saveFileDialog.ShowAsync(this);
            if (result != null)
            {
                switch (Path.GetExtension(result))
                {
                    case ".tkey":
                        string password = await PasswordDialog.Show(this);
                        try
                        {
                            data.SaveKey(result, password);
                        }
                        catch (Exception ex)
                        {
                            await MessageBox.Show(this, ex.Message, Localization.Localization.Exception_Saving, MessageBox.MessageBoxButtons.OkCancel);
                        }
                        break;
                    case ".txt":
                        try
                        {
                            data.SaveKey(result);
                        }
                        catch (Exception ex)
                        {
                            await MessageBox.Show(this, ex.Message, Localization.Localization.Exception_Saving, MessageBox.MessageBoxButtons.OkCancel);
                        }
                        break;
                }
            }
        }

        private async void OpenKey_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();

            FileDialogFilter txtFilter = new();
            txtFilter.Extensions.Add("txt");
            txtFilter.Extensions.Add("tkey");
            txtFilter.Name = "Key files (.txt, .tkey)";

            openFileDialog.Filters = new();
            openFileDialog.Filters.Add(txtFilter);

            string[] result = await openFileDialog.ShowAsync(this);
            if (result != null && result.Length > 0)
            {
                switch (Path.GetExtension(result[0]))
                {
                    case ".tkey":
                        string password = await PasswordDialog.Show(this);
                        try
                        {
                            data.LoadKey(result[0], password);
                        }
                        catch (Exception ex)
                        {
                            await MessageBox.Show(this, ex.Message, Localization.Localization.Exception_LoadFailed, MessageBox.MessageBoxButtons.OkCancel);
                        }
                        break;
                    case ".txt":
                        try
                        {
                            data.LoadKey(result[0]);
                        }
                        catch (Exception ex)
                        {
                            await MessageBox.Show(this, ex.Message, Localization.Localization.Exception_LoadFailed, MessageBox.MessageBoxButtons.OkCancel);
                        }
                        break;
                }
            }
        }

        private async void EditKey_Click(object sender, RoutedEventArgs e)
        {
            data.keys = await EditKey.Show(this, data.keys.ToString());
        }
    }
}