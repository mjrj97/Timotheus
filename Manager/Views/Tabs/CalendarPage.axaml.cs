using Avalonia.Input;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using System;
using System.IO;
using System.Collections.Generic;
using Timotheus.Schedule;
using Timotheus.Utility;
using Timotheus.ViewModels;
using Timotheus.Views.Dialogs;
using Timotheus.IO;

namespace Timotheus.Views.Tabs
{
    public partial class CalendarPage : Tab
    {
        /// <summary>
        /// The object presenting the calendar to the view.
        /// </summary>
        public new CalendarViewModel ViewModel
        {
            get
            {
                return (CalendarViewModel)base.ViewModel;
            }
            set
            {
                value.UpdateCalendarTable();
                base.ViewModel = value;
            }
        }

        public CalendarPage() : base()
        {
            AvaloniaXamlLoader.Load(this);
            Title = Localization.Calendar_Page;
            LoadingTitle = Localization.InsertKey_LoadCalendar;
            Icon = "avares://Timotheus/Resources/Calendar.png";
            ViewModel = new CalendarViewModel();
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
                    ViewModel.UpdatePeriod(true);
                else if (button.Name == "-")
                    ViewModel.UpdatePeriod(false);
            }
        }

        /// <summary>
        /// Updates the period according to the textbox.
        /// </summary>
        private void Period_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Avalonia.Input.Key.Enter)
            {
                ViewModel.UpdatePeriod(((TextBox)sender).Text);
                ViewModel.UpdateCalendarTable();
            }
        }

        /// <summary>
        /// Opens a SyncCalendar dialog to sync the current calendar.
        /// </summary>
        private async void SyncCalendar_Click(object sender, RoutedEventArgs e)
        {
            SyncCalendar dialog = new()
            {
                Period = ViewModel.PeriodText,
                UseCurrent = ViewModel.IsSetup,
                CanUseCurrent = ViewModel.IsSetup
            };

            await dialog.ShowDialog(MainWindow.Instance);

            if (dialog.DialogResult == DialogResult.OK)
            {
                try
                {
                    if (!dialog.UseCurrent)
                    {
                        ViewModel.SetupSync(dialog.Username, dialog.Password, dialog.URL);
                        Keys.Update("Calendar-Email", dialog.Username);
                        Keys.Update("Calendar-Password", dialog.Password);
                        Keys.Update("Calendar-URL", dialog.URL);
                    }

                    ProgressDialog pDialog = new()
                    {
                        Title = Localization.SyncCalendar_Worker
                    };
                    Period syncPeriod;
                    if (dialog.SyncAll)
                        syncPeriod = new Period(DateTime.MinValue, DateTime.MaxValue);
                    else if (dialog.SyncPeriod)
                        syncPeriod = new Period(ViewModel.PeriodText);
                    else
                        syncPeriod = new Period(dialog.Start, dialog.End.AddDays(1));

                    await pDialog.ShowDialog(MainWindow.Instance, ViewModel.Sync, syncPeriod);

                    ViewModel.UpdateCalendarTable();
                }
                catch (Exception ex)
                {
                    Program.Error(Localization.Exception_Sync, ex, MainWindow.Instance);
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
                    ViewModel.Save(result);

					MessageDialog confirmation = new()
					{
						DialogTitle = Localization.Confirmation,
						DialogText = Localization.Confirmation_SaveCalendar,
						DialogShowCancel = false
					};
					await confirmation.ShowDialog(MainWindow.Instance);
				}
                catch (Exception ex)
                {
                    Program.Error(Localization.Exception_Saving, ex, MainWindow.Instance);
                }
            }
        }

		/// <summary>
		/// Opens a SaveFileDialog to save the current Calendar.
		/// </summary>
		private void SaveAs_Click(object sender, RoutedEventArgs e)
		{
			
		}

		/// <summary>
		/// Opens a OpenCalendar dialog
		/// </summary>
		private async void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenCalendar dialog = new()
            {
                Username = Keys.Retrieve("Calendar-Email"),
                Password = Keys.Retrieve("Calendar-Password"),
                URL = Keys.Retrieve("Calendar-URL"),
                Path = Keys.Retrieve("Calendar-Path")
            };

            await dialog.ShowDialog(MainWindow.Instance);

            if (dialog.DialogResult == DialogResult.OK)
            {
                try
                {
                    if (dialog.IsRemote)
                    {
                        ViewModel = new(dialog.Username, dialog.Password, dialog.URL);
                        Keys.Update("Calendar-Email", dialog.Username);
                        Keys.Update("Calendar-Password", dialog.Password);
                        Keys.Update("Calendar-URL", dialog.URL);
                    }
                    else
                    {
                        ViewModel = new(dialog.Path);
                        Keys.Update("Calendar-Path", dialog.Path);
                    }
                }
                catch (Exception ex)
                {
                    Program.Error(Localization.Exception_InvalidCalendar, ex, MainWindow.Instance);
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
            if ((text = Keys.Retrieve("Settings-Address")) != string.Empty)
                dialog.Location = text;
            if ((text = Keys.Retrieve("Settings-EventDescription")) != string.Empty)
                dialog.Description = text;
            if ((text = Keys.Retrieve("Settings-EventStart")) != string.Empty)
                dialog.StartTime = text;
            if ((text = Keys.Retrieve("Settings-EventEnd")) != string.Empty)
                dialog.EndTime = text;

            Period period = new(ViewModel.PeriodText);
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
                    ViewModel.AddEvent(dialog.Start, dialog.End, dialog.EventName, dialog.Description, dialog.Location, string.Empty);
                    ViewModel.UpdateCalendarTable();
                }
                catch (Exception ex)
                {
                    Program.Error(Localization.Exception_InvalidEvent, ex, MainWindow.Instance);
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
                WarningDialog msDialog = new()
                {
                    DialogTitle = Localization.Exception_Warning,
                    DialogText = Localization.Calendar_DeleteEvent.Replace("#", ev.Name)
                };
                await msDialog.ShowDialog(MainWindow.Instance);
                if (msDialog.DialogResult == DialogResult.OK)
                {
                    ViewModel.RemoveEvent(ev);
                }
            }
        }
		
        /// <summary>
		/// Marks the selected event for deletion.
		/// </summary>
		private void UndoRemoveEvent_Click(object sender, RoutedEventArgs e)
		{
			EventViewModel ev = (EventViewModel)((Button)e.Source).DataContext;
			if (ev != null)
			{
				ViewModel.RestoreEvent(ev);
			}
		}

		/// <summary>
		/// Marks the selected event for deletion.
		/// </summary>
		private async void RemoveEvent_ContextMenu_Click(object sender, RoutedEventArgs e)
		{
			EventViewModel ev = ViewModel.Selected;
			if (sender != null)
			{
				try
				{
					WarningDialog msDialog = new()
					{
						DialogTitle = Localization.Exception_Warning,
						DialogText = Localization.Calendar_DeleteEvent.Replace("#", ev.Name)
					};
					await msDialog.ShowDialog(MainWindow.Instance);
					if (msDialog.DialogResult == DialogResult.OK)
					{
						ViewModel.RemoveEvent(ev);
					}
				}
				catch (Exception ex)
				{
					Program.Error(Localization.Exception_Name, ex, MainWindow.Instance);
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
                    LogoPath = Keys.Retrieve("PDF_Logo"),
                    PDFTitle = Keys.Retrieve("PDF_Title"),
                    Subtitle = Keys.Retrieve("PDF_Subtitle"),
                    Footer = Keys.Retrieve("PDF_Footer"),
                    Backpage = Keys.Retrieve("PDF_Backpage"),
                    Comment = Keys.Retrieve("PDF_Comment"),
                    ExportPath = Keys.Retrieve("PDF_ExportPath"),
                    ArchivePath = Keys.Retrieve("PDF_ArchivePath")
				};

                if (Keys.Retrieve("PDF_Columns") != string.Empty)
                {
                    dialog.Columns = Keys.Retrieve("PDF_Columns");
				}

                await dialog.ShowDialog(MainWindow.Instance);

                bool Changed = false;

                Changed |= Keys.Update("PDF_Logo", dialog.LogoPath);
                Changed |= Keys.Update("PDF_Title", dialog.PDFTitle);
                Changed |= Keys.Update("PDF_Subtitle", dialog.Subtitle);
                Changed |= Keys.Update("PDF_Footer", dialog.Footer);
                Changed |= Keys.Update("PDF_Backpage", dialog.Backpage);
                Changed |= Keys.Update("PDF_Comment", dialog.Comment);
                Changed |= Keys.Update("PDF_ExportPath", dialog.ExportPath);
                Changed |= Keys.Update("PDF_ArchivePath", dialog.ArchivePath);
				Changed |= Keys.Update("PDF_Columns", dialog.Columns);

				if (Changed)
                {
                    MessageDialog messageBox = new()
                    {
                        DialogTitle = Localization.InsertKey_ChangeDetected,
                        DialogText = Localization.InsertKey_DoYouWantToSave
                    };
                    await messageBox.ShowDialog(MainWindow.Instance);
                    if (messageBox.DialogResult == DialogResult.OK)
                        MainWindow.Instance.SaveKey_Click(null, null);
                }

                if (dialog.DialogResult == DialogResult.OK)
                {
                    FileInfo file = new(dialog.ExportPath);

                    List<Event> events = new();
                    List<Event> cal = ViewModel.Calendar.Events;
                    for (int i = 0; i < cal.Count; i++)
                    {
                        if (cal[i].In(ViewModel.CalendarPeriod))
                            events.Add(cal[i]);
                    }

                    string tabName = string.Empty;
                    if (!Directory.Exists(Path.GetDirectoryName(dialog.ExportPath)))
                        Directory.CreateDirectory(Path.GetDirectoryName(dialog.ExportPath));
                    switch (dialog.CurrentTab) {
                        case 0:
                            Register columns = new(':', dialog.Columns);

                            PDF.CreateTable(events, dialog.ExportPath, dialog.PDFTitle, dialog.Subtitle, dialog.Footer, dialog.LogoPath, columns);
                            tabName = Localization.PDF_Type_Table;
                            break;
                        case 1:
                            PDF.CreateBook(events, dialog.ExportPath, dialog.PDFTitle, dialog.Subtitle, dialog.Comment, dialog.Backpage, dialog.LogoPath);
                            tabName = Localization.PDF_Type_Book;
                            break;
                    }

                    if (dialog.SaveToArchive)
                    {
                        if (!Directory.Exists(dialog.ArchivePath))
                            throw new Exception(Localization.Exception_PDFArchiveNotFound);
                        if (dialog.ArchivePath != string.Empty)
                            File.Copy(dialog.ExportPath, Path.Combine(dialog.ArchivePath, ViewModel.CalendarPeriod.ToFileName() + " (" + tabName + ")" + ".pdf"), true);
                    }
                }
            }
            catch (Exception ex)
            {
                Program.Error(Localization.Exception_Saving, ex, MainWindow.Instance);
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
                    Title = Localization.AddEvent_Edit,
                    ButtonName = Localization.AddEvent_EditButton,
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

                        ViewModel.UpdateCalendarTable();
                    }
                    catch (Exception ex)
                    {
                        Program.Error(Localization.Exception_InvalidEvent, ex, MainWindow.Instance);
                    }
                }
            }
        }

		/// <summary>
		/// Marks the selected event for deletion.
		/// </summary>
		private async void EditEvent_ContextMenu_Click(object sender, RoutedEventArgs e)
		{
			EventViewModel ev = ViewModel.Selected;
			if (sender != null)
			{
				try
				{
					AddEvent dialog = new()
					{
						Title = Localization.AddEvent_Edit,
						ButtonName = Localization.AddEvent_EditButton,
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

							ViewModel.UpdateCalendarTable();
						}
						catch (Exception ex)
						{
							Program.Error(Localization.Exception_InvalidEvent, ex, MainWindow.Instance);
						}
					}
				}
				catch (Exception ex)
				{
					Program.Error(Localization.Exception_Name, ex, MainWindow.Instance);
				}
			}
		}

		/// <summary>
		/// Handles coloring of the rows.
		/// </summary>
		private void Calendar_RowLoading(object sender, DataGridRowEventArgs e)
		{
			if (e.Row.DataContext is EventViewModel ev)
			{
				if (e.Row.GetIndex() % 2 == 1)
				{
					e.Row.Background = ev.Deleted switch
					{
						true => DeleteDark,
						_ => StdDark,
					};
				}
				else
				{
					e.Row.Background = ev.Deleted switch
					{
						true => DeleteLight,
						_ => StdLight,
					};
				}
			}
		}

		public override void Load()
        {
            if (Keys.Retrieve("Calendar-Email") != string.Empty)
            {
                try
                {
                    ViewModel = new(Keys.Retrieve("Calendar-Email"), Keys.Retrieve("Calendar-Password"), Keys.Retrieve("Calendar-URL"));
                }
                catch (Exception ex)
                {
                    Program.Log(ex);
                    ViewModel = new();
                }
            }
            else
                ViewModel = new();
        }

        public override string HasBeenChanged()
        {
            return ViewModel.HasBeenChanged ? Title : string.Empty;
        }
    }
}