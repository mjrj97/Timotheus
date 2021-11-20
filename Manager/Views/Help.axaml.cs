using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Timotheus
{
    public partial class Help : Window
    {
        private string _Version = Localization.Localization.Help_VersionLabel + " " + Timotheus.Version;
        public string Version
        {
            get { return _Version; }
            set { _Version = value; }
        }

        private string _License = Localization.Localization.Help_LicenseLabel + " Apache 2.0";
        public string License
        {
            get { return _License; }
            set { _License = value; }
        }

        private string _Source = Localization.Localization.Help_SourceLabel + " https://github.com/mjrj97/Timotheus";
        public string Source
        {
            get { return _Source; }
            set { _Source = value; }
        }

        private string _Email = Localization.Localization.Help_EmailLabel + " martin.jensen.1997@hotmail.com";
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        private string _Contributors = Localization.Localization.Help_ContributorsLabel + " Martin J. R. Jensen & Jesper Roager";
        public string Contributors
        {
            get { return _Contributors; }
            set { _Contributors = value; }
        }

        public Help()
        {
            DataContext = this;
            AvaloniaXamlLoader.Load(this);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
