using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.IO;
using Timotheus.Schedule;
using Timotheus.Utility;
using Timotheus.ViewModels;
using Timotheus.Views.Dialogs;

namespace Timotheus.Views.Tabs
{
    public partial class CalendarPage : Tab
    {
        /// <summary>
        /// A SFTP object connecting a local and remote directory.
        /// </summary>
        public CalendarViewModel Calendar
        {
            get
            {
                return (CalendarViewModel)ViewModel;
            }
            set
            {
                ViewModel = value;
            }
        }

        public CalendarPage()
        {
            LoadingTitle = Localization.Localization.InsertKey_LoadCalendar;
            AvaloniaXamlLoader.Load(this);
            DataContext = Calendar;
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
                    Calendar.UpdatePeriod(true);
                else if (button.Name == "-")
                    Calendar.UpdatePeriod(false);
            }
        }

        /// <summary>
        /// Updates the period according to the textbox.
        /// </summary>
        private void Period_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Avalonia.Input.Key.Enter)
            {
                Calendar.UpdatePeriod(((TextBox)sender).Text);
                Update();
            }
        }

        /// <summary>
        /// Opens a SyncCalendar dialog to sync the current calendar.
        /// </summary>
        private async void SyncCalendar_Click(object sender, RoutedEventArgs e)
        {
            SyncCalendar dialog = new()
            {
                Period = Calendar.PeriodText,
                UseCurrent = Calendar.IsSetup,
                CanUseCurrent = Calendar.IsSetup
            };

            await dialog.ShowDialog(MainWindow.Instance);

            if (dialog.DialogResult == DialogResult.OK)
            {
                try
                {
                    if (!dialog.UseCurrent)
                    {
                        Calendar.SetupSync(dialog.Username, dialog.Password, dialog.URL);
                        MainViewModel.Instance.Keys.Update("Calendar-Email", dialog.Username);
                        MainViewModel.Instance.Keys.Update("Calendar-Password", dialog.Password);
                        MainViewModel.Instance.Keys.Update("Calendar-URL", dialog.URL);
                    }

                    ProgressDialog pDialog = new()
                    {
                        Title = Localization.Localization.SyncCalendar_Worker
                    };
                    Period syncPeriod;
                    if (dialog.SyncAll)
                        syncPeriod = new Period(DateTime.MinValue, DateTime.MaxValue);
                    else if (dialog.SyncPeriod)
                        syncPeriod = new Period(Calendar.PeriodText);
                    else
                        syncPeriod = new Period(dialog.Start, dialog.End.AddDays(1));

                    await pDialog.ShowDialog(MainWindow.Instance, Calendar.Sync, syncPeriod);

                    Update();
                }
                catch (Exception ex)
                {
                    Timotheus.Log(ex);
                    MainWindow.Instance.Error(Localization.Localization.Exception_Sync, ex.Message);
                }
            }
        }

        /// <summary>
        /// Opens a SaveFileDialog to save the current Calendar.
        /// </summary>
        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new();
            FileDialogFilter filter = new();
            string result;

            filter.Extensions.Add("ics");
            filter.Name = "Calendar (.ics)";

            saveFileDialog.Filters = new()
            {
                filter
            };

            result = await saveFileDialog.ShowAsync(MainWindow.Instance);
            if (result != null)
            {
                try
                {
                    Calendar.Save(result);
                }
                catch (Exception ex)
                {
                    Timotheus.Log(ex);
                    MainWindow.Instance.Error(Localization.Localization.Exception_Saving, ex.Message);
                }
            }
        }

        /// <summary>
        /// Opens a OpenCalendar dialog
        /// </summary>
        private async void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenCalendar dialog = new()
            {
                Username = MainViewModel.Instance.Keys.Retrieve("Calendar-Email"),
                Password = MainViewModel.Instance.Keys.Retrieve("Calendar-Password"),
                URL = MainViewModel.Instance.Keys.Retrieve("Calendar-URL"),
                Path = MainViewModel.Instance.Keys.Retrieve("Calendar-Path")
            };

            await dialog.ShowDialog(MainWindow.Instance);

            if (dialog.DialogResult == DialogResult.OK)
            {
                try
                {
                    if (dialog.IsRemote)
                    {
                        Calendar = new(dialog.Username, dialog.Password, dialog.URL);
                        MainViewModel.Instance.Keys.Update("Calendar-Email", dialog.Username);
                        MainViewModel.Instance.Keys.Update("Calendar-Password", dialog.Password);
                        MainViewModel.Instance.Keys.Update("Calendar-URL", dialog.URL);
                        Update();
                    }
                    else
                    {
                        Calendar = new(dialog.Path);
                        MainViewModel.Instance.Keys.Update("Calendar-Path", dialog.Path);
                        Update();
                    }
                }
                catch (Exception ex)
                {
                    Timotheus.Log(ex);
                    MainWindow.Instance.Error(Localization.Localization.Exception_InvalidCalendar, ex.Message);
                }
            }
        }

        /// <summary>
        /// Opens a AddEvent dialog, and adds the result to the current calendar.
        /// </summary>
        private async void AddEvent_Click(object sender, RoutedEventArgs e)
        {
            AddEvent dialog = new();

            string text;
            if ((text = MainViewModel.Instance.Keys.Retrieve("Settings-Address")) != string.Empty)
                dialog.Location = text;
            if ((text = MainViewModel.Instance.Keys.Retrieve("Settings-EventDescription")) != string.Empty)
                dialog.Description = text;
            if ((text = MainViewModel.Instance.Keys.Retrieve("Settings-EventStart")) != string.Empty)
                dialog.StartTime = text;
            if ((text = MainViewModel.Instance.Keys.Retrieve("Settings-EventEnd")) != string.Empty)
                dialog.EndTime = text;

            Period period = new(Calendar.PeriodText);
            if (!period.In(DateTime.Now))
            {
                dialog.Start = period.Start.Date;
                dialog.End = period.Start.Date;
            }

            await dialog.ShowDialog(MainWindow.Instance);

            if (dialog.DialogResult == DialogResult.OK)
            {
                try
                {
                    Calendar.AddEvent(dialog.Start, dialog.End, dialog.EventName, dialog.Description, dialog.Location, string.Empty);
                    Update();
                }
                catch (Exception ex)
                {
                    Timotheus.Log(ex);
                    MainWindow.Instance.Error(Localization.Localization.Exception_InvalidEvent, ex.Message);
                }
            }
        }

        /// <summary>
        /// Marks the selected event for deletion.
        /// </summary>
        private void RemoveEvent_Click(object sender, RoutedEventArgs e)
        {
            EventViewModel ev = (EventViewModel)((Button)e.Source).DataContext;
            if (ev != null)
            {
                Calendar.RemoveEvent(ev);
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

            saveFileDialog.Filters = new()
            {
                filter
            };

            string result = await saveFileDialog.ShowAsync(MainWindow.Instance);

            if (result != null)
            {
                try
                {
                    FileInfo file = new(result);
                    Calendar.ExportCalendar(file.Name, file.DirectoryName);
                }
                catch (Exception ex)
                {
                    Timotheus.Log(ex);
                    MainWindow.Instance.Error(Localization.Localization.Exception_Saving, ex.Message);
                }
            }
        }

        /// <summary>
        /// Marks the selected event for deletion.
        /// </summary>
        private async void EditEvent_Click(object sender, RoutedEventArgs e)
        {
            EventViewModel ev = (EventViewModel)((Button)e.Source).DataContext;
            if (ev != null)
            {
                AddEvent dialog = new()
                {
                    Title = Localization.Localization.AddEvent_Edit,
                    ButtonName = Localization.Localization.AddEvent_EditButton,
                    EventName = ev.Name,
                    Start = ev.StartSort,
                    End = ev.EndSort,
                    AllDayEvent = ev.AllDayEvent,
                    Location = ev.Location,
                    Description = ev.Description
                };

                await dialog.ShowDialog(MainWindow.Instance);

                if (dialog.DialogResult == DialogResult.OK)
                {
                    try
                    {
                        ev.Name = dialog.EventName;
                        ev.Start = dialog.Start.ToString();
                        ev.End = dialog.End.ToString();
                        ev.Location = dialog.Location;
                        ev.Description = dialog.Description;

                        Update();
                    }
                    catch (Exception ex)
                    {
                        Timotheus.Log(ex);
                        MainWindow.Instance.Error(Localization.Localization.Exception_InvalidEvent, ex.Message);
                    }
                }
            }
        }

        public override void Load()
        {
            if (MainViewModel.Instance.Keys.Retrieve("Calendar-Email") != string.Empty)
            {
                try
                {
                    Calendar = new(MainViewModel.Instance.Keys.Retrieve("Calendar-Email"), MainViewModel.Instance.Keys.Retrieve("Calendar-Password"), MainViewModel.Instance.Keys.Retrieve("Calendar-URL"));
                }
                catch (Exception ex)
                {
                    Timotheus.Log(ex);
                    Calendar = new();
                }
            }
            else
                Calendar = new();
        }

        public override void Update()
        {
            Calendar.UpdateCalendarTable();
        }

        public override bool HasBeenChanged()
        {
            return Calendar.HasBeenChanged;
        }
    }
}