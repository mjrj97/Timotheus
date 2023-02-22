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
            set
            { 
                person.Name = value;
                NotifyPropertyChanged(nameof(Name));
            }
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
            set
            {
                person.ConsentDate = value;
                NotifyPropertyChanged(nameof(Date));
                NotifyPropertyChanged(nameof(SortableDate));
            }
        }

        public string Version
        {
            get { return person.ConsentVersion; }
            set 
            { 
                person.ConsentVersion = value;
                NotifyPropertyChanged(nameof(Version));
            }
        }

        public string Comment
        {
            get { return person.Comment; }
            set
            { 
                person.Comment = value;
                NotifyPropertyChanged(nameof(Comment));
            }
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
                NotifyPropertyChanged(nameof(Inactive));
            }
        }

        public bool Inactive
        {
            get
            {
                return !person.Active;
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