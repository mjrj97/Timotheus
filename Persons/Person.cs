using System;
using Timotheus.Forms;

namespace Timotheus.Persons
{
    /// <summary>
    /// Object that stores data associated with a person (Member or not).
    /// </summary>
    public class Person
    {
        //Private versions of the variables below. Used if a custom setter is defined.
        private DateTime birthday;
        private DateTime entry;

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
        public DateTime Birthday
        { 
            get
            {
                return birthday;
            }
            set
            {
                birthday = value;
                MainWindow.window.CountMembersUnder25();
            }
        }
        /// <summary>
        /// The date the person became a member in the association.
        /// </summary>
        public DateTime Entry
        {
            get
            {
                return entry;
            }
            set
            {
                entry = value;
                MainWindow.window.CountMembersUnder25();
            }
        }
        /// <summary>
        /// Date when consent was given.
        /// </summary>
        public DateTime Signed { get; set; }
        /// <summary>
        /// The versions of consent forms are usually identified by the date of printing/publishing.
        /// </summary>
        public DateTime Version { get; set; }

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
        }
        public Person(string Name, DateTime Signed, DateTime Version, string Comment) //Called by AddConsentForm
        {
            this.Name = Name;
            Address = string.Empty;
            this.Comment = Comment;
            this.Birthday = DateTime.MinValue;
            this.Entry = DateTime.MinValue;
            this.Signed = Signed;
            this.Version = Version;
        }
    }
}