using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using System;
using Timotheus.IO;
using Timotheus.Views;

namespace Timotheus.ViewModels
{
    public class EventViewModel : ViewModel
    {
        /// <summary>
        /// The equivalent event on in the file.
        /// </summary>
		public CalendarEvent Saved { get; private set; }

        /// <summary>
        /// The equivalent event on the remote.
        /// </summary>
        public CalendarEvent Remote { get; private set; }

        /// <summary>
        /// Event the View Model takes data from.
        /// </summary>
        public CalendarEvent Local { get; private set; }

        private CalendarEvent Reference
        {
            get
            {
                if (Local != null) return Local;
                else if (Remote != null) return Remote;
                else return Saved;
            }
        }

        /// <summary>
        /// The start date of the event formatted.
        /// </summary>
        public string Start
        {
            get
            {
                if (IsAllDayEvent)
                    return Reference.Start.Value.ToString("d");
                else
                    return Reference.Start.Value.ToString("g");
            }
            set
            {
                try
                {
                    CalDateTime newValue = DateTime.Parse(value);
                    if (newValue > Reference.End)
                        throw new Exception();

                    Reference.Start = new CalDateTime(newValue);
                }
                catch (Exception)
                {
                    
                }
            }
        }

        /// <summary>
        /// The start date of the event for sorting.
        /// </summary>
        public DateTime StartSort
        {
            get
            {
                return Reference.Start.Value;
            }
        }

        /// <summary>
        /// The end date of the event formatted.
        /// </summary>
        public string End
        {
            get
            {
                if (IsAllDayEvent)
                    return Reference.End.Value.ToString("d");
                else
                    return Reference.End.Value.ToString("g");
            }
            set
            {
                try
                {
					CalDateTime newValue = DateTime.Parse(value);
					if (newValue < Reference.Start)
						throw new Exception();

					Reference.End = new CalDateTime(DateTime.Parse(value));
                }
                catch (Exception)
                {
					
				}
            }
        }

        /// <summary>
        /// The end date of the event for sorting.
        /// </summary>
        public DateTime EndSort
        {
            get
            {
                return Reference.End.Value;
            }
        }

        /// <summary>
        /// Name of the event.
        /// </summary>
        public string Name
        {
            get
            {
                return Reference.Summary;
            }
            set
            {
                string initialValue = Reference.Summary;
                try
                {
                    if (value.Trim() == string.Empty)
                        throw new Exception(Localization.Exception_EmptyEventName);
                    Reference.Summary = value;
                }
                catch (Exception exception)
                {
                    Reference.Summary = initialValue;
                    NotifyPropertyChanged(nameof(Name));
                    Program.Error(Localization.Exception_Name, exception, MainWindow.Instance);
                }
            }
        }

        /// <summary>
        /// The events description.
        /// </summary>
        public string Description
        {
            get
            {
                return Reference.Description;
            }
            set
            {
                Reference.Description = value;
            }
        }

        /// <summary>
        /// The location of the event.
        /// </summary>
        public string Location
        {
            get
            {
                return Reference.Location;
            }
            set
            {
                Reference.Location = value;
            }
        }

        /// <summary>
        /// Whether the event is marked for deletion or not.
        /// </summary>
        public bool Deleted
        {
            get
            {
                return Handle == SyncHandle.DeleteRemote || Handle == SyncHandle.DeleteLocal;
            }
            set
            {
                if (value)
                    Handle = SyncHandle.DeleteRemote;
                else
                    Handle = SyncHandle.Nothing;
            }
        }

        public bool ShowSyncMessage
        {
            get
            {
                return SyncMessage != string.Empty;
            }
        }

        public string SyncMessage
        {
            get
            {
                string message = string.Empty;

                switch (Handle)
                {
                    case SyncHandle.DeleteRemote:
                        message = Localization.Calendar_Sync_DeleteRemote;
                        break;
                    case SyncHandle.DeleteLocal:
                        message = Localization.Calendar_Sync_DeleteLocal;
                        break;
                    case SyncHandle.Upload:
                        message = Localization.Calendar_Sync_Upload;
                        break;
                    case SyncHandle.NewUpload:
                        message = Localization.Calendar_Sync_NewUpload;
                        break;
					case SyncHandle.Download:
						message = Localization.Calendar_Sync_Download;
						break;
					case SyncHandle.NewDownload:
						message = Localization.Calendar_Sync_NewDownload;
						break;
				}

                return message;
            }
        }

        /// <summary>
        /// Opposite of deleted.
        /// </summary>
		public bool NotDeleted
        {
            get
            {
                return !Deleted;
            }
        }

