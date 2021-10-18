using ReactiveUI;
using System;
using System.Collections.ObjectModel;

namespace Timotheus
{
    public class Data : ReactiveObject
    {
        private string _Caption = "500";
        public string Caption
        {
            get => _Caption;
            set => this.RaiseAndSetIfChanged(ref _Caption, value);
        }

        private ObservableCollection<Person> _People = new ObservableCollection<Person>();
        public ObservableCollection<Person> People
        {
            get => _People;
            set => this.RaiseAndSetIfChanged(ref _People, value);
        }

        public Data() {
            People.Add(new Person("13/01/2021 19.00", "13/01/2021 20.30", "Opstartsaften med forlænget andagt", "Mødeleder: Martin JRJ\nMusiker: Lillian MW\nKaffehold:Randi MJ, Casper TN, Clara CW", "Vesterbro 9B, 5000 Odense C"));
            Caption = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        public void Add(Person person)
        {
            People.Add(person);
        }

        public void Remove(Person i)
        {
            People.Remove(i);
        }
    }

    public class Person {
        private string _Start = "";
        public string Start
        {
            get
            {
                return _Start;
            }
            set 
            {
                _Start = value;
            }
        }

        private string _End = "";
        public string End
        {
            get
            {
                return _End;
            }
            set
            {
                _End = value;
            }
        }

        private string _Name = "";
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        private string _Description = "";
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
            }
        }

        private string _Address = "";
        public string Address
        {
            get
            {
                return _Address;
            }
            set
            {
                _Address = value;
            }
        }

        public Person(string Start, string End, string Name, string Description, string Address)
        {
            this.Start = Start;
            this.End = End;
            this.Name = Name;
            this.Description = Description;
            this.Address = Address;
        }
    }
}