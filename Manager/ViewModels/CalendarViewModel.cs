using System;
using System.IO;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Timotheus.Schedule;
using Period = Timotheus.Schedule.Period;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Timotheus.Views;
using Avalonia.Threading;

namespace Timotheus.ViewModels
{
    public class CalendarViewModel : TabViewModel
    {
		/// <summary>
		/// The currently selected event. Used for the context menu.
		/// </summary>
		public EventViewModel Selected { get; set; }

        private Period _calendarPeriod;
		/// <summary>
		/// Type of period used by Calendar_View.
		/// </summary>
		public Period CalendarPeriod
        {
            get
            {
                return _calendarPeriod;
            }
            private set
            {
                _calendarPeriod = value;
            }
        }

        /// <summary>
        /// The index of the current period type.
        /// </summary>
        public int SelectedPeriod
        {
            get { return (int)CalendarPeriod.Type; }
            set
            {
                CalendarPeriod.SetType((PeriodType)value);
                UpdateCalendarTable();
            }
        }

        private string _PeriodText = string.Empty;
        /// <summary>
        /// The text showing the current period.
        /// </summary>
        public string PeriodText
        {
            get => _PeriodText;
            set
            {
                _PeriodText = value;
                NotifyPropertyChanged(nameof(PeriodText));
            }
        }

        private ObservableCollection<EventViewModel> _Events = new();
        /// <summary>
        /// A list of the events in the current period.
        /// </summary>
        public ObservableCollection<EventViewModel> Events
        {
            get => _Events;
            set
            {
                _Events = value;
                NotifyPropertyChanged(nameof(Events));
            }
        }

		private string url = string.Empty;
		private string username = string.Empty;
		private string password = string.Empty;

		private Calendar _calendar = new();
        /// <summary>
        /// Current calendar used by the program.
        /// </summary>
        public Calendar Calendar
        {
            get
            {
                return _calendar;
            }
            set
            {
                _calendar = value;
            }
        }

        /// <summary>
        /// Tooltip shown when hovering over the Save button.
        /// </summary>
        public string Save_ToolTip
        {
            get
            {
                if (Path == string.Empty)
                    return Localization.Calendar_Save_ToolTip.Replace("#1", Localization.Unsaved);
                else
					return Localization.Calendar_Save_ToolTip.Replace("#1", Path);
			}
        }

		private bool _connected = false;
		public bool Connected
		{
			get
			{
				return _connected;
			}
			set
			{
				_connected = value;
				NotifyPropertyChanged(nameof(Connected));
			}
		}

        /// <summary>
        /// Worker used for synchronizing the calendar in the ProgressDialog.
        /// </summary>
        public BackgroundWorker Sync { get; private set; }

        public CalendarViewModel(string path, string url = "", string username = "", string password = "") : this(url, username, password)
        {
            Path = path;
            string text = File.ReadAllText(path);
            Calendar = Calendar.Load(text);

			bool found = false;
			for (int i = 0; i < Calendar.TimeZones.Count && !found; i++)
			{
				if (Calendar.TimeZones[i].TzId == "Europe/Copenhagen")
					found = true;
			}
			if (!found)
				Calendar.AddTimeZone(new VTimeZone("Europe/Copenhagen"));
		}
        public CalendarViewModel(string url = "", string username = "", string password = "")
        {
			this.url = url;
			this.username = username;
			this.password = password;

            CalendarPeriod = new(DateTime.Now.Year + " " + (DateTime.Now.Month >= 7 ? Localization.Calendar_Fall : Localization.Calendar_Spring));
            PeriodText = CalendarPeriod.ToString();
			Calendar.AddTimeZone(new VTimeZone("Europe/Copenhagen"));

			Sync = new();
			Sync.DoWork += Synchronize;
			Sync.RunWorkerCompleted += SyncComplete;
		}

		/// <summary>
		/// Saves the calendar to the given path. Deletes all events marked for deletion.
		/// </summary>
		public void Save(string path)
		{
			List<CalendarEvent> events = new();
			foreach (CalendarEvent ev in Calendar.Events)
			{
				events.Add(ev);
			}

			CalendarSerializer iCalSerializer = new();
			string result = iCalSerializer.SerializeToString(Calendar);

			Path = path;
			NotifyPropertyChanged(nameof(Save_ToolTip));
			File.WriteAllText(path, result);
			UpdateCalendarTable();
		}