        /// <summary>
        /// Returns whether the date is an all-day event.
        /// </summary>
        private bool IsAllDayEvent
        {
            get
            {
                return Reference.Start.Hour == Reference.End.Hour && Reference.Start.Minute == Reference.End.Minute && Reference.Start.Second == Reference.End.Second && Reference.Start.Hour == 0 && Reference.Start.Minute == 0 && Reference.Start.Second == 0;
            }
        }

        /// <summary>
        /// The UID of the event.
        /// </summary>
        public string UID
        {
            get
            {
                return Reference.Uid;
            }
            set
            {
                Reference.Uid = value;
            }
        }

        private SyncHandle _handle = SyncHandle.Nothing;
        public SyncHandle Handle
        {
            get
            {
                return _handle;
            }
            private set
            {
                _handle = value;
                NotifyPropertyChanged(nameof(SyncMessage));
                NotifyPropertyChanged(nameof(ShowSyncMessage));
            }
        }

        public bool Unsaved
        {
            get
            {
                bool unsaved = false;

                if (Saved == null)
                {
                    unsaved = true;
                }
                else
                {
                    // IF SOMEHOW CHANGED
                }

                return unsaved;
            }
        }

        public bool Editable
        {
            get
            {
                return Local != null;
            }
        }

        /// <summary>
        /// Creates a view model of an event for usage in UI.
        /// </summary>
        public EventViewModel(CalendarEvent Local, CalendarEvent Remote, CalendarEvent Saved)
        {
            this.Local = Local;
            this.Remote = Remote;
            this.Saved = Saved;

            Handle = CalculateSyncHandle();
		}

        private SyncHandle CalculateSyncHandle()
        {
            SyncHandle handle = SyncHandle.Nothing;

			if (Local != null)
            {
                if (Remote != null)
                {
                    if (Remote.Uid != "NULL")
                    {
                        {
                            bool summary = Local.Summary.Trim() != Remote.Summary.Trim();

                            string localDescription = Local.Description != null ? Local.Description.Replace("\r\n", "\n").Trim() : string.Empty;
                            string remoteDescription = Remote.Description != null ? Remote.Description.Replace("\r\n", "\n").Trim() : string.Empty;
                            bool description = localDescription != remoteDescription;

                            bool start = Local.Start.Day != Remote.Start.Day || Local.Start.Month != Remote.Start.Month || Local.Start.Year != Remote.Start.Year ||
                                         Local.Start.Hour != Remote.Start.Hour || Local.Start.Minute != Remote.Start.Minute;
                            bool end = Local.End.Day != Remote.End.Day || Local.End.Month != Remote.End.Month || Local.End.Year != Remote.End.Year ||
                                       Local.End.Hour != Remote.End.Hour || Local.End.Minute != Remote.End.Minute;

                            string localLocation = Local.Location != null ? Local.Location.Trim() : string.Empty;
                            string remoteLocation = Remote.Location != null ? Remote.Location.Trim() : string.Empty;
                            bool location = localLocation != remoteLocation;

                            if (summary || description || start || end || location)
                            {
                                handle = SyncHandle.Upload;
                            }
                        }
                    }

					if (handle != SyncHandle.Upload)
					{
						if (Saved != null)
						{
							bool summary = Local.Summary.Trim() != Saved.Summary.Trim();

							string localDescription = Local.Description != null ? Local.Description.Replace("\r\n", "\n").Trim() : string.Empty;
							string savedDescription = Saved.Description != null ? Saved.Description.Replace("\r\n", "\n").Trim() : string.Empty;
							bool description = localDescription != savedDescription;

							bool start = Local.Start.Day != Saved.Start.Day || Local.Start.Month != Saved.Start.Month || Local.Start.Year != Saved.Start.Year ||
										 Local.Start.Hour != Saved.Start.Hour || Local.Start.Minute != Saved.Start.Minute;
							bool end = Local.End.Day != Saved.End.Day || Local.End.Month != Saved.End.Month || Local.End.Year != Saved.End.Year ||
									   Local.End.Hour != Saved.End.Hour || Local.End.Minute != Saved.End.Minute;

							string localLocation = Local.Location != null ? Local.Location.Trim() : string.Empty;
							string savedLocation = Saved.Location != null ? Saved.Location.Trim() : string.Empty;
							bool location = localLocation != savedLocation;

							if (summary || description || start || end || location)
							{
								handle = SyncHandle.Download;
							}
						}
						else
						{
							handle = SyncHandle.NewDownload;
						}
					}
				}
                else
                {
					handle = SyncHandle.NewUpload;
				}
			}
            else
            {
                if (Remote != null)
                {
					handle = SyncHandle.DeleteRemote;
				}
                else
                {
					handle = SyncHandle.DeleteLocal;
				}
            }

            return handle;
		}
    }
}