using System;
using Timotheus.Persons;

namespace Timotheus.ViewModels
{
    public class PersonViewModel : ViewModel
    {
        private readonly Person person;

        public string Name
        {
            get { return person.Name; }
            set { person.Name = value; }
        }

        public string Date
        {
            get
            {
                if (person.ConsentDate == DateTime.MinValue)
                    return "No consent";
                else
                    return person.ConsentDate.Date.ToString("d");
            }
        }

        public DateTime SortableDate
        {
            get
            {
                return person.ConsentDate;
            }
        }

        public string Version
        {
            get { return person.ConsentVersion; }
            set { person.ConsentVersion = value; }
        }

        public string Comment
        {
            get { return person.Comment; }
            set { person.Comment = value; }
        }

        public bool Active
        {
            get
            {
                return person.Active;
            }
            set
            {
                person.Active = value;
                NotifyPropertyChanged(nameof(Active));
            }
        }

        public bool Deleted
        {
            get
            {
                return person.Deleted;
            }
            set
            {
                person.Deleted = value;
                NotifyPropertyChanged(nameof(Deleted));
            }
        }

        public PersonViewModel(Person person)
        {
            this.person = person;
        }
    }
}