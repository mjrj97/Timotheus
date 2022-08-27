using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.IO;
using System.Collections.Generic;
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
                    Program.Error(Localization.Localization.Exception_Sync, ex, MainWindow.Instance);
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
                    Program.Error(Localization.Localization.Exception_Saving, ex, MainWindow.Instance);
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
                    Program.Error(Localization.Localization.Exception_InvalidCalendar, ex, MainWindow.Instance);
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
                    Program.Error(Localization.Localization.Exception_InvalidEvent, ex, MainWindow.Instance);
                }
            }
        }

        /// <summary>
        /// Marks the selected event for deletion.
        /// </summary>
        private async void RemoveEvent_Click(object sender, RoutedEventArgs e)
        {
            EventViewModel ev = (EventViewModel)((Button)e.Source).DataContext;
            if (ev != null)
            {
                MessageBox msDialog = new()
                {
                    DialogTitle = Localization.Localization.Exception_Warning,
                    DialogText = Localization.Localization.Calendar_DeleteEvent.Replace("#", ev.Name)
                };
                await msDialog.ShowDialog(MainWindow.Instance);
                if (msDialog.DialogResult == DialogResult.OK)
                {
                    Calendar.RemoveEvent(ev);
                }
            }
        }

        /// <summary>
        /// Opens a SaveFileDialog to export the current Calendar (in the given period) as a PDF.
        /// </summary>
        private async void ExportPDF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PDFDialog dialog = new()
                {
                    LogoPath = MainViewModel.Instance.Keys.Retrieve("PDF_Logo"),
                    PDFTitle = MainViewModel.Instance.Keys.Retrieve("PDF_Title"),
                    Subtitle = MainViewModel.Instance.Keys.Retrieve("PDF_Subtitle"),
                    Footer = MainViewModel.Instance.Keys.Retrieve("PDF_Footer"),
                    Backpage = MainViewModel.Instance.Keys.Retrieve("PDF_Backpage"),
                    Comment = MainViewModel.Instance.Keys.Retrieve("PDF_Comment"),
                    ExportPath = MainViewModel.Instance.Keys.Retrieve("PDF_ExportPath"),
                    ArchivePath = MainViewModel.Instance.Keys.Retrieve("PDF_ArchivePath")
                };

                await dialog.ShowDialog(MainWindow.Instance);

                bool Changed = false;

                Changed |= MainViewModel.Instance.Keys.Update("PDF_Logo", dialog.LogoPath);
                Changed |= MainViewModel.Instance.Keys.Update("PDF_Title", dialog.PDFTitle);
                Changed |= MainViewModel.Instance.Keys.Update("PDF_Subtitle", dialog.Subtitle);
                Changed |= MainViewModel.Instance.Keys.Update("PDF_Footer", dialog.Footer);
                Changed |= MainViewModel.Instance.Keys.Update("PDF_Backpage", dialog.Backpage);
                Changed |= MainViewModel.Instance.Keys.Update("PDF_Comment", dialog.Comment);
                Changed |= MainViewModel.Instance.Keys.Update("PDF_ExportPath", dialog.ExportPath);
                Changed |= MainViewModel.Instance.Keys.Update("PDF_ArchivePath", dialog.ArchivePath);

                if (Changed)
                {
                    MessageBox messageBox = new()
                    {
                        DialogTitle = Localization.Localization.InsertKey_ChangeDetected,
                        DialogText = Localization.Localization.InsertKey_DoYouWantToSave
                    };
                    await messageBox.ShowDialog(MainWindow.Instance);
                    if (messageBox.DialogResult == DialogResult.OK)
                    {
                        MainWindow.Instance.SaveKey_Click(null, null);
                    }
                }

                if (dialog.DialogResult == DialogResult.OK)
                {
                    FileInfo file = new(dialog.ExportPath);

                    List<Event> events = new();
                    List<Event> cal = Calendar.Calendar.Events;
                    for (int i = 0; i < cal.Count; i++)
                    {
                        if (cal[i].In(Calendar.CalendarPeriod))
                            events.Add(cal[i]);
                    }

                    string tabName = string.Empty;
                    if (!Directory.Exists(Path.GetDirectoryName(dialog.ExportPath)))
                        Directory.CreateDirectory(Path.GetDirectoryName(dialog.ExportPath));
                    switch (dialog.CurrentTab) {
                        case 0:
                            PDF.CreateTable(events, dialog.ExportPath, dialog.PDFTitle, dialog.Subtitle, dialog.Footer, dialog.LogoPath);
                            tabName = Localization.Localization.PDF_Type_Table;
                            break;
                        case 1:
                            PDF.CreateBook(events, dialog.ExportPath, dialog.PDFTitle, dialog.Subtitle, dialog.Comment, dialog.Backpage, dialog.LogoPath);
                            tabName = Localization.Localization.PDF_Type_Book;
                            break;
                    }

                    if (dialog.SaveToArchive)
                    {
                        if (!Directory.Exists(dialog.ArchivePath))
                            throw new Exception(Localization.Localization.Exception_PDFArchiveNotFound);
                        if (dialog.ArchivePath != string.Empty)
                            File.Copy(dialog.ExportPath, Path.Combine(dialog.ArchivePath, Calendar.CalendarPeriod.ToFileName() + " (" + tabName + ")" + ".pdf"), true);
                    }
                }
            }
            catch (Exception ex)
            {
                Program.Error(Localization.Localization.Exception_Saving, ex, MainWindow.Instance);
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
                        Program.Error(Localization.Localization.Exception_InvalidEvent, ex, MainWindow.Instance);
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
                    Program.Log(ex);
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