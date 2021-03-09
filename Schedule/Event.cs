using System;

namespace Timotheus.Schedule
{
    /// <summary>
    /// A class that conforms to the iCal definition of a calendar event.
    /// </summary>
    public class Event
    {
        //Hidden versions that holds the values of the public variables.
        private string name;
        private string description;
        private string location;

        /// <summary>
        /// Start time of the event.
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// End time of the event.
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// Last time that the event was changed.
        /// </summary>
        public DateTime Changed;
        /// <summary>
        /// Time when the event was created.
        /// </summary>
        public DateTime Created;

        /// <summary>
        /// Name of the event. Cannot be multiple lines.
        /// </summary>
        public string Name { get { return name;  } set { name = value.Replace("\r\n", ""); Changed = DateTime.Now; } }
        /// <summary>
        /// Description of the event.
        /// </summary>
        public string Description { get { return description; } set { description = value; Changed = DateTime.Now; } }
        /// <summary>
        /// Location of the event. Is often an address. Cannot be multiple lines.
        /// </summary>
        public string Location { get { return location; } set { location = value.Replace("\r\n", ""); Changed = DateTime.Now; } }
        /// <summary>
        /// Unique identifier of the event. Cannot be changed.
        /// </summary>
        public readonly string UID;
        /// <summary>
        /// Is true of the event is marked for deletion. Is used instead of just deleting the event, so the calendar knows which event was deleted locally when syncing.
        /// </summary>
        public bool Deleted;

        //Constructors
        public Event(DateTime StartTime, DateTime EndTime, DateTime Created, string Name, string Description, string Location, string UID)
        {
            this.StartTime = StartTime;
            this.EndTime = EndTime;
            this.Created = Created;
            this.Name = Name;
            this.Location = Location;
            this.Description = Description;
            Changed = Created;
            Deleted = false;
            if (UID == null)
                this.UID = GenerateUID();
            else
                this.UID = UID;
        }
        public Event(DateTime StartTime, DateTime EndTime, string Name, string Description, string Location, string UID) : this(StartTime, EndTime, DateTime.Now, Name, Description, Location, UID) { }
        public Event(DateTime StartTime, DateTime EndTime, string Name, string Description, string UID) : this(StartTime, EndTime, DateTime.Now, Name, Description, null, UID) { }
        public Event(DateTime StartTime, DateTime EndTime, string Name, string Description) : this(StartTime, EndTime, DateTime.Now, Name, Description, null, null) { }

        /// <summary>
        /// Generates a UID to be used a unique identifier in the calendar.
        /// </summary>
        public static string GenerateUID()
        {
            byte[] data = new byte[16];
            Random rnd = new Random();
            rnd.NextBytes(data);
            string tempUID = BitConverter.ToString(data).Replace("-", "");
            string UID = tempUID.Substring(0,8) + "-" + tempUID.Substring(8,4) + "-" + tempUID.Substring(12, 4) + "-" + tempUID.Substring(16, 4) + "-" + tempUID.Substring(20, 12);
            return UID;
        }

        /// <summary>
        /// Updates the variables of this event with the variables of a separate event ev.
        /// </summary>
        /// <param name="ev">Newer version of this (Must have the same UID).</param>
        public void Update(Event ev)
        {
            if (UID == ev.UID)
            {
                Name = ev.Name;
                Description = ev.Description;
                Location = ev.Location;
                StartTime = ev.StartTime;
                EndTime = ev.EndTime;
                Created = ev.Created;
            }
        }

        /// <summary>
        /// Checks if the event is in a given period of time.
        /// </summary>
        /// <param name="a">Start time of the interval.</param>
        /// <param name="b">End time of the interval.</param>
        public bool IsInPeriod(DateTime a, DateTime b)
        {
            return (StartTime > a && StartTime < b) || (EndTime > a && EndTime < b);
        }

        /// <summary>
        /// Checks if another object has the same values as this.
        /// </summary>
        public override bool Equals(object obj)
        {
            bool equals = false;
            if (obj != null && obj is Event @event)
            {
                Event ev = @event;
                equals = StartTime.Equals(ev.StartTime) && EndTime.Equals(ev.EndTime) && Created.Equals(ev.Created) && Name.Equals(ev.Name) && Description.Equals(ev.Description) && Location.Equals(ev.Location) && UID == ev.UID;
            }
            return equals;
        }

        /// <summary>
        /// Returns the hash code of this.
        /// </summary>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}