		/// <summary>
		/// Adds the event to the calendar.
		/// </summary>
		public void AddEvent(DateTime Start, DateTime End, string Name, string Description, string Location)
        {
			CalendarEvent ev = new()
			{
				Start = new CalDateTime(Start),
                End = new CalDateTime(End),
                Summary = Name,
                Description = Description,
                Location = Location
			};

			Calendar.Events.Add(ev);
        }

		/// <summary>
		/// Marks the event for deletion.
		/// </summary>
        public void RemoveEvent(EventViewModel evm)
        {
            bool found = false;

            for (int i = 0; i < Calendar.Events.Count && !found; i++)
            {
                if (Calendar.Events[i].Uid.ToUpper() == evm.UID.ToUpper())
				{
					found = true;
					Calendar.Events.Remove(Calendar.Events[i]);
				}
            }

            UpdateCalendarTable();
        }

		/// <summary>
		/// Restores the event with a given UID. aka. removes it from the deleted list.
		/// </summary>
        public void RestoreEvent(EventViewModel evm)
        {
			if (evm.Saved != null)
				Calendar.Events.Add(evm.Saved);
			else if (evm.Remote != null)
				Calendar.Events.Add(evm.Remote);

			UpdateCalendarTable();
		}

		/// <summary>
		/// Sets up the calender to synchronizing.
		/// </summary>
		public void SetupSync(string url, string username, string password)
		{
			this.url = url;
			this.username = username;
			this.password = password;

			UpdateCalendarTable();
		}

		/// <summary>
		/// Adds the event to the remote calendar.
		/// </summary>
		private void AddToRemote(CalendarEvent calendarEvent)
		{
			CalendarSerializer calendarSerializer = new();

			Calendar calendar = new();
			calendar.AddTimeZone(new VTimeZone("Europe/Copenhagen"));
			calendar.AddChild(calendarEvent);

			string data = calendarSerializer.SerializeToString(calendar);
			HttpRequest(url + calendarEvent.Uid + ".ics", HttpMethod.Put, Encoding.UTF8.GetBytes(data));
		}

		/// <summary>
		/// Removes the event from the remote calendar.
		/// </summary>
		private void RemoveFromRemote(CalendarEvent calendarEvent)
		{
			HttpRequest(url + calendarEvent.Uid + ".ics", HttpMethod.Delete);
		}

		/// <summary>
		/// Syncs the events in the time interval from a to b with the remote calendar. (As long as either the start time or end time is in the interval)
		/// </summary>
		private void Synchronize(object sender, DoWorkEventArgs e)
		{
			try
			{
				List<EventViewModel> events = GetEvents();

				for (int i = 0; i < events.Count; i++)
				{
					EventViewModel ev = events[i];
					if (Sync.CancellationPending == true)
					{
						break;
					}
					else
					{
						Sync.ReportProgress((i * 100) / events.Count, events[i].Name);

						switch (ev.Handle)
						{
							case IO.SyncHandle.Upload: // Overwrite remote with local
								RemoveFromRemote(ev.Remote);
								AddToRemote(ev.Local);
								break;

							case IO.SyncHandle.NewUpload: // Add new event on the remote from local
								AddToRemote(ev.Local);
								break;

							case IO.SyncHandle.DeleteRemote: // Delete it remotely
								RemoveFromRemote(ev.Remote);
								break;
						}
					}
				}

				UpdateCalendarTable();
			}
			catch (Exception exception)
			{
				e.Result = exception;
			}
		}

		/// <summary>
		/// Event after the synchronization is complete.
		/// </summary>
		private void SyncComplete(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Result is Exception ex)
			{
				Dispatcher.UIThread.InvokeAsync(delegate
				{
					Program.Error(Localization.Exception_Name, ex, MainWindow.Instance);
				});
			}
		}

		/// <summary>
		/// Sends a HTTP request to a URL and returns the response as a string array.
		/// </summary>
		private string HttpRequest(string url, HttpMethod httpMethod, byte[] data = null)
		{
			HttpClient client = new();
			NetworkCredential credentials = new(username, password);

			using HttpRequestMessage message = new(httpMethod, url);
			message.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{credentials.UserName}:{credentials.Password}")));

			if (httpMethod == HttpMethod.Put || httpMethod == HttpMethod.Post)
			{
				message.Content = new ByteArrayContent(data);
				message.Content.Headers.ContentType = new MediaTypeHeaderValue("text/calendar");
				message.Content.Headers.ContentLength = data.Length;
			}

			HttpResponseMessage response = client.SendAsync(message).Result;
			response.EnsureSuccessStatusCode();
			return response.Content.ReadAsStringAsync().Result;
		}

