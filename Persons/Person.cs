using System;
using System.Collections.Generic;


namespace Timotheus.Persons
{
    public class Person
    {
        public string name { get; set; }
        public string address { get; set; }
        public DateTime birthday { get; set; }
        public DateTime memberSince { get; set; }
        public int Age { get; set; }

        //Constructor
        public Person(string Name, string Address, DateTime Birthday, DateTime MemberSince)
        {
            name = Name;
            address = Address;
            birthday = Birthday;
            memberSince = MemberSince;
            
            Age = memberSince.Year - birthday.Year;
            if (memberSince.DayOfYear < birthday.DayOfYear)
                Age -= 1;
        }
    }
}
