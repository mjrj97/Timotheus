﻿using System;
using System.Collections.Generic;

namespace Timotheus.Persons
{
    /// <summary>
    /// Object that stores data associated with a person (Member or not).
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Full name of the person.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The persons physical address.
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// This field is used for data not appropriate for the other fields.
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// The persons birthday.
        /// </summary>
        public DateTime Birthday { get; set; }
        /// <summary>
        /// The date the person became a member in the association.
        /// </summary>
        public DateTime Entry { get; set; }
        /// <summary>
        /// Date when consent was given.
        /// </summary>
        public DateTime Signed { get; set; }
        /// <summary>
        /// The versions of consent forms are usually identified by the date of printing/publishing.
        /// </summary>
        public DateTime Version { get; set; }

        /// <summary>
        /// List of created persons.
        /// </summary>
        public static List<Person> list = new List<Person>();

        //Constructors
        public Person(string Name, string Address, DateTime Birthday, DateTime Entry) //Called by AddMember
        {
            this.Name = Name;
            this.Address = Address;
            Comment = string.Empty;
            this.Birthday = Birthday;
            this.Entry = Entry;
            Signed = DateTime.MinValue;
            Version = DateTime.MinValue;

            list.Add(this);
        }
        public Person(string Name, DateTime Signed, DateTime Version, string Comment) //Called by AddConsentForm
        {
            this.Name = Name;
            Address = string.Empty;
            this.Comment = Comment;
            Birthday = DateTime.MinValue;
            Entry = DateTime.MinValue;
            this.Signed = Signed;
            this.Version = Version;

            list.Add(this);
        }
        public Person(string Name, string Address, string Comment, DateTime Birthday, DateTime Entry, DateTime Signed, DateTime Version)
        {
            this.Name = Name;
            this.Address = Address;
            this.Comment = Comment;
            this.Birthday = Birthday;
            this.Entry = Entry;
            this.Signed = Signed;
            this.Version = Version;

            list.Add(this);
        }

        public int CalculateAge()
        {
            int Age = Entry.Year - Birthday.Year;
            if (Entry.DayOfYear < Birthday.DayOfYear)
                Age -= 1;
            return Age;
        }
    }
}