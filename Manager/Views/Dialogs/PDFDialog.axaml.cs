using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System;
using Timotheus.Utility;

namespace Timotheus.Views.Dialogs
{
    public partial class PDFDialog : Dialog
    {
        private Bitmap _editorImage = null;
        public Bitmap EditorImage
        {
            get
            {
                return _editorImage;
            }
            set
            {
                _editorImage = value;
                NotifyPropertyChanged(nameof(EditorImage));
            }
        }

        private string _logoPath = string.Empty;
        public string LogoPath
        {
            get
            {
                return _logoPath;
            }
            set
            {
                _logoPath = value;
                if (System.IO.File.Exists(value))
                    EditorImage = new Bitmap(value);
                NotifyPropertyChanged(nameof(LogoPath));
            }
        }

        private string _title = string.Empty;
        public string PDFTitle
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                NotifyPropertyChanged(nameof(PDFTitle));
            }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get
            {
                return _subtitle;
            }
            set
            {
                _subtitle = value;
                NotifyPropertyChanged(nameof(Subtitle));
            }
        }

        private string _footer = string.Empty;
        public string Footer
        {
            get
            {
                return _footer;
            }
            set
            {
                _footer = value;
                NotifyPropertyChanged(nameof(Footer));
            }
        }

        private string _comment = string.Empty;
        public string Comment
        {
            get
            {
                return _comment;
            }
            set
            {
                _comment = value;
                NotifyPropertyChanged(nameof(Comment));
            }
        }

        private string _backpage = string.Empty;
        public string Backpage
        {
            get
            {
                return _backpage;
            }
            set
            {
                _backpage = value;
                NotifyPropertyChanged(nameof(Backpage));
            }
        }

        private int _currentTab = 0;
        public int CurrentTab 
        { 
            get
            {
                return _currentTab;
            }
            set
            {
                _currentTab = value;
                NotifyPropertyChanged(nameof(CurrentTab));
            }
        }

        private string _exportPath = string.Empty;
        public string ExportPath
        {
            get
            {
                return _exportPath;
            }
            set
            {
                _exportPath = value;
                NotifyPropertyChanged(nameof(ExportPath));
            }
        }

        private string _archivePath = string.Empty;
        public string ArchivePath 
        { 
            get 
            { 
                return _archivePath; 
            } 
            set 
            { 
                _archivePath = value;
                NotifyPropertyChanged(nameof(ArchivePath));
            }
        }

        private bool _saveToArchive = true;
        public bool SaveToArchive 
        { 
            get
            {
                return _saveToArchive;
            }
            set
            {
                _saveToArchive = value;
                NotifyPropertyChanged(nameof(SaveToArchive));
            }
        }

        public PDFDialog()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = this;
        }

        private async void LogoBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                AllowMultiple = false
            };

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
            {
                LogoPath = result[0];
            }
        }

        private async void ExportBrowse_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new();
            FileDialogFilter filter = new();
            filter.Extensions.Add("pdf");
            filter.Name = "PDF Files (.pdf)";

            saveFileDialog.Filters = new()
            {
                filter
            };

            string result = await saveFileDialog.ShowAsync(this);
            if (result != null && result != string.Empty)
                ExportPath = result;
        }

        private async void ArchiveBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog openFolder = new();
            string path = await openFolder.ShowAsync(this);
            if (path != string.Empty && path != null)
                ArchivePath = path;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ExportPath == string.Empty)
                    throw new Exception(Localization.Localization.Exception_PDFEmptyExport);

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex) 
            {
                Program.Error(Localization.Localization.Exception_Name, ex, this);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
