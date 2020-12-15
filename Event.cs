namespace Manager
{
    public class Event
    {
        public int StartTime { get; set;}
        public int EndTime { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Event(int StartTime, int EndTime, string Name, string Description)
        {
            this.StartTime = StartTime;
            this.EndTime = EndTime;
            this.Name = Name;
            this.Description = Description;
        }
    }
}