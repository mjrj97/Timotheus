using System;

namespace Manager.Schedule
{
    public class Event
    {
        public DateTime StartTime;
        public DateTime EndTime;
        public readonly DateTime Created;

        public string Name;
        public string Description;
        public string ID;

        //Constructors
        public Event(DateTime StartTime, DateTime EndTime, DateTime Created, string Name, string Description, string ID)
        {
            this.StartTime = StartTime;
            this.EndTime = EndTime;
            this.Name = Name;
            this.Description = Description;
            this.Created = Created;
            this.ID = ID;
        }
        public Event(DateTime StartTime, DateTime EndTime, string Name, string Description, string ID) : this(StartTime, EndTime, DateTime.Now, Name, Description, ID) { }
        public Event(DateTime StartTime, DateTime EndTime, string Name, string Description) : this(StartTime, EndTime, DateTime.Now, Name, Description, null) { }
    
        public static Event Copy(Event ev)
        {
            return new Event(ev.StartTime, ev.EndTime, ev.Created, ev.Name, ev.Description, ev.ID);
        }
    }
}