		/// <summary>
		/// Changes the selected year and calls UpdateTable.
		/// </summary>
		public void UpdatePeriod(bool add)
        {
            if (add)
                CalendarPeriod.Add();
            else
                CalendarPeriod.Subtract();
            UpdateCalendarTable();
        }

        /// <summary>
        /// Changes the selected year and calls UpdateTable.
        /// </summary>
        public void UpdatePeriod(string text)
        {
            CalendarPeriod.SetPeriod(text);
            NotifyPropertyChanged(nameof(SelectedPeriod));
        }

		/// <summary>
		/// Returns a list of EventViewModels in the period
		/// </summary>
		public List<EventViewModel> GetEvents()
		{
			List<EventViewModel> events = new();

			List<CalendarEvent> remoteEvents = new();
			List<CalendarEvent> savedEvents = new();

			if (url != string.Empty && username != string.Empty && password != string.Empty)
			{
				try
				{
					string data = HttpRequest(url, HttpMethod.Get);
					Calendar Calendar = Calendar.Load(data);

					foreach (CalendarEvent remote in Calendar.Events)
					{
						if (remote.In(CalendarPeriod))
						{
							remoteEvents.Add(remote);
						}
					}

					Connected = true;
				}
				catch (Exception)
				{
					Connected = false;
					remoteEvents = new List<CalendarEvent>();
				}
			}
			else
				Connected = false;
			bool[] foundRemote = new bool[remoteEvents.Count];

			if (Path != string.Empty)
			{
				try
				{
					string data = File.ReadAllText(Path);
					Calendar Calendar = Calendar.Load(data);

					foreach (CalendarEvent saved in Calendar.Events)
					{
						if (saved.In(CalendarPeriod))
						{
							savedEvents.Add(saved);
						}
					}
				}
				catch (Exception)
				{
					savedEvents = new List<CalendarEvent>();
				}
			}
			bool[] foundSaved = new bool[savedEvents.Count];

			foreach (CalendarEvent local in Calendar.Events)
			{
				if (local.In(CalendarPeriod))
				{
					string localUID = local.Uid.ToUpper();

					CalendarEvent remote = Connected ? null : new CalendarEvent() { Uid = "NULL" };
					for (int i = 0; i < remoteEvents.Count && remote == null; i++)
					{
						if (remoteEvents[i].Uid.ToUpper() == localUID)
						{
							remote = remoteEvents[i];
							foundRemote[i] = true;
						}
					}

					CalendarEvent saved = null;
					for (int i = 0; i < savedEvents.Count && saved == null; i++)
					{
						if (savedEvents[i].Uid.ToUpper() == localUID)
						{
							saved = savedEvents[i];
							foundSaved[i] = true;
						}
					}

					EventViewModel evm = new(local, remote, saved);

					events.Add(evm);
				}
			}

			for (int i = 0; i < remoteEvents.Count; i++)
			{
				if (!foundRemote[i])
				{
					string remoteEventUID = remoteEvents[i].Uid.ToUpper();
					CalendarEvent saved = null;
					for (int j = 0; j < savedEvents.Count && saved == null; j++)
					{
						if (savedEvents[j].Uid.ToUpper() == remoteEventUID)
						{
							saved = savedEvents[j];
							foundSaved[j] = true;
						}
					}

					EventViewModel evm = new(null, remoteEvents[i], saved);

					events.Add(evm);
				}
			}

			for (int i = 0; i < savedEvents.Count; i++)
			{
				if (!foundSaved[i])
				{
					EventViewModel evm = new(null, null, savedEvents[i]);

					events.Add(evm);
				}
			}

			return events;
		}

        /// <summary>
        /// Updates the contents of the event table.
        /// </summary>
        public void UpdateCalendarTable()
        {
			Events = new ObservableCollection<EventViewModel>(GetEvents().OrderBy(i => i.StartSort));
            PeriodText = CalendarPeriod.ToString();
        }

		/// <summary>
		/// Whether the calendar has been changed.
		/// </summary>
		public bool HasBeenChanged()
        {
            bool changed = false;

            if (Path != string.Empty && File.Exists(Path))
            {
                string dataFromFile = File.ReadAllText(Path);

				CalendarSerializer iCalSerializer = new();
				string dataFromMemory = iCalSerializer.SerializeToString(Calendar);

                changed = dataFromFile != dataFromMemory;
            }
            else if (Events.Count > 0)
            {
                changed = true;
            }

            return changed;
        }
    }
}