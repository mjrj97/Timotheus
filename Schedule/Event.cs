using System;

namespace Manager.Schedule
{
    public class Event
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public readonly DateTime Created;

        public string Name { get; set; }
        public string Description { get; set; }
        public readonly string id;

        public Event(DateTime StartTime, DateTime EndTime, DateTime Created, string Name, string Description, string id) 
        {
            this.StartTime = StartTime;
            this.EndTime = EndTime;
            this.Name = Name;
            this.Description = Description;
            this.Created = Created;
            this.id = id;
        }
        public Event(DateTime StartTime, DateTime EndTime, string Name, string Description, string id) : this(StartTime, EndTime, DateTime.Now, Name, Description, id) { }
    }
}