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
    public partial class CalendarPage : UserControl
    {
        private MainViewModel MVM
        {
            get
            {
                return DataContext as MainViewModel;
            }
        }

        public CalendarPage()
        {
            AvaloniaXamlLoader.Load(this);
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
                    MVM.UpdatePeriod(true);
                else if (button.Name == "-")
                    MVM.UpdatePeriod(false);
            }
        }

        /// <summary>
        /// Updates the period according to the textbox.
        /// </summary>
        private void Period_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Avalonia.Input.Key.Enter)
            {
                MVM.UpdatePeriod(((TextBox)sender).Text);
                MVM.UpdateCalendarTable();
            }
        }

        /// <summary>
        /// Opens a SyncCalendar dialog to sync the current calendar.
        /// </summary>
        private async void SyncCalendar_Click(object sender, RoutedEventArgs e)
        {
            SyncCalendar dialog = new()
            {
                Period = MVM.PeriodText,
                UseCurrent = MVM.Calendar.IsSetup(),
                CanUseCurrent = MVM.Calendar.IsSetup()
            };

            await dialog.ShowDialog(MainWindow.Instance);

            if (dialog.DialogResult == DialogResult.OK)
            {
                try
                {
                    if (!dialog.UseCurrent)
                    {
                        MVM.Calendar.SetupSync(dialog.Username, dialog.Password, dialog.URL);
                        MVM.Keys.Update("Calendar-Email", dialog.Username);
                        MVM.Keys.Update("Calendar-Password", dialog.Password);
                        MVM.Keys.Update("Calendar-URL", dialog.URL);
                    }

                    ProgressDialog pDialog = new();
                    pDialog.Title = Localization.Localization.SyncCalendar_Worker;
                    Period syncPeriod;
                    if (dialog.SyncAll)
                        syncPeriod = new Period(DateTime.MinValue, DateTime.MaxValue);
                    else if (dialog.SyncPeriod)
                        syncPeriod = new Period(MVM.PeriodText);
                    else
                        syncPeriod = new Period(dialog.Start, dialog.End.AddDays(1));

                    await pDialog.ShowDialog(MainWindow.Instance, MVM.Calendar.Sync, syncPeriod);

                    MVM.UpdateCalendarTable();
                }
                catch (Exception ex)
                {
                    MainWindow.Instance.Error(Localization.Localization.Exception_Sync, ex.Message);
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
            if ((text = MVM.Keys.Retrieve("Settings-Address")) != string.Empty)
                dialog.Location = text;
            if ((text = MVM.Keys.Retrieve("Settings-EventDescription")) != string.Empty)
                dialog.Description = text;
            if ((text = MVM.Keys.Retrieve("Settings-EventStart")) != string.Empty)
                dialog.StartTime = text;
            if ((text = MVM.Keys.Retrieve("Settings-EventEnd")) != string.Empty)
                dialog.EndTime = text;

            await dialog.ShowDialog(MainWindow.Instance);

            if (dialog.DialogResult == DialogResult.OK)
            {
                try
                {
                    MVM.Calendar.Events.Add(new Event(dialog.Start, dialog.End, dialog.EventName, dialog.Description, dialog.Location, string.Empty));
                    MVM.UpdateCalendarTable();
                }
                catch (Exception ex)
                {
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
                MVM.Remove(ev);
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

            string result = await saveFileDialog.ShowAsync(MainWindow.Instance);

            if (result != null)
            {
                try
                {
                    FileInfo file = new(result);
                    MVM.ExportCalendar(file.Name, file.DirectoryName);
                }
                catch (Exception ex)
                {
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

                        MVM.UpdateCalendarTable();
                    }
                    catch (Exception ex)
                    {
                        MainWindow.Instance.Error(Localization.Localization.Exception_InvalidEvent, ex.Message);
                    }
                }
            }
        }
    }
}