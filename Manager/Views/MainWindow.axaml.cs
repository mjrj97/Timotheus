using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using System;
using System.IO;
using System.Linq;
using Timotheus.Schedule;
using Timotheus.Utility;
using Timotheus.ViewModels;

namespace Timotheus.Views
{
    /// <summary>
    /// MainWindow of the Timotheus app. Contains Calendar and Filesharing tabs.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Data Context of the MainWindow.
        /// </summary>
        private readonly MainViewModel mvm;

        /// <summary>
        /// Constructor. Creates datacontext and loads XAML.
        /// </summary>
        public MainWindow()
        {
            mvm = new();
            AvaloniaXamlLoader.Load(this);
            DataContext = mvm;
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
                            mvm.LoadKey(keyPath, password);
                        }
                        else
                        {
                            PasswordDialog dialog = new();
                            bool result = await dialog.Show(this);
                            if (result)
                            {
                                password = dialog.Password;
                                mvm.LoadKey(keyPath, password);
                                if (dialog.Save)
                                {
                                    encodedPassword = Cipher.Encrypt(password);
                                    Timotheus.Registry.Set("KeyPassword", encodedPassword);
                                }
                            }
                        }
                        break;
                    case ".txt":
                        mvm.LoadKey(keyPath);
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
                    mvm.UpdatePeriod(true);
                else if (button.Name == "-")
                    mvm.UpdatePeriod(false);
            }
        }

        /// <summary>
        /// Updates the period according to the textbox.
        /// </summary>
        private void Period_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                mvm.UpdatePeriod(((TextBox)sender).Text);
                mvm.UpdateCalendarTable();
            }
        }

        /// <summary>
        /// Opens a OpenCalendar dialog
        /// </summary>
        private async void OpenCalendar_Click(object sender, RoutedEventArgs e)
        {
            OpenCalendar dialog = new();
            dialog.Username = mvm.Keys.Get("Calendar-Email");
            dialog.Password = mvm.Keys.Get("Calendar-Password");
            dialog.URL = mvm.Keys.Get("Calendar-URL");
            dialog.Path = mvm.Keys.Get("Calendar-Path");

            await dialog.ShowDialog(this);

            if (dialog.DialogResult == DialogResult.OK)
            {
                try
                {
                    if (dialog.IsRemote)
                    {
                        mvm.Calendar = new(dialog.Username, dialog.Password, dialog.URL);
                        mvm.Keys.Set("Calendar-Email", dialog.Username);
                        mvm.Keys.Set("Calendar-Password", dialog.Password);
                        mvm.Keys.Set("Calendar-URL", dialog.URL);
                    }
                    else
                    {
                        mvm.Calendar = new(dialog.Path);
                        mvm.Keys.Set("Calendar-Path", dialog.Path);
                    }
                }
                catch (Exception ex)
                {
                    await MessageBox.Show(this, ex.Message, Localization.Localization.Exception_InvalidCalendar, MessageBox.MessageBoxButtons.OkCancel);
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
                    mvm.Calendar.Save(result);
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
        private async void SyncCalendar_Click(object sender, RoutedEventArgs e)
        {
            SyncCalendar dialog = new();
            dialog.Period = mvm.PeriodText;
            dialog.UseCurrent = mvm.Calendar.IsSetup();
            dialog.CanUseCurrent = mvm.Calendar.IsSetup();

            await dialog.ShowDialog(this);

            if (dialog.DialogResult == DialogResult.OK)
            {
                try
                {
                    if (!dialog.UseCurrent)
                    {
                        mvm.Calendar.SetupSync(dialog.Username, dialog.Password, dialog.URL);
                        mvm.Keys.Set("Calendar-Email", dialog.Username);
                        mvm.Keys.Set("Calendar-Password", dialog.Password);
                        mvm.Keys.Set("Calendar-URL", dialog.URL);
                    }

                    if (dialog.SyncAll)
                        mvm.Calendar.Sync();
                    else if (dialog.SyncPeriod)
                        mvm.Calendar.Sync(new Period(mvm.PeriodText));
                    else
                        mvm.Calendar.Sync(new Period(dialog.Start, dialog.End.AddDays(1)));

                    mvm.UpdateCalendarTable();
                }
                catch (Exception ex)
                {
                    await MessageBox.Show(this, ex.Message, Localization.Localization.Exception_Sync, MessageBox.MessageBoxButtons.OkCancel);
                }
            }
        }

        /// <summary>
        /// Opens a AddEvent dialog, and adds the result to the current calendar.
        /// </summary>
        private async void AddEvent_Click(object sender, RoutedEventArgs e)
        {
            AddEvent dialog = new();
            await dialog.ShowDialog(this);

            if (dialog.DialogResult == DialogResult.OK)
            {
                try
                {
                    DateTime Start = new(int.Parse(dialog.StartYear), dialog.StartMonth + 1, int.Parse(dialog.StartDay));
                    DateTime End = new(int.Parse(dialog.EndYear), dialog.EndMonth + 1, int.Parse(dialog.EndDay));

                    if (!dialog.AllDayEvent)
                    {
                        int hour, minute;

                        hour = int.Parse(dialog.StartTime.Substring(0, -3 + dialog.StartTime.Length));
                        minute = int.Parse(dialog.StartTime.Substring(-2 + dialog.StartTime.Length, 2));
                        Start = Start.AddMinutes(minute + hour * 60);

                        hour = int.Parse(dialog.EndTime.Substring(0, -3 + dialog.EndTime.Length));
                        minute = int.Parse(dialog.EndTime.Substring(-2 + dialog.EndTime.Length, 2));
                        End = End.AddMinutes(minute + hour * 60);
                    }

                    mvm.Calendar.Events.Add(new Event(Start, End, dialog.EventName, dialog.Description, dialog.Location, string.Empty));
                    mvm.UpdateCalendarTable();
                }
                catch (Exception ex)
                {
                    await MessageBox.Show(this, ex.Message, Localization.Localization.Exception_InvalidEvent, MessageBox.MessageBoxButtons.OkCancel);
                }
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
                mvm.Remove(ev);
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
                    mvm.ExportCalendar(file.Name, file.DirectoryName);
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
        private async void UpDirectory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                mvm.GoUpDirectory();
            }
            catch (Exception ex)
            {
                await MessageBox.Show(this, ex.Message, Localization.Localization.Exception_Name, MessageBox.MessageBoxButtons.OkCancel);
            }
        }

        /// <summary>
        /// Synchronizes the files in the local and remote directory.
        /// </summary>
        private async void SyncFiles_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                mvm.Directory.Synchronize();
                mvm.GoToDirectory(mvm.Directory.RemotePath);
            }
            catch (Exception ex)
            {
                await MessageBox.Show(this, ex.Message, Localization.Localization.Exception_Name, MessageBox.MessageBoxButtons.OkCancel);
            }
        }

        /// <summary>
        /// Opens a SetupSFTP dialog to define the SFTP parameters.
        /// </summary>
        private async void SetupFiles_Click(object sender, RoutedEventArgs e)
        {
            SetupSFTP dialog = new();
            dialog.Local = mvm.Keys.Get("SSH-LocalDirectory");
            dialog.Remote = mvm.Keys.Get("SSH-RemoteDirectory");
            dialog.Host = mvm.Keys.Get("SSH-URL");
            dialog.Username = mvm.Keys.Get("SSH-Username");
            dialog.Password = mvm.Keys.Get("SSH-Password");

            await dialog.ShowDialog(this);
            if (dialog.DialogResult == DialogResult.OK)
            {
                try
                {
                    mvm.Directory = new IO.DirectoryManager(dialog.Local, dialog.Remote, dialog.Host, dialog.Username, dialog.Password);
                    mvm.Keys.Set("SSH-LocalDirectory", dialog.Local);
                    mvm.Keys.Set("SSH-RemoteDirectory", dialog.Remote);
                    mvm.Keys.Set("SSH-URL", dialog.Host);
                    mvm.Keys.Set("SSH-Username", dialog.Username);
                    mvm.Keys.Set("SSH-Password", dialog.Password);
                }
                catch (Exception ex)
                {
                    await MessageBox.Show(this, ex.Message, Localization.Localization.Exception_Name, MessageBox.MessageBoxButtons.OkCancel);
                }
            }
        }

        /// <summary>
        /// Goes one level down into the selected directory.
        /// </summary>
        private async void GoToDirectory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var row = ((IControl)e.Source).GetSelfAndVisualAncestors()
                                .OfType<DataGridRow>()
                                .FirstOrDefault();

                if (row != null)
                    mvm.GoToDirectory(row.GetIndex());
            }
            catch (Exception ex)
            {
                await MessageBox.Show(this, ex.Message, Localization.Localization.Exception_Name, MessageBox.MessageBoxButtons.OkCancel);
            }
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
                mvm.Keys = new(':');
            }
        }

        /// <summary>
        /// Opens a SaveFileDialog so the user can save the current key as a file.
        /// </summary>
        private async void SaveKey_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new();
            saveFileDialog.Filters = new();

            FileDialogFilter tkeyFilter = new();
            tkeyFilter.Extensions.Add("tkey");
            tkeyFilter.Name = "Encrypted key (.tkey)";
            saveFileDialog.Filters.Add(tkeyFilter);

            FileDialogFilter txtFilter = new();
            txtFilter.Extensions.Add("txt");
            txtFilter.Name = "Text files (.txt)";
            saveFileDialog.Filters.Add(txtFilter);

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
                                mvm.SaveKey(result, password);
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
                            mvm.SaveKey(result);
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
                                mvm.LoadKey(result[0], password);
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
                            mvm.LoadKey(result[0]);
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
            EditKey dialog = new();
            dialog.Text = mvm.Keys.ToString();
            await dialog.ShowDialog(this);
            if (dialog.DialogResult == DialogResult.OK)
            {
                try
                {
                    mvm.Keys = new IO.Register(':', dialog.Text);
                }
                catch (Exception ex)
                {
                    await MessageBox.Show(this, ex.Message, Localization.Localization.Exception_Name, MessageBox.MessageBoxButtons.OkCancel);
                }
            }
        }

        /// <summary>
        /// Opens a Help dialog.
        /// </summary>
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            Help dialog = new();
            dialog.Show(this);
        }

        /// <summary>
        /// Opens a Settings dialog.
        /// </summary>
        private async void Settings_Click(object sender, RoutedEventArgs e)
        {
            Settings dialog = new();

            dialog.AssociationName = mvm.Keys.Get("Settings-Name");
            dialog.AssociationAddress = mvm.Keys.Get("Settings-Address");
            dialog.ImagePath = mvm.Keys.Get("Settings-Image");

            await dialog.ShowDialog(this);
            if (dialog.DialogResult == DialogResult.OK)
            {
                mvm.Keys.Set("Settings-Name", dialog.AssociationName);
                mvm.Keys.Set("Settings-Address", dialog.AssociationAddress);
                mvm.Keys.Set("Settings-Image", dialog.ImagePath);
            }
        }
        #endregion
    }
}