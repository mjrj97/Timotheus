﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Timotheus.Persons;

namespace Timotheus.ViewModels
{
    public class PeopleViewModel : ViewModel
    {
        private PersonRepository _personRepo = new();
        /// <summary>
        /// The repository containing all the people.
        /// </summary>
        public PersonRepository PersonRepo
        {
            get
            {
                return _personRepo;
            }
            set
            {
                _personRepo = value;
            }
        }

        private ObservableCollection<PersonViewModel> _People = new();
        /// <summary>
        /// A list of people that has made consent.
        /// </summary>
        public ObservableCollection<PersonViewModel> People
        {
            get => _People;
            set
            {
                _People = value;
                NotifyPropertyChanged(nameof(People));
            }
        }

        private string _searchField = string.Empty;
        /// <summary>
        /// The search field in the people tab.
        /// </summary>
        public string SearchField
        {
            get
            {
                return _searchField;
            }
            set
            {
                _searchField = value;
                NotifyPropertyChanged(nameof(SearchField));
            }
        }

        private bool _showInactive = true;
        /// <summary>
        /// Whether inactive people should be shown in the people table.
        /// </summary>
        public bool ShowInactive
        {
            get
            {
                return _showInactive;
            }
            set
            {
                _showInactive = value;
                NotifyPropertyChanged(nameof(ShowInactive));
                NotifyPropertyChanged(nameof(HideInactive));
            }
        }

        /// <summary>
        /// The inverse of ShowInactive. Used for UI:
        /// </summary>
        public bool HideInactive
        {
            get
            {
                return !_showInactive;
            }
        }

        public PeopleViewModel(string path = "")
        {
            if (path != string.Empty)
            {
                PersonRepo = new(path);
            }
            else
                PersonRepo = new();
        }

        /// <summary>
        /// Updates the contents of the persons/people table.
        /// </summary>
        public void UpdatePeopleTable()
        {
            People.Clear();
            List<Person> people = PersonRepo.RetrieveAll().OrderBy(o => o.Name).ToList();
            for (int i = 0; i < people.Count; i++)
            {
                if (!people[i].Deleted && (people[i].Active || (!people[i].Active && ShowInactive)) && (people[i].Name.ToLower().Contains(SearchField.ToLower()) || people[i].Comment.ToLower().Contains(SearchField.ToLower())))
                    People.Add(new PersonViewModel(people[i]));
            }

            NotifyPropertyChanged(nameof(People));
        }

        /// <summary>
        /// Adds a person to the list.
        /// </summary>
        public void AddPerson(string Name, DateTime ConsentDate, string ConsentVersion, string ConsentComment)
        {
            PersonRepo.Create(new Person(Name, ConsentDate, ConsentVersion, ConsentComment, true));
            UpdatePeopleTable();
        }

        /// <summary>
        /// Removes the person from list.
        /// </summary>
        public void RemovePerson(PersonViewModel person)
        {
            person.Deleted = true;
            UpdatePeopleTable();
        }
    }
}