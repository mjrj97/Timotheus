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
    /// <summary>
    /// MainWindow of the Timotheus app. Contains Calendar and Filesharing tabs.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Data Context of the MainWindow.
        /// </summary>
        private readonly MainController data;

        /// <summary>
        /// Constructor. Creates datacontext and loads XAML.
        /// </summary>
        public MainWindow()
        {
            data = new();
            AvaloniaXamlLoader.Load(this);
            DataContext = data;
        }

        /// <summary>
        /// Loads the key last used.
        /// </summary>
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
                        string password = string.Empty;
                        if (encodedPassword != string.Empty)
                        {
                            password = Cipher.Decrypt(encodedPassword);
                            data.LoadKey(keyPath, password);
                        }
                        else
                        {
                            PasswordDialog dialog = new();
                            bool result = await dialog.Show(this);
                            if (result)
                            {
                                password = dialog.Password;
                                data.LoadKey(keyPath, password);
                                if (dialog.Save)
                                {
                                    encodedPassword = Cipher.Encrypt(password);
                                    Timotheus.Registry.Set("KeyPassword", encodedPassword);
                                }
                            }
                        }
                        break;
                    case ".txt":
                        data.LoadKey(keyPath);
                        break;
                }
            }
            catch (Exception ex)
            {
                await MessageBox.Show(this, ex.Message, Localization.Localization.Exception_NoKeys, MessageBox.MessageBoxButtons.OkCancel);
            }
        }

        /// <summary>
        /// Opens the window and retrieves last used key.
        /// </summary>
        public override void Show()
        {
            base.Show();
            if (!isShown)
            {
                StartUpKey();
                isShown = true;
            }
        }
        private static bool isShown = false;

        #region Calendar
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

        /// <summary>
        /// Updates the period according to the textbox.
        /// </summary>
        private void Period_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                data.calendarPeriod.SetPeriod(((TextBox)sender).Text);
                data.UpdateCalendarTable();
            }
        }

        /// <summary>
        /// Opens a OpenCalendar dialog
        /// </summary>
        private async void OpenCalendar_Click(object sender, RoutedEventArgs e)
        {
            OpenCalendar dialog = new();
            await dialog.ShowDialog(this);

            if (dialog.DialogResult == DialogResult.OK)
            {
                if (dialog.IsRemote)
                    data.Calendar = new(dialog.Username, dialog.Password, dialog.URL);
                else
                {
                    data.Calendar = new(dialog.Path);
                }
            }
        }

        /// <summary>
        /// Opens a SaveFileDialog to save the current Calendar.
        /// </summary>
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

        /// <summary>
        /// Opens a SyncCalendar dialog to sync the current calendar.
        /// </summary>
        private void SyncCalendar_Click(object sender, RoutedEventArgs e)
        {
            SyncCalendar.Show(this, data.Calendar, data.calendarPeriod);
            data.UpdateCalendarTable();
        }

        /// <summary>
        /// Opens a AddEvent dialog, and adds the result to the current calendar.
        /// </summary>
        private async void AddEvent_Click(object sender, RoutedEventArgs e)
        {
            Event ev = await AddEvent.Show(this);
            if (ev != null)
            {
                data.Calendar.Events.Add(ev);
                data.UpdateCalendarTable();
            }
        }

        /// <summary>
        /// Marks the selected event for deletion.
        /// </summary>
        private void RemoveEvent_Click(object sender, RoutedEventArgs e)
        {
            Event ev = (Event)((Button)e.Source).DataContext;
            if (ev != null)
            {
                data.Remove(ev);
            }
        }

        /// <summary>
        /// Opens a SaveFileDialog to export the current Calendar (in the given period) as a PDF.
        /// </summary>
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
                    PDF.ExportCalendar(events, file.DirectoryName, file.Name, data.Keys.Get("Settings-Name"), data.Keys.Get("Settings-Address"), data.Keys.Get("Settings-Image"), data.PeriodText);
                }
                catch (Exception ex)
                {
                    await MessageBox.Show(this, ex.Message, Localization.Localization.Exception_Saving, MessageBox.MessageBoxButtons.OkCancel);
                }
            }
        }
        #endregion

        #region Files
        /// <summary>
        /// Goes one level up from the currently visible directory.
        /// </summary>
        private void UpDirectory_Click(object sender, RoutedEventArgs e)
        {
            data.GoUpDirectory();
        }

        /// <summary>
        /// Synchronizes the files in the local and remote directory.
        /// </summary>
        private void SyncFiles_Click(object sender, RoutedEventArgs e)
        {
            data.Directory.Synchronize();
        }

        /// <summary>
        /// Opens a SetupSFTP dialog to define the SFTP parameters.
        /// </summary>
        private async void SetupFiles_Click(object sender, RoutedEventArgs e)
        {
            SetupSFTP dialog = new();
            bool result = await dialog.Show(this);
            if (result)
            {
                data.Directory = new IO.DirectoryManager(dialog.data.Local, dialog.data.Remote, dialog.data.Host, dialog.data.Username, dialog.data.Password);
                data.Keys.Set("SSH-LocalDirectory", dialog.data.Local);
                data.Keys.Set("SSH-RemoteDirectory", dialog.data.Remote);
                data.Keys.Set("SSH-URL", dialog.data.Host);
                data.Keys.Set("SSH-Username", dialog.data.Username);
                data.Keys.Set("SSH-Password", dialog.data.Password);
            }
        }

        /// <summary>
        /// Goes one level down into the selected directory.
        /// </summary>
        private void GoToDirectory_Click(object sender, RoutedEventArgs e)
        {
            var row = ((IControl)e.Source).GetSelfAndVisualAncestors()
                                .OfType<DataGridRow>()
                                .FirstOrDefault();

            if (row != null)
                data.GoToDirectory(row.GetIndex());
        }
        #endregion

        #region Toolstrip
        /// <summary>
        /// Clears the Calendar and Directory.
        /// </summary>
        private async void NewProject_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = await MessageBox.Show(this, Localization.Localization.ToolStrip_NewSecure, Localization.Localization.ToolStrip_NewFile, MessageBox.MessageBoxButtons.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                Timotheus.Registry.Remove("KeyPath");
                Timotheus.Registry.Remove("KeyPassword");
                data.Keys = new(':');
            }
        }

        /// <summary>
        /// Opens a SaveFileDialog so the user can save the current key as a file.
        /// </summary>
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
                        PasswordDialog dialog = new();
                        bool p = await dialog.Show(this);
                        if (p)
                        {
                            string password = dialog.Password;
                            try
                            {
                                data.SaveKey(result, password);
                            }
                            catch (Exception ex)
                            {
                                await MessageBox.Show(this, ex.Message, Localization.Localization.Exception_Saving, MessageBox.MessageBoxButtons.OkCancel);
                            }
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

        /// <summary>
        /// Opens an OpenFileDialog so the user can open a key file.
        /// </summary>
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
                        PasswordDialog dialog = new();
                        bool p = await dialog.Show(this);
                        if (p)
                        {
                            string password = dialog.Password;
                            if (dialog.Save)
                            {
                                string encodedPassword = Cipher.Encrypt(password);
                                Timotheus.Registry.Set("KeyPassword", encodedPassword);
                            }
                            else
                                Timotheus.Registry.Remove("KeyPassword");
                            try
                            {
                                data.LoadKey(result[0], password);
                            }
                            catch (Exception ex)
                            {
                                await MessageBox.Show(this, ex.Message, Localization.Localization.Exception_LoadFailed, MessageBox.MessageBoxButtons.OkCancel);
                            }
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

        /// <summary>
        /// Opens an EditKey dialog where the user can change the values of the key.
        /// </summary>
        private async void EditKey_Click(object sender, RoutedEventArgs e)
        {
            data.Keys = await EditKey.Show(this, data.Keys.ToString());
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            Help dialog = new();
            dialog.Show(this);
        }
        #endregion
    }
}