using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System;

namespace Timotheus.Views.Dialogs
{
    public partial class PDFDialog : Dialog
    {
        private Bitmap _editorImage = null;
        /// <summary>
        /// Image that is shown in the editors preview.
        /// </summary>
        private Bitmap EditorImage
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
        /// <summary>
        /// Path to the logo on the local computer.
        /// </summary>
        public string LogoPath
        {
            get
            {
                return _logoPath;
            }
            set
            {
                _logoPath = value;
                NotifyPropertyChanged(nameof(LogoPath));
            }
        }

        private string _title = string.Empty;
        /// <summary>
        /// Title of the PDF
        /// </summary>
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
        /// <summary>
        /// Subtitle of the PDF
        /// </summary>
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
        /// <summary>
        /// Footer of the PDF
        /// </summary>
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
        /// <summary>
        /// Comment on the PDF
        /// </summary>
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
        /// <summary>
        /// Text on the backpage of the PDF.
        /// </summary>
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
        /// <summary>
        /// The index of the current tab.
        /// </summary>
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
        /// <summary>
        /// Path where the PDF should be exported.
        /// </summary>
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
        /// <summary>
        /// Path to the folder where a copy of the exported PDF should be placed.
        /// </summary>
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
        /// <summary>
        /// Whether the PDF should be saved in a archive folder.
        /// </summary>
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

        /// <summary>
        /// Constructor of the dialog.
        /// </summary>
        public PDFDialog()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = this;
        }

        /// <summary>
        /// What happens when 'Browse' is pressed on the dialog, where the user can select the logo to be used.
        /// </summary>
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
                if (System.IO.File.Exists(LogoPath))
                    EditorImage = new Bitmap(LogoPath);
            }
        }

        /// <summary>
        /// Handles key presses in the window.
        /// </summary>
        private void LogoKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (System.IO.File.Exists(LogoPath))
                    EditorImage = new Bitmap(LogoPath);
            }
        }

        /// <summary>
        /// Here the user chooses where to export the PDF to.
        /// </summary>
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

        /// <summary>
        /// Here the user chooses where to archive the PDF to.
        /// </summary>
        private async void ArchiveBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog openFolder = new();
            string path = await openFolder.ShowAsync(this);
            if (path != string.Empty && path != null)
                ArchivePath = path;
        }

        /// <summary>
        /// What happens when 'OK' is pressed on the dialog.
        /// </summary>
        protected override void Ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ExportPath == string.Empty)
                    throw new Exception(Localization.Exception_PDFEmptyExport);

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex) 
            {
                Program.Error(Localization.Exception_Name, ex, this);
            }
        }
    }
}