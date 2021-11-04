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
        public MainController data = new();

        public MainWindow()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = data;
            DataGrid filesGrid = this.Find<DataGrid>("Files");
            filesGrid.AddHandler(
                PointerPressedEvent,
                (s, e) =>
                {
                    if (e.MouseButton == MouseButton.Left)
                    {
                        if (e.ClickCount == 2)
                        {
                            var row = ((IControl)e.Source).GetSelfAndVisualAncestors()
                                .OfType<DataGridRow>()
                                .FirstOrDefault();

                            data.GoToDirectory(row.GetIndex());
                        }
                    }
                },
                handledEventsToo: true);
        }

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
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            FileDialogFilter filter = new FileDialogFilter();
            filter.Extensions.Add("ics");
            filter.Name = "Calendar (.ics)";

            saveFileDialog.Filters = new List<FileDialogFilter>();
            saveFileDialog.Filters.Add(filter);

            string result = await saveFileDialog.ShowAsync(this);
            if (result != null)
            {
                try
                {
                    byte[] d = System.Text.Encoding.UTF8.GetBytes(data.Calendar.ToString());
                    File.WriteAllBytes(result, d);
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
            Event? ev = await AddEvent.Show(this);
            if (ev != null)
            {
                data.Calendar.events.Add(ev);
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
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            FileDialogFilter filter = new FileDialogFilter();
            filter.Extensions.Add("pdf");
            filter.Name = "PDF Files (.pdf)";

            saveFileDialog.Filters = new List<FileDialogFilter>();
            saveFileDialog.Filters.Add(filter);

            string result = await saveFileDialog.ShowAsync(this);
            if (result != null)
            {
                try
                {
                    FileInfo file = new FileInfo(result);
                    List<Event> events = new List<Event>();
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
                try
                {
                    data.SaveKey(result);
                }
                catch (Exception ex)
                {
                    await MessageBox.Show(this, ex.Message, Localization.Localization.Exception_Saving, MessageBox.MessageBoxButtons.OkCancel);
                }
            }
        }

        private async void OpenKey_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();

            FileDialogFilter txtFilter = new();
            txtFilter.Extensions.Add("txt");
            txtFilter.Name = "Text files (.txt)";

            FileDialogFilter tkeyFilter = new();
            tkeyFilter.Extensions.Add("tkey");
            tkeyFilter.Name = "Encrypted key (.tkey)";

            openFileDialog.Filters = new();
            openFileDialog.Filters.Add(txtFilter);
            openFileDialog.Filters.Add(tkeyFilter);

            string[] result = await openFileDialog.ShowAsync(this);
            if (result != null && result.Length > 0)
            {
                try
                {
                    data.OpenKey(result[0]);
                }
                catch (Exception ex)
                {
                    await MessageBox.Show(this, ex.Message, Localization.Localization.Exception_Saving, MessageBox.MessageBoxButtons.OkCancel);
                }
            }
        }

        private void EditKey_Click(object sender, RoutedEventArgs e)
        {
            EditKey.Show(this, data.keys.ToString());
        }
    }
}