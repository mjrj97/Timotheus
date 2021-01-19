using System;

namespace Manager.Schedule
{
    public class Event
    {
        private DateTime startTime;
        private DateTime endTime;
        private string name;
        private string description;
        private string id;

        public DateTime StartTime { get { return startTime; } set { startTime = value; Edited = true; } }
        public DateTime EndTime { get { return endTime; } set { endTime = value; Edited = true; } }
        public readonly DateTime Created;

        public string Name { get { return name; } set { name = value; Edited = true; } }
        public string Description { get { return description; } set { description = value; Edited = true; } }
        public string ID { get { return id; } set { id = value; Edited = true; } }

        private bool Edited { get; set; }

        //Constructors
        public Event(DateTime StartTime, DateTime EndTime, DateTime Created, string Name, string Description, string ID)
        {
            this.StartTime = StartTime;
            this.EndTime = EndTime;
            this.Name = Name;
            this.Description = Description;
            this.Created = Created;
            this.ID = ID;
            if (ID.Length > 2)
                Edited = false;
        }
        public Event(DateTime StartTime, DateTime EndTime, string Name, string Description, string ID) : this(StartTime, EndTime, DateTime.Now, Name, Description, ID) { }
        public Event(DateTime StartTime, DateTime EndTime, string Name, string Description) : this(StartTime, EndTime, DateTime.Now, Name, Description, "") { }
    }
}