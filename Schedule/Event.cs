using System;
using System.Text;
using System.Text.RegularExpressions;
using Timotheus.IO;

namespace Timotheus.Schedule
{
    /// <summary>
    /// A class that conforms to the iCal definition of a calendar event.
    /// </summary>
    public class Event : Period
    {
        //Hidden versions that holds the values of the public variables.
        private string name = string.Empty;
        private string description = string.Empty;
        private string location = string.Empty;

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
        public Event(DateTime Start, DateTime End, DateTime Created, string Name, string Description, string Location, string UID)
        {
            this.Start = Start;
            this.End = End;
            this.Created = Created;
            this.Name = Name;
            this.Location = Location;
            this.Description = Description;
            Changed = Created;
            Deleted = false;
            if (UID == null)
                this.UID = Guid.NewGuid().ToString().ToUpper();
            else
                this.UID = UID;
        }
        public Event(DateTime Start, DateTime End, string Name, string Description, string Location, string UID) : this(Start, End, DateTime.Now, Name, Description, Location, UID) { }
        public Event(DateTime Start, DateTime End, string Name, string Description, string UID) : this(Start, End, DateTime.Now, Name, Description, null, UID) { }
        public Event(DateTime Start, DateTime End, string Name, string Description) : this(Start, End, DateTime.Now, Name, Description, null, null) { }
        public Event(string text)
        {
            string[] lines = Regex.Split(text, "\r\n|\r|\n");

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("SUMMARY"))
                    Name = Key.Value(lines[i], ':');
                if (lines[i].Contains("DESCRIPTION"))
                    Description = Calendar.ConvertFromCALString(Key.Value(lines[i], ':'));
                if (lines[i].Contains("LOCATION"))
                    Location = Calendar.ConvertFromCALString(Key.Value(lines[i], ':'));
                if (lines[i].Contains("UID"))
                    UID = Key.Value(lines[i], ':');
                if (lines[i].Contains("DTSTART"))
                    Start = Calendar.StringToDate(Key.Value(lines[i], ':'));
                if (lines[i].Contains("DTEND"))
                    End = Calendar.StringToDate(Key.Value(lines[i], ':'));
                if (lines[i].Contains("DTSTAMP"))
                    Created = Calendar.StringToDate(Key.Value(lines[i], ':'));
            }

            Changed = Created;
        }

        /// <summary>
        /// Updates the variables of this event with the variables of a separate event ev.
        /// </summary>
        /// <param name="ev">Newer version of this (Must have the same UID).</param>
        public void Update(Event ev)
        {
            Name = ev.Name;
            Description = ev.Description;
            Location = ev.Location;
            Start = ev.Start;
            End = ev.End;
            Created = ev.Created;
        }

        /// <summary>
        /// Converts a Event into a iCal string.
        /// </summary>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder("BEGIN:VEVENT\nUID:");
            builder.Append(UID);
            if (Start.Hour == End.Hour && Start.Minute == End.Minute && Start.Second == End.Second && Start.Hour == 0 && Start.Minute == 0 && Start.Second == 0)
            {
                builder.Append("\nDTSTART;TZID=Europe/Copenhagen:");
                builder.Append(Calendar.DateToString(Start));
                builder.Append("\nDTEND;TZID=Europe/Copenhagen:");
                builder.Append(Calendar.DateToString(End));
            }
            else
            {
                builder.Append("\nDTSTART;TZID=Europe/Copenhagen:");
                builder.Append(Calendar.DateTimeToString(Start));
                builder.Append("\nDTEND;TZID=Europe/Copenhagen:");
                builder.Append(Calendar.DateTimeToString(End));
            }
            if (Description != string.Empty)
            {
                builder.Append("\nDESCRIPTION:");
                builder.Append(Calendar.ConvertToCALString(Description));
            }
            builder.Append("\nDTSTAMP:");
            builder.Append(Calendar.DateTimeToString(Changed));
            builder.Append('Z');
            if (Location != string.Empty)
            {
                builder.Append("\nLOCATION:");
                builder.Append(Calendar.ConvertToCALString(Location));
            }
            builder.Append("\nSUMMARY:");
            builder.Append(Name);
            builder.Append("\nEND:VEVENT");

            return builder.ToString();
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
                equals = Start.Equals(ev.Start) && End.Equals(ev.End) && Created.Equals(ev.Created) && Name.Equals(ev.Name) && Description.Equals(ev.Description) && Location.Equals(ev.Location) && UID == ev.UID;
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