using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using Timotheus.IO;
using Timotheus.Utility;

namespace Timotheus.Views.Dialogs
{
    public partial class EditKey : Dialog
    {
        /// <summary>
        /// The keys usually expected in a key file.
        /// </summary>
        private readonly static string[] std = {
            "Calendar-Email",
            "Calendar-Password",
            "Calendar-URL",
            "Calendar-Path",
            "SSH-URL",
            "SSH-Username",
            "SSH-Password",
            "SSH-RemoteDirectory",
            "SSH-LocalDirectory",
            "Settings-Name",
            "Settings-Address",
            "Settings-Image"
        };

        private string _Text;
        /// <summary>
        /// The text in the Text box. It represents the current Key in Text format.
        /// </summary>
        public string Text
        {
            get => _Text;
            set
            {
                _Text = value;
                NotifyPropertyChanged(nameof(Text));
            }
        }

        /// <summary>
        /// Constructor. Loads the XAML and assigns data context.
        /// </summary>
        public EditKey()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = this;
        }

        /// <summary>
        /// Checks if the text contains all of the standard keys, and if any are missing adds them to the text.
        /// </summary>
        private void AddStdKeys_Click(object sender, RoutedEventArgs e)
        {
            Register register = new(':', Text);
            List<Key> keys = register.RetrieveAll();

            bool firstAdded = Text.Trim() != string.Empty;
            for (int i = 0; i < std.Length; i++)
            {
                bool found = false;

                int j = 0;
                while (j < keys.Count && !found)
                {
                    if (keys[j].Name == std[i])
                        found = true;
                    j++;
                }

                if (!found)
                {
                    if (firstAdded)
                        Text += '\n' + std[i] + ':';
                    else
                    {
                        Text += std[i] + ':';
                        firstAdded = true;
                    }
                }
            }
        }
    }
}