using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Timotheus.Utility
{
    /// <summary>
    /// Class used to load a localization file and to access values.
    /// </summary>
    public class LocalizationLoader
    {
        /// <summary>
        /// List of localization values that was loaded by the constructor.
        /// </summary>
        private readonly List<Localization> locale = new List<Localization>();

        /// <summary>
        /// Constructor. Loads a .txt file with name culture from the given path. Each line has the format NAME,VALUE, where NAME is the variables name and value is the given word in a language.
        /// </summary>
        /// <param name="path">Path to localization folder. Must end with a \.</param>
        /// <param name="culture">The culture/language used by the program. e.g. en-GB.</param>
        public LocalizationLoader(string path, string culture)
        {
            string file = path + culture + ".txt";
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

        /// <summary>
        /// Finds the localization with the name.
        /// </summary>
        /// <param name="name">The name of the variable (e.g. Calendar_Page).</param>
        public string GetLocalization(string name)
        {
            string value = string.Empty;
            bool found = false;
            int i = 0;

            while (!found && i < locale.Count)
            {
                if (!locale[i].found && locale[i].name == name)
                {
                    value = locale[i].value;
                    locale[i].found = true;
                    found = true;
                }
                i++;
            }

            return value;
        }
        /// <summary>
        /// Finds the localization with the name, but a standard value can be provided in case the variable wasn't found.
        /// </summary>
        /// <param name="name">The name of the variable (e.g. Calendar_Page).</param>
        /// <param name="standard">The standard value in case the variable wasn't found.</param>
        public string GetLocalization(string name, string standard)
        {
            string value = GetLocalization(name);
            if (value == string.Empty)
                return standard;
            else
                return value;
        }
        /// <summary>
        /// Finds the localization using control.Name.
        /// </summary>
        /// <param name="control">The control object that needs a language specific text.</param>
        public string GetLocalization(Control control)
        {
            string value = GetLocalization(control.Name);
            if (value == string.Empty)
                return control.Text;
            else
                return value;
        }
        /// <summary>
        /// Finds the localization using column.Name.
        /// </summary>
        /// <param name="column">The column that needs a language specific header text.</param>
        public string GetLocalization(DataGridViewColumn column)
        {
            string value = GetLocalization(column.Name);
            if (value == string.Empty)
                return column.HeaderText;
            else
                return value;
        }
    }

    /// <summary>
    /// Class used to contain loaded localization data.
    /// </summary>
    class Localization
    {
        public readonly string name;
        public readonly string value;
        public bool found = false;

        public Localization(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
    }
}