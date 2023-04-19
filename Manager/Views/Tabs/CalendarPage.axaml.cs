using Avalonia.Input;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using System;
using System.IO;
using System.Collections.Generic;
using Timotheus.IO;
using Timotheus.Schedule;
using Timotheus.Utility;
using Timotheus.ViewModels;
using Timotheus.Views.Dialogs;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;

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

        /// <summary>
        /// Constructor for Calendar page
        /// </summary>
        public CalendarPage() : base()
        {
            AvaloniaXamlLoader.Load(this);
            Title = Localization.Calendar_Page;
            LoadingTitle = Localization.InsertKey_LoadCalendar;
            Icon = "avares://Timotheus/Resources/Calendar.png";
            ViewModel = new CalendarViewModel();
        }

		/// <summary>
		/// Loads the calendar using the values of the current key.
		/// </summary>
		public override void Load()
		{
			string path = Project.Keys.Retrieve("Calendar-Path").Replace('\\', '/');
			if (path != string.Empty)
			{
				try
				{
					ViewModel = new(Project.DirectoryPath + path, Project.Keys.Retrieve("Calendar-URL"), Project.Keys.Retrieve("Calendar-Email"), Project.Keys.Retrieve("Calendar-Password"));
				}
				catch (DirectoryNotFoundException)
				{
					Program.Error(Localization.Exception_Name, new Exception(Localization.Exception_CantFindCalendar.Replace("#1", path)), MainWindow.Instance);
					ViewModel = new();
				}
				catch (FileNotFoundException)
				{
					Program.Error(Localization.Exception_Name, new Exception(Localization.Exception_CantFindCalendar.Replace("#1", path)), MainWindow.Instance);
					ViewModel = new();
				}
				catch (Exception exception)
				{
					Program.Error(Localization.Exception_Name, exception, MainWindow.Instance);
					ViewModel = new();
				}
			}
			else
				ViewModel = new();
		}

		/// <summary>
		/// Changes the selected year and calls UpdateTable.
		/// </summary>
		private void Period_Click(object sender, RoutedEventArgs e)
        {
			try
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
			catch (Exception exception)
			{
				Program.Error(Localization.Exception_Saving, exception, MainWindow.Instance);
			}
        }

        /// <summary>
        /// Updates the period according to the textbox.
        /// </summary>
        private void Period_KeyDown(object sender, KeyEventArgs e)
        {
			try
			{
				if (e.Key == Avalonia.Input.Key.Enter)
				{
					ViewModel.UpdatePeriod(((TextBox)sender).Text);
					ViewModel.UpdateCalendarTable();
				}
			}
			catch (Exception exception)
			{
				Program.Error(Localization.Exception_Saving, exception, MainWindow.Instance);
			}
        }

        /// <summary>
        /// Opens a SyncCalendar dialog to sync the current calendar.
        /// </summary>
        private async void SyncCalendar_Click(object sender, RoutedEventArgs e)
        {
			try
			{
				SyncCalendar dialog = new()
				{
					Period = ViewModel.PeriodText
				};

				await dialog.ShowDialog(MainWindow.Instance);

				if (dialog.DialogResult == DialogResult.OK)
				{
					ProgressDialog pDialog = new()
					{
						Title = Localization.SyncCalendar_Worker
					};

					Schedule.Period syncPeriod;

					if (dialog.SyncAll)
						syncPeriod = new Schedule.Period(DateTime.MinValue, DateTime.MaxValue);
					else if (dialog.SyncPeriod)
						syncPeriod = new Schedule.Period(ViewModel.PeriodText);
					else
						syncPeriod = new Schedule.Period(dialog.Start, dialog.End.AddDays(1));

					await pDialog.ShowDialog(MainWindow.Instance, ViewModel.Sync, syncPeriod);

					ViewModel.UpdateCalendarTable();
				}
			}
			catch (Exception exception)
			{
				Program.Error(Localization.Exception_Saving, exception, MainWindow.Instance);
			}
        }

        /// <summary>
        /// Opens a SaveFileDialog to save the current Calendar.
        /// </summary>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
				if (Project.FilePath == string.Empty)
					throw new Exception(Localization.Exception_MustSaveKeyFirst);

				string path = Project.DirectoryPath + Project.Keys.Retrieve("Calendar-Path");

				if (path == string.Empty)
                {
                    SaveAs_Click(null, null);
                }
                else
                {
					ViewModel.Save(path);
				}
            }
            catch (Exception exception)
            {
				Program.Error(Localization.Exception_Saving, exception, MainWindow.Instance);
			}
        }

		/// <summary>
		/// Opens a SaveFileDialog to save the current Calendar.
		/// </summary>
		private async void SaveAs_Click(object sender, RoutedEventArgs e)
		{
            try
            {
				if (Project.FilePath == string.Empty)
					throw new Exception(Localization.Exception_MustSaveKeyFirst);

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
					string path = Project.GetProjectPath(result);
					Project.Keys.Update("Calendar-Path", path.Replace('\\', '/'));

					ViewModel.Save(result);

					MessageDialog confirmation = new()
					{
						DialogTitle = Localization.Confirmation,
						DialogText = Localization.Confirmation_SaveCalendar,
						DialogShowCancel = false
					};
					await confirmation.ShowDialog(MainWindow.Instance);
				}
			}
            catch (Exception exception)
            {
				Program.Error(Localization.Exception_Saving, exception, MainWindow.Instance);
			}
		}

		/// <summary>
		/// Opens a OpenCalendar dialog
		/// </summary>
		private async void Open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
				bool open = true;

				if (Project.FilePath == string.Empty)
					throw new Exception(Localization.Exception_MustSaveKeyFirst);

				if (ViewModel.Events.Count > 0)
				{
					WarningDialog warning = new()
					{
						DialogTitle = Localization.Exception_Warning,
						DialogText = Localization.Calendar_OpenWarning,
						DialogShowCancel = true
					};
					await warning.ShowDialog(MainWindow.Instance);
					open = warning.DialogResult == DialogResult.OK;
				}

				if (open)
				{
					OpenFileDialog openFileDialog = new();

					FileDialogFilter txtFilter = new();
					txtFilter.Extensions.Add("ics");
					txtFilter.Name = "iCal (.ics)";

					openFileDialog.Filters = new()
				    {
					    txtFilter
				    };

					string[] result = await openFileDialog.ShowAsync(MainWindow.Instance);
                    if (result != null && result.Length > 0)
                    {
                        string path = Project.GetProjectPath(result[0]);
						Project.Keys.Update("Calendar-Path", path.Replace('\\', '/'));
                        Load();
					}
				}
			}
			catch (Exception exception)
			{
				Program.Error(Localization.Exception_Opening, exception, MainWindow.Instance);
			}
        }

        /// <summary>
        /// Opens a AddEvent dialog, and adds the result to the current calendar.
        /// </summary>
        private async void AddEvent_Click(object sender, RoutedEventArgs e)
        {
			try
			{
				AddEvent dialog = new();

				string text;
				if ((text = Project.Keys.Retrieve("Settings-Address")) != string.Empty)
					dialog.Location = text;
				if ((text = Project.Keys.Retrieve("Settings-EventDescription")) != string.Empty)
					dialog.Description = text;
				if ((text = Project.Keys.Retrieve("Settings-EventStart")) != string.Empty)
					dialog.StartTime = text;
				if ((text = Project.Keys.Retrieve("Settings-EventEnd")) != string.Empty)
					dialog.EndTime = text;
				if ((text = Project.Keys.Retrieve("Settings-EventLocation")) != string.Empty)
					dialog.Location = text;

				Schedule.Period period = new(ViewModel.PeriodText);
				if (!period.In(DateTime.Now))
				{
					dialog.Start = period.Start.Date;
					dialog.End = period.Start.Date;
				}

				await dialog.ShowDialog(MainWindow.Instance);

				if (dialog.DialogResult == DialogResult.OK)
				{
					ViewModel.AddEvent(dialog.Start, dialog.End, dialog.EventName, dialog.Description, dialog.Location);
					ViewModel.UpdateCalendarTable();
				}
			}
			catch (Exception exception)
			{
				Program.Error(Localization.Exception_Saving, exception, MainWindow.Instance);
			}
		}

        /// <summary>
        /// Marks the selected event for deletion.
        /// </summary>
        private async void RemoveEvent_Click(object sender, RoutedEventArgs e)
        {
			try
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
			catch (Exception exception)
			{
				Program.Error(Localization.Exception_Name, exception, MainWindow.Instance);
			}
        }

		/// <summary>
		/// Marks the selected event for deletion.
		/// </summary>
		private void UndoRemoveEvent_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				EventViewModel ev = (EventViewModel)((Button)e.Source).DataContext;
				if (ev != null)
				{
					ViewModel.RestoreEvent(ev);
				}
			}
			catch (Exception exception)
			{
				Program.Error(Localization.Exception_Name, exception, MainWindow.Instance);
			}
		}

		/// <summary>
		/// Marks the selected event for deletion.
		/// </summary>
		private async void RemoveEvent_ContextMenu_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				EventViewModel ev = ViewModel.Selected;
				if (sender != null)
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
			catch (Exception exception)
			{
				Program.Error(Localization.Exception_Saving, exception, MainWindow.Instance);
			}
		}
		/// <summary>
		/// Opens a SaveFileDialog to export the current Calendar (in the given period) as a PDF.
		/// </summary>
		private async void SetupCalendar_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (Project.FilePath == string.Empty)
					throw new Exception(Localization.Exception_MustSaveKeyFirst);

				SetupCalendar dialog = new()
				{
					Username = Project.Keys.Retrieve("Calendar-Email"),
					Password = Project.Keys.Retrieve("Calendar-Password"),
					URL = Project.Keys.Retrieve("Calendar-URL"),
					Description = Project.Keys.Retrieve("Settings-EventDescription"),
					StartTime = Project.Keys.Retrieve("Settings-EventStart"),
					EndTime = Project.Keys.Retrieve("Settings-EventEnd"),
					Location = Project.Keys.Retrieve("Settings-EventLocation")
				};

				await dialog.ShowDialog(MainWindow.Instance);

				if (dialog.DialogResult == DialogResult.OK)
				{
					bool changed = false;

					if (dialog.Description != string.Empty)
						changed |= Project.Keys.Update("Settings-EventDescription", dialog.Description);
					if (dialog.StartTime != string.Empty)
						changed |= Project.Keys.Update("Settings-EventStart", dialog.StartTime);
					if (dialog.EndTime != string.Empty)
						changed |= Project.Keys.Update("Settings-EventEnd", dialog.EndTime);
					if (dialog.Location != string.Empty)
						changed |= Project.Keys.Update("Settings-EventLocation", dialog.Location);
					if (dialog.Username != string.Empty)
						changed |= Project.Keys.Update("Calendar-Email", dialog.Username);
					if (dialog.Password != string.Empty)
						changed |= Project.Keys.Update("Calendar-Password", dialog.Password);
					if (dialog.URL != string.Empty)
						changed |= Project.Keys.Update("Calendar-URL", dialog.URL);

					if (changed)
					{
						ViewModel.SetupSync(dialog.URL, dialog.Username, dialog.Password);

						MessageDialog messageBox = new()
						{
							DialogTitle = Localization.InsertKey_ChangeDetected,
							DialogText = Localization.InsertKey_DoYouWantToSave
						};
						await messageBox.ShowDialog(MainWindow.Instance);
						if (messageBox.DialogResult == DialogResult.OK)
						{
							MainWindow.Instance.SaveKey_Click(null, null);
						}
					}
				}
			}
			catch (Exception exception)
			{
				Program.Error(Localization.Exception_Name, exception, MainWindow.Instance);
			}
		}

		/// <summary>
		/// Opens a SaveFileDialog to export the current Calendar (in the given period) as a PDF.
		/// </summary>
		private async void ExportPDF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
				if (Project.FilePath == string.Empty)
					throw new Exception(Localization.Exception_MustSaveKeyFirst);

				PDFDialog dialog = new()
                {
                    LogoPath = Project.DirectoryPath + Project.Keys.Retrieve("PDF_Logo"),
                    PDFTitle = Project.Keys.Retrieve("PDF_Title"),
                    Subtitle = Project.Keys.Retrieve("PDF_Subtitle"),
                    Footer = Project.Keys.Retrieve("PDF_Footer"),
                    Backpage = Project.Keys.Retrieve("PDF_Backpage"),
                    Comment = Project.Keys.Retrieve("PDF_Comment"),
                    ArchivePath = Project.DirectoryPath + Project.Keys.Retrieve("PDF_ArchivePath"),
					DirectoryPath = Project.DirectoryPath
				};

                if (Project.Keys.Retrieve("PDF_Columns") != string.Empty)
                {
                    dialog.Columns = Project.Keys.Retrieve("PDF_Columns");
				}

                await dialog.ShowDialog(MainWindow.Instance);

                
                if (dialog.DialogResult == DialogResult.OK)
                {
					bool Changed = false;

					Changed |= Project.Keys.Update("PDF_Logo", Project.GetProjectPath(dialog.LogoPath));
					Changed |= Project.Keys.Update("PDF_Title", dialog.PDFTitle);
					Changed |= Project.Keys.Update("PDF_Subtitle", dialog.Subtitle);
					Changed |= Project.Keys.Update("PDF_Footer", dialog.Footer);
					Changed |= Project.Keys.Update("PDF_Backpage", dialog.Backpage);
					Changed |= Project.Keys.Update("PDF_Comment", dialog.Comment);
					Changed |= Project.Keys.Update("PDF_ArchivePath", Project.GetProjectPath(dialog.ArchivePath));
					Changed |= Project.Keys.Update("PDF_Columns", dialog.Columns);

					List<CalendarEvent> events = new();
                    foreach (CalendarEvent ev in ViewModel.Calendar.Events)
                    {
						if (ev.In(ViewModel.CalendarPeriod))
							events.Add(ev);
					}

                    string tabName = string.Empty;

					SaveFileDialog saveFileDialog = new()
					{
						InitialFileName = Path.GetFileName(Project.Keys.Retrieve("PDF-FileName")),
						Directory = Path.GetDirectoryName(Project.DirectoryPath + Project.Keys.Retrieve("PDF-FileName"))
					};

					FileDialogFilter filter = new();
					filter.Extensions.Add("pdf");
					filter.Name = "PDF Files (.pdf)";

					saveFileDialog.Filters = new()
			        {
				        filter
			        };

					string result = await saveFileDialog.ShowAsync(MainWindow.Instance);

                    if (result != string.Empty)
                    {
                        Project.Keys.Update("PDF-FileName", Project.GetProjectPath(result));
						if (!Directory.Exists(Path.GetDirectoryName(result)))
							Directory.CreateDirectory(Path.GetDirectoryName(result));
						switch (dialog.CurrentTab)
						{
							case 0:
								Register columns = new(':', dialog.Columns);

								PDF.CreateTable(events, result, dialog.PDFTitle, dialog.Subtitle, dialog.Footer, dialog.LogoPath, columns);
								tabName = Localization.PDF_Type_Table;
								break;
							case 1:
								PDF.CreateBook(events, result, dialog.PDFTitle, dialog.Subtitle, dialog.Comment, dialog.Backpage, dialog.LogoPath);
								tabName = Localization.PDF_Type_Book;
								break;
						}

						if (dialog.SaveToArchive)
						{
							if (!Directory.Exists(dialog.ArchivePath))
								throw new Exception(Localization.Exception_PDFArchiveNotFound);
							if (dialog.ArchivePath != string.Empty)
								File.Copy(result, Path.Combine(dialog.ArchivePath, ViewModel.CalendarPeriod.ToFileName() + " (" + tabName + ")" + ".pdf"), true);
						}
					}

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
				}				
			}
            catch (Exception exception)
            {
				Program.Error(Localization.Exception_Name, exception, MainWindow.Instance);
			}
        }

        /// <summary>
        /// Marks the selected event for deletion.
        /// </summary>
        private async void EditEvent_Click(object sender, RoutedEventArgs e)
        {
			try
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
						Location = ev.Location,
						Description = ev.Description
					};

					await dialog.ShowDialog(MainWindow.Instance);

					if (dialog.DialogResult == DialogResult.OK)
					{
						ev.Name = dialog.EventName;
						ev.Start = dialog.Start.ToString();
						ev.End = dialog.End.ToString();
						ev.Location = dialog.Location;
						ev.Description = dialog.Description;

						ViewModel.UpdateCalendarTable();
					}
				}
			}
			catch (Exception exception)
			{
				Program.Error(Localization.Exception_Saving, exception, MainWindow.Instance);
			}
        }

		private void StartTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			EventViewModel ev = (EventViewModel)((TextBox)e.Source).DataContext;
			try
			{
				string value = ((TextBox)e.Source).Text;
				CalDateTime newValue = DateTime.Parse(value);

				if (ev != null)
				{
					if (newValue.Value > ev.EndSort)
						throw new Exception(Localization.Exception_StartLaterThanEnd);

					DateTime fixedDate = ev.StartSort;
					if (newValue.Year < 2000)
					{
						fixedDate = new DateTime(ev.EndSort.Year, ev.StartSort.Month, ev.StartSort.Day, ev.StartSort.Hour, ev.StartSort.Minute, 0);
						if (fixedDate >= ev.EndSort)
							fixedDate = ev.EndSort.AddMinutes(-90);
					}

					((TextBox)e.Source).Text = fixedDate.ToString("g");
				}
			}
			catch (FormatException)
			{
				if (ev != null)
				{
					((TextBox)e.Source).Text = ev.StartSort.ToString("g");
				}
				Program.Error(Localization.Exception_Name, new Exception(Localization.Exception_InvalidDate), MainWindow.Instance);
			}
			catch (Exception exception)
			{
				if (ev != null)
				{
					((TextBox)e.Source).Text = ev.StartSort.ToString("g");
				}
				Program.Error(Localization.Exception_Name, exception, MainWindow.Instance);
			}
		}

		private void EndTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			EventViewModel ev = (EventViewModel)((TextBox)e.Source).DataContext;
			try
			{
				string value = ((TextBox)e.Source).Text;
				CalDateTime newValue = DateTime.Parse(value);

				if (ev != null)
				{
					if (newValue.Value < ev.StartSort)
						throw new Exception(Localization.Exception_StartLaterThanEnd);

					DateTime fixedDate = ev.EndSort;
					if (newValue.Year < 2000)
					{
						fixedDate = new DateTime(ev.StartSort.Year, ev.EndSort.Month, ev.EndSort.Day, ev.EndSort.Hour, ev.EndSort.Minute, 0);
						if (fixedDate <= ev.StartSort)
							fixedDate = ev.StartSort.AddMinutes(90);
					}

					((TextBox)e.Source).Text = fixedDate.ToString("g");
				}
			}
			catch (FormatException)
			{
				if (ev != null)
				{
					((TextBox)e.Source).Text = ev.EndSort.ToString("g");
				}
				Program.Error(Localization.Exception_Name, new Exception(Localization.Exception_InvalidDate), MainWindow.Instance);
			}
			catch (Exception exception)
			{
				if (ev != null)
				{
					((TextBox)e.Source).Text = ev.EndSort.ToString("g");
				}
				Program.Error(Localization.Exception_Name, exception, MainWindow.Instance);
			}
		}

		/// <summary>
		/// Ensures that the textbox only contains numbers or periods
		/// </summary>
		private void OnlyDate(object sender, KeyEventArgs e)
		{
			bool normalNumber = e.Key >= Avalonia.Input.Key.D0 && e.Key <= Avalonia.Input.Key.D9;
			bool numPadNumber = e.Key >= Avalonia.Input.Key.NumPad0 && e.Key <= Avalonia.Input.Key.NumPad9;
			bool separator = e.Key == Avalonia.Input.Key.OemPeriod;

			e.Handled = !normalNumber && !numPadNumber && !separator;
		}

		/// <summary>
		/// Marks the selected event for deletion.
		/// </summary>
		private async void EditEvent_ContextMenu_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				EventViewModel ev = ViewModel.Selected;
				if (sender != null)
				{
					AddEvent dialog = new()
					{
						Title = Localization.AddEvent_Edit,
						ButtonName = Localization.AddEvent_EditButton,
						EventName = ev.Name,
						Start = ev.StartSort,
						End = ev.EndSort,
						Location = ev.Location,
						Description = ev.Description
					};

					await dialog.ShowDialog(MainWindow.Instance);

					if (dialog.DialogResult == DialogResult.OK)
					{
						ev.Name = dialog.EventName;
						ev.Start = dialog.Start.ToString();
						ev.End = dialog.End.ToString();
						ev.Location = dialog.Location;
						ev.Description = dialog.Description;

						ViewModel.UpdateCalendarTable();
					}
				}
			}
			catch (Exception ex)
			{
				Program.Error(Localization.Exception_Name, ex, MainWindow.Instance);
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
					e.Row.Background = ev.Handle switch
					{
						SyncHandle.NewUpload => NewDark,
						SyncHandle.Upload => UpdateDark,
						SyncHandle.DeleteRemote => DeleteDark,
						SyncHandle.DeleteLocal or SyncHandle.Download or SyncHandle.NewDownload => BlueDark,
						_ => StdDark,
					};
				}
				else
				{
					e.Row.Background = ev.Handle switch
					{
						SyncHandle.NewUpload => NewLight,
						SyncHandle.Upload => UpdateLight,
						SyncHandle.DeleteRemote => DeleteLight,
						SyncHandle.DeleteLocal or SyncHandle.Download or SyncHandle.NewDownload => BlueLight,
						_ => StdLight,
					};
				}
			}
		}

		/// <summary>
		/// Returns a message, if the calendar has been changed.
		/// </summary>
		public override string HasBeenChanged()
        {
            return ViewModel.HasBeenChanged() ? Title : string.Empty;
        }
    }
}