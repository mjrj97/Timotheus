using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Timotheus.Persons
{
    public class Memberlist
    {
        public List<Person> list;

        public Memberlist(string path)
        {
            string text = File.ReadAllText(path);
            string[] lines = Regex.Split(text, "\r\n|\r|\n");

            string[] headers = Regex.Split(lines[0], ",");
            int[] indices = { -1, -1, -1, -1, -1, -1, -1 };
            for (int i = 0; i < headers.Length; i++)
            {
                switch (headers[i])
                {
                    case "Name":
                        indices[0] = i;
                        break;
                    case "Address":
                        indices[1] = i;
                        break;
                    case "Comment":
                        indices[2] = i;
                        break;
                    case "Birthday":
                        indices[3] = i;
                        break;
                    case "Entry":
                        indices[4] = i;
                        break;
                    case "Signed":
                        indices[5] = i;
                        break;
                    case "Version":
                        indices[6] = i;
                        break;
                }
            }

            for (int i = 1; i < lines.Length; i++)
            {
                string[] values = Regex.Split(lines[i], ",");

                string name = indices[0] != -1 ? values[indices[0]] : string.Empty;
                string address = indices[0] != -1 ? values[indices[1]] : string.Empty;
                string comment = indices[0] != -1 ? values[indices[2]] : string.Empty;
                string birthday = indices[0] != -1 ? values[indices[3]] : string.Empty;
                string entry = indices[0] != -1 ? values[indices[4]] : string.Empty;
                string signed = indices[0] != -1 ? values[indices[5]] : string.Empty;
                string version = indices[0] != -1 ? values[indices[6]] : string.Empty;

                Person person = new Person(name, address, comment, DateTime.Parse(birthday), DateTime.Parse(entry), DateTime.Parse(signed), DateTime.Parse(version));
                list.Add(person);
            }
        }
    }
}