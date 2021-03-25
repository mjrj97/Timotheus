using System;

namespace Timotheus.Persons
{
    public class Person
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime MemberSince { get; set; }
        public int Age { get; set; }

        //Constructor
        public Person(string Name, string Address, DateTime Birthday, DateTime MemberSince)
        {
            this.Name = Name;
            this.Address = Address;
            this.Birthday = Birthday;
            this.MemberSince = MemberSince;
            
            //Caucluater age when the start being member
            Age = this.MemberSince.Year - this.Birthday.Year;
            if (this.MemberSince.DayOfYear < this.Birthday.DayOfYear)
                Age -= 1;
        }
    }
}