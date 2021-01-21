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
        public string UID;

        public static readonly string DELETE_TAG = "DELETE";

        //Constructors
        public Event(DateTime StartTime, DateTime EndTime, DateTime Created, string Name, string Description, string UID)
        {
            this.StartTime = StartTime;
            this.EndTime = EndTime;
            this.Name = Name;
            this.Description = Description;
            this.Created = Created;
            if (UID == null)
                this.UID = GenerateUID();
            else
                this.UID = UID;
        }
        public Event(DateTime StartTime, DateTime EndTime, string Name, string Description, string UID) : this(StartTime, EndTime, DateTime.Now, Name, Description, UID) { }
        public Event(DateTime StartTime, DateTime EndTime, string Name, string Description) : this(StartTime, EndTime, DateTime.Now, Name, Description, null) { }

        public static string GenerateUID()
        {
            Byte[] data = new Byte[16];
            Random rnd = new Random();
            rnd.NextBytes(data);
            string tempUID = BitConverter.ToString(data).Replace("-", "");
            string UID = tempUID.Substring(0,8) + "-" + tempUID.Substring(8,4) + "-" + tempUID.Substring(12, 4) + "-" + tempUID.Substring(16, 4) + "-" + tempUID.Substring(20, 12);
            return UID;
        }

        public Event Copy()
        {
            return new Event(StartTime, EndTime, Created, Name, Description, UID);
        }

        public override bool Equals(object obj)
        {
            bool equals = false;
            if (obj != null && obj is Event @event)
            {
                Event ev = @event;
                equals = StartTime.Equals(ev.StartTime) && EndTime.Equals(ev.EndTime) && Created.Equals(ev.Created) && Name.Equals(ev.Name) && Description.Equals(ev.Description) && UID == ev.UID;
            }
            return equals;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}