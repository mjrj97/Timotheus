using System.IO;
using System.Windows.Forms;

namespace Timotheus.Utility
{
    /// <summary>
    /// Class used to contain loaded localization data.
    /// </summary>
    class Localization
    {
        private readonly string name;
        private readonly string value;

        private static Localization[] locals;

        /// <summary>
        /// Constructor a specific localization.
        /// </summary>
        /// <param name="name">Name of the object. ie. Calendar_Page</param>
        /// <param name="value">Localization for the given object. ie. Calendar in en-GB</param>
        private Localization(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        /// <summary>
        /// Loads the localization using the designated culture from a path.
        /// </summary>
        /// <param name="path">Path to localization folder. Must end with a \.</param>
        /// <param name="culture">The culture/language used by the program. e.g. en-GB.</param>
        public static void Initialize(string path, string culture)
        {
            string file = path + culture + ".txt";
            if (File.Exists(file))
            {
                StreamReader steamReader = new StreamReader(file);
                string[] lines = steamReader.ReadToEnd().Split("\n");
                steamReader.Close();
                locals = new Localization[lines.Length];

                for (int i = 0; i < locals.Length; i++)
                {
                    locals[i] = ReadLine(lines[i]);
                }
            }
        }

        /// <summary>
        /// Returns a Localization object with a name and a value (ie. Calendar_Page,Calendar)
        /// </summary>
        /// <param name="line">Line from a localization file.</param>
        private static Localization ReadLine(string line)
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

            string name = line.Substring(0, i);
            string value = line.Substring(i + 1, line.Length - i - 1).Trim();

            return new Localization(name, value);
        }

        /// <summary>
        /// Finds the localization with the name.
        /// </summary>
        /// <param name="name">The name of the variable (e.g. Calendar_Page).</param>
        public static string Get(string name)
        {
            string value = string.Empty;
            bool found = false;
            int i = 0;

            while (!found && i < locals.Length)
            {
                if (locals[i].name == name)
                {
                    value = locals[i].value;
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
        public static string Get(string name, string standard)
        {
            string value = Get(name);
            if (value == string.Empty)
                return standard;
            else
                return value;
        }
        /// <summary>
        /// Finds the localization using control.Name.
        /// </summary>
        /// <param name="control">The control object that needs a language specific text.</param>
        public static string Get(Control control)
        {
            string value = Get(control.Name);
            if (value == string.Empty)
                return control.Text;
            else
                return value;
        }
        /// <summary>
        /// Finds the localization using column.Name.
        /// </summary>
        /// <param name="column">The column that needs a language specific header text.</param>
        public static string Get(DataGridViewColumn column)
        {
            string value = Get(column.Name);
            if (value == string.Empty)
                return column.HeaderText;
            else
                return value;
        }
    }
}