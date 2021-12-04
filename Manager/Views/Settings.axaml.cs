using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
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
                if (System.IO.File.Exists(value))
                    Image = new Bitmap(value);
                NotifyPropertyChanged(nameof(ImagePath));
            }
        }

        private Bitmap _image = null;
        public Bitmap Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
                NotifyPropertyChanged(nameof(Image));
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
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}