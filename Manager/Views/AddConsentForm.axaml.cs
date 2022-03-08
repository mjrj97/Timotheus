using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using Timotheus.Utility;

namespace Timotheus.Views
{
    public partial class AddConsentForm : Dialog
    {
        /// <summary>
        /// The name of the person giving consent.
        /// </summary>
        public string ConsentName { get; set; }

        private DateTime _consentDate;
        /// <summary>
        /// The date of where consent was given.
        /// </summary>
        public DateTime ConsentDate
        {
            get
            {
                return _consentDate;
            }
            set
            {
                _consentDate = value;
                NotifyPropertyChanged(nameof(ConsentDate));
            }
        }

        /// <summary>
        /// The version of the consent form.
        /// </summary>
        public int ConsentVersion { get; set; }

        /// <summary>
        /// Any comment given to the consent.
        /// </summary>
        public string ConsentComment { get; set; }

        /// <summary>
        /// Error message given and shown in the dialog.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Constructor creating the dialog.
        /// </summary>
        public AddConsentForm()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = this;
        }

        /// <summary>
        /// Closes the dialog and sets the DialogResult to OK.
        /// </summary>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }

        /// <summary>
        /// Closes the dialog and sets the DialogResult to Cancel.
        /// </summary>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}