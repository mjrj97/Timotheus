using System;
using System.Collections.Generic;
using Timotheus.Utility;
using System.IO;

namespace Timotheus.Persons
{
    public class PersonRepository : IRepository<Person>
    {
        private readonly List<Person> people = new();

        private readonly string path;

        public PersonRepository(string path)
        {
            this.path = path;
            Load();
        }

        public PersonRepository()
        {

        }

        public void Create(Person obj)
        {
            people.Add(obj);
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

        public void Load()
        {
            people.Clear();
            if (File.Exists(path))
            {
                using StreamReader reader = new(path, System.Text.Encoding.UTF8);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] data = line.Split(';');
                    if (data.Length < 5)
                        continue;
                    people.Add(new Person(data[0], DateTime.Parse(data[1]), data[2], data[3], data[4] == "True"));
                }
            }
        }

        public void Save(string savePath)
        {
            using StreamWriter writer = new(savePath, false, System.Text.Encoding.UTF8);
            writer.WriteLine("sep=;");
            for (int i = 0; i < people.Count; i++)
            {
                writer.WriteLine(people[i].Name + ";" + people[i].ConsentDate.ToString("d") + ";" + people[i].ConsentVersion + ";" + people[i].Comment + ";" + people[i].Active.ToString());
            }
        }
    }
}