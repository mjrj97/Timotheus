using System;
using Timotheus.Schedule;

namespace Timotheus.ViewModels
{
    public class EventViewModel : ViewModel
    {
        /// <summary>
        /// Event the View Model takes data from.
        /// </summary>
        private readonly Event ev;

        /// <summary>
        /// The start date of the event formatted.
        /// </summary>
        public string Start
        {
            get
            {
                if (ev.IsAllDayEvent)
                    return ev.Start.ToString("d");
                else
                    return ev.Start.ToString("g");
            }
            set
            {
                ev.Start = DateTime.Parse(value);
            }
        }

        /// <summary>
        /// The start date of the event for sorting.
        /// </summary>
        public DateTime StartSort
        {
            get
            {
                return ev.Start;
            }
        }

        /// <summary>
        /// The end date of the event formatted.
        /// </summary>
        public string End
        {
            get
            {
                if (ev.IsAllDayEvent)
                    return ev.End.ToString("d");
                else
                    return ev.End.ToString("g");
            }
            set
            {
                ev.End = DateTime.Parse(value);
            }
        }

        /// <summary>
        /// The end date of the event for sorting.
        /// </summary>
        public DateTime EndSort
        {
            get
            {
                return ev.End;
            }
        }

        /// <summary>
        /// Name of the event.
        /// </summary>
        public string Name
        {
            get
            {
                return ev.Name;
            }
            set
            {
                ev.Name = value;
            }
        }

        /// <summary>
        /// The events description.
        /// </summary>
        public string Description
        {
            get
            {
                return ev.Description;
            }
            set
            {
                ev.Description = value;
            }
        }

        /// <summary>
        /// The location of the event.
        /// </summary>
        public string Location
        {
            get
            {
                return ev.Location;
            }
            set
            {
                ev.Location = value;
            }
        }

        /// <summary>
        /// Whether the event is marked for deletion or not.
        /// </summary>
        public bool Deleted
        {
            get
            {
                return ev.Deleted;
            }
            set
            {
                ev.Deleted = value;
            }
        }

		/// <summary>
		/// Whether the event is marked for deletion or not.
		/// </summary>
		public bool NotDeleted
		{
			get
			{
				return !ev.Deleted;
			}
		}

		/// <summary>
		/// Whether the event is the entire day.
		/// </summary>
		public bool AllDayEvent
        {
            get
            {
                return ev.IsAllDayEvent;
            }
        }

        /// <summary>
        /// The UID of the event.
        /// </summary>
        public string UID
        {
            get
            {
                return ev.UID;
            }
            set
            {
                ev.UID = value;
            }
        }

        /// <summary>
        /// Creates a view model of an event for usage in UI.
        /// </summary>
        public EventViewModel(Event ev)
        {
            this.ev = ev;
        }
    }
}