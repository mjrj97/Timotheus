using System;

namespace Timotheus.Utility
{
    public class ConsentForm
    {
        public string Name { get; set; }
        public DateTime Signed { get; set; }
        public DateTime Version { get; set; }
        public string Comment { get; set; }

        public ConsentForm(string Name, DateTime Signed, DateTime Version, string Comment)
        {
            this.Name = Name;
            this.Signed = Signed;
            this.Version = Version;
            this.Comment = Comment;
        }
        public ConsentForm(string Name, DateTime Signed, DateTime Version) : this(Name, Signed, Version, string.Empty) { }
    }
}