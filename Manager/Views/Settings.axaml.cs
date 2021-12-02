using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Timotheus.Utility;

namespace Timotheus.Views
{
    public partial class Settings : Dialog
    {
        private string _associationName = string.Empty;
        public string AssociationName
        { 
            get
            {
                return _associationName;
            }
            set
            {
                _associationName = value;
                NotifyPropertyChanged(nameof(AssociationName));
            }
        }

        private string _associationAddress = string.Empty;
        public string AssociationAddress
        {
            get
            {
                return _associationAddress;
            }
            set
            {
                _associationAddress = value;
                NotifyPropertyChanged(nameof(AssociationAddress));
            }
        }

        private string _imagePath = string.Empty;
        public string ImagePath
        {
            get
            {
                return _imagePath;
            }
            set
            {
                _imagePath = value;
                NotifyPropertyChanged(nameof(ImagePath));
            }
        }

        public Settings()
        {
            DataContext = this;
            AvaloniaXamlLoader.Load(this);
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}