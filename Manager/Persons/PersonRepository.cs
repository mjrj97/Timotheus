using System;
using System.IO;
using System.Collections.Generic;

namespace Timotheus.Persons
{
    public class PersonRepository
    {
        private readonly List<Person> people = new();

        public PersonRepository(string path)
        {
            char separator = ',';

            people.Clear();
            using StreamReader reader = new(path, System.Text.Encoding.UTF8);
            string line;

            int nameIndex = -1;
            int dateIndex = -1;
            int versionIndex = -1;
            int commentIndex = -1;
            int activeIndex = -1;

            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("sep="))
                {
                    separator = line[4];
                    continue;
                }
                string[] data = line.Split(separator);

                if (nameIndex == -1 || dateIndex == -1 || versionIndex == -1 || commentIndex == -1 || activeIndex == -1)
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        switch (data[i])
                        {
                            case "Name":
                                nameIndex = i;
                                break;
                            case "ConsentDate":
                                dateIndex = i;
                                break;
                            case "ConsentVersion":
                                versionIndex = i;
                                break;
                            case "Comment":
                                commentIndex = i;
                                break;
                            case "Active":
                                activeIndex = i;
                                break;
                        }
                    }
                }
                else
                {
                    if (data.Length < 5)
                        continue;
                    people.Add(new Person(data[nameIndex], DateTime.ParseExact(data[dateIndex], "dd/mm/yyyy", null), data[versionIndex], data[commentIndex], data[activeIndex].ToLower() == "true"));
                }
            }
        }

        public PersonRepository() { }

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

        public void Save(string savePath)
        {
            using StreamWriter writer = new(savePath, false, System.Text.Encoding.UTF8);
            writer.WriteLine("sep=" + ';');
            writer.WriteLine("Name" + ';' + "ConsentDate" + ';' + "ConsentVersion" + ';' + "Comment" + ';' + "Active");
            for (int i = 0; i < people.Count; i++)
            {
                writer.WriteLine(people[i].Name + ';' + people[i].ConsentDate.ToString("d").Replace('.', '/') + ';' + people[i].ConsentVersion + ';' + people[i].Comment + ';' + people[i].Active.ToString());
            }
        }
    }
}