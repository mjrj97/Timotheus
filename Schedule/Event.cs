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
        public string ID;

        public static readonly string DELETE_TAG = "DELETE";

        //Constructors
        public Event(DateTime StartTime, DateTime EndTime, DateTime Created, string Name, string Description, string ID)
        {
            this.StartTime = StartTime;
            this.EndTime = EndTime;
            this.Name = Name;
            this.Description = Description;
            this.Created = Created;
            if (ID == null)
                this.ID = GenerateUUID();
            else
                this.ID = ID;
        }
        public Event(DateTime StartTime, DateTime EndTime, string Name, string Description, string ID) : this(StartTime, EndTime, DateTime.Now, Name, Description, ID) { }
        public Event(DateTime StartTime, DateTime EndTime, string Name, string Description) : this(StartTime, EndTime, DateTime.Now, Name, Description, null) { }

        public static string GenerateUUID()
        {
            Byte[] data = new Byte[16];
            Random rnd = new Random();
            rnd.NextBytes(data);
            string tempUUID = BitConverter.ToString(data).Replace("-", "").ToLower();
            string UUID = tempUUID.Substring(0,8) + "-" + tempUUID.Substring(8,4) + "-" + tempUUID.Substring(12, 4) + "-" + tempUUID.Substring(16, 4) + "-" + tempUUID.Substring(20, 12);
            return UUID;
        }

        public Event Copy()
        {
            return new Event(StartTime, EndTime, Created, Name, Description, ID);
        }

        public override bool Equals(object obj)
        {
            bool equals = false;
            if (obj != null && obj is Event @event)
            {
                Event ev = @event;
                equals = StartTime.Equals(ev.StartTime) && EndTime.Equals(ev.EndTime) && Created.Equals(ev.Created) && Name.Equals(ev.Name) && Description.Equals(ev.Description) && ID == ev.ID;
            }
            return equals;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}