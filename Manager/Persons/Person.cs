using System;

namespace Timotheus.Persons
{
    public class Person
    {
        public string Name { get; set; }
        public DateTime ConsentDate { get; set; }
        public string ConsentVersion { get; set; }
        public string Comment { get; set; }
        public bool Active { get; set; }

        public bool Deleted { get; set; }

        public Person(string Name, DateTime ConsentDate, string ConsentVersion, string Comment, bool Active)
        {
            this.Name = Name;
            this.ConsentDate = ConsentDate;
            this.ConsentVersion = ConsentVersion;
            this.Comment = Comment;
            this.Active = Active;
        }
    }
}