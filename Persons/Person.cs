using System;
using System.Collections.Generic;


namespace Timotheus.Persons
{
    public class Person
    {
        public string name { get; set; }
        public string adresse { get; set; }
        public DateTime birthday { get; set; }
        public DateTime memberSince { get; set; }

        //Constructor
        public Person(string Name, string Address, DateTime Birthday, DateTime MemberSince)
        {
            name = Name;
            adresse = Address;
            birthday = Birthday;
            memberSince = MemberSince;
            
        }
    }
}
