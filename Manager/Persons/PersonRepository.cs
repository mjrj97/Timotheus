using System;
using System.Collections.Generic;
using Timotheus.Utility;
using System.IO;

namespace Timotheus.Persons
{
    public class PersonRepository : IRepository<Person>
    {
        private readonly List<Person> people = new();

        public PersonRepository()
        {
            Load(@"C:\Users\marti\Desktop\Test.csv");
        }

        public void Create(Person obj)
        {
            people.Add(obj);
            Save(@"C:\Users\marti\Desktop\Test.csv");
        }

        public void Delete(Person obj)
        {
            people.Remove(obj);
        }

        public Person Retrieve(string id)
        {
            throw new NotImplementedException();
        }

        public List<Person> RetrieveAll()
        {
            return people;
        }

        public void Update(Person obj)
        {
            throw new NotImplementedException();
        }

        private void Load(string path)
        {
            people.Clear();
            if (File.Exists(path))
            {
                using StreamReader reader = new(path, Timotheus.Encoding);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] data = line.Split(';');
                    people.Add(new Person(data[0], DateTime.Parse(data[1]), data[2], data[3], data[4] == "True"));
                }
            }
        }

        private void Save(string path)
        {
            using StreamWriter writer = new(path, false, Timotheus.Encoding);
            for (int i = 0; i < people.Count; i++)
            {
                writer.WriteLine(people[i].Name + ";" + people[i].ConsentDate.ToString() + ";" + people[i].ConsentVersion + ";" + people[i].Comment + ";" + people[i].Active.ToString());
            }
        }
    }
}