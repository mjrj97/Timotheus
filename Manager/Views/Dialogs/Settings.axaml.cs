using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System;
using System.Text.RegularExpressions;
using Timotheus.Utility;

namespace Timotheus.Views.Dialogs
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

        private string _description = string.Empty;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                NotifyPropertyChanged(nameof(Description));
            }
        }

        private string _startTime;
        public string StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                NotifyPropertyChanged(nameof(StartTime));
            }
        }

        private string _endTime;
        public string EndTime
        {
            get { return _endTime; }
            set
            {
                _endTime = value;
                NotifyPropertyChanged(nameof(EndTime));
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

        private int _selectedLanguage = 0;
        public int SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                _selectedLanguage = value;
                NotifyPropertyChanged(nameof(SelectedLanguage));
            }
        }

        private bool _lookForUpdates = true;
        public bool LookForUpdates
        {
            get { return _lookForUpdates; }
            set
            {
                _lookForUpdates = value;
                NotifyPropertyChanged(nameof(LookForUpdates));
            }
        }

        public Settings()
        {
            DataContext = this;
            AvaloniaXamlLoader.Load(this);
        }

        private async void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();

            FileDialogFilter imgFilter = new();
            imgFilter.Extensions.Add("png");
            imgFilter.Extensions.Add("jpg");
            imgFilter.Name = "Images (.png, .jpg)";

            openFileDialog.Filters = new()
            {
                imgFilter
            };

            string[] result = await openFileDialog.ShowAsync(this);
            if (result != null && result.Length > 0)
                ImagePath = result[0];
        }

        /// <summary>
        /// Makes sure that the year fields only contain numbers.
        /// </summary>
        private void FixTime(object sender, KeyEventArgs e)
        {
            string text = ((TextBox)sender).Text;
            try
            {
                Regex regexObj = new(@"[^\d&&:&&.]");
                ((TextBox)sender).Text = regexObj.Replace(text, "");
                NotifyPropertyChanged(nameof(StartTime));
                NotifyPropertyChanged(nameof(EndTime));
            }
            catch (ArgumentException ex)
            {
                Timotheus.Log(ex);
            }
        }

        private void DeleteSettings_Click(object sender, RoutedEventArgs e)
        {
            Timotheus.DeleteRegistry();
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