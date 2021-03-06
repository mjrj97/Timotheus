using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Timotheus.Utility
{
    public class LocalizationLoader
    {
        readonly List<Localization> locale = new List<Localization>();

        public LocalizationLoader(string culture)
        {
            string file = Application.StartupPath + "locale\\" + culture + ".txt";
            if (File.Exists(file))
            {
                StreamReader steamReader = new StreamReader(file);
                string[] content = steamReader.ReadToEnd().Split("\n");
                steamReader.Close();

                for (int i = 0; i < content.Length; i++)
                {
                    locale.Add(GetValue(content[i]));
                }
            }

            static Localization GetValue(string line)
            {
                int i = 0;
                bool found = false;

                while (i < line.Length && !found)
                {
                    if (line[i] == ',')
                        found = true;
                    else
                        i++;
                }

                string name = line.Substring(0,i);
                string value = line.Substring(i + 1, line.Length - i - 1).Trim();

                return new Localization(name, value);
            }
        }

        public string GetLocalization(string name)
        {
            string value = "";
            bool found = false;
            int i = 0;

            while (!found && i < locale.Count)
            {
                if (locale[i].name == name)
                {
                    value = locale[i].value;
                    found = true;
                }
                i++;
            }

            return value;
        }
    }

    class Localization
    {
        public readonly string name;
        public readonly string value;

        public Localization(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
    }
}