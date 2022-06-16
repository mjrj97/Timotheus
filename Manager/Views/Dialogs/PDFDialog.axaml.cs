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
        #region Table
        private Bitmap _tableImage = null;
        public Bitmap TableImage
        {
            get
            {
                return _tableImage;
            }
            set
            {
                _tableImage = value;
                NotifyPropertyChanged(nameof(TableImage));
            }
        }

        private string _tableLogoPath = string.Empty;
        public string TableLogoPath
        {
            get
            {
                return _tableLogoPath;
            }
            set
            {
                _tableLogoPath = value;
                if (System.IO.File.Exists(value))
                    TableImage = new Bitmap(value);
                NotifyPropertyChanged(nameof(TableLogoPath));
            }
        }

        private string _tableTitle = string.Empty;
        public string TableTitle
        {
            get
            {
                return _tableTitle;
            }
            set
            {
                _tableTitle = value;
                NotifyPropertyChanged(nameof(TableTitle));
            }
        }

        private string _tableSubtitle = string.Empty;
        public string TableSubtitle
        {
            get
            {
                return _tableSubtitle;
            }
            set
            {
                _tableSubtitle = value;
                NotifyPropertyChanged(nameof(TableSubtitle));
            }
        }

        private string _tableFooter = string.Empty;
        public string TableFooter
        {
            get
            {
                return _tableFooter;
            }
            set
            {
                _tableFooter = value;
                NotifyPropertyChanged(nameof(TableFooter));
            }
        }
        #endregion

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

        public PDFDialog()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = this;
        }

        #region Table
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
                TableLogoPath = result[0];
            }
        }
        #endregion

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
