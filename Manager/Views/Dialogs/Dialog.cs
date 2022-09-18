using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System.ComponentModel;

namespace Timotheus.Views.Dialogs
{
    public abstract class Dialog : Window, INotifyPropertyChanged
    {
        private DialogResult _dialogResult = DialogResult.None;
        public DialogResult DialogResult
        {
            get
            {
                return _dialogResult;
            }
            set
            {
                _dialogResult = value;
                Close();
            }
        }

        /// <summary>
        /// Sets different events.
        /// </summary>
        protected Dialog()
        {
            PointerPressed += PointerClicked;
            KeyDown += KeyDown_Window;
        }

        /// <summary>
        /// Event when pointer is clicked on Window.
        /// </summary>
        private void PointerClicked(object sender, PointerPressedEventArgs e)
        {
            ((FocusManager)FocusManager.Instance).SetFocusedElement(this, this);
        }

        /// <summary>
        /// Handles key presses in the window.
        /// </summary>
        private void KeyDown_Window(object sender, KeyEventArgs e)
        {
            IInputElement obj = ((FocusManager)FocusManager.Instance).GetFocusedElement(this);
            if (obj is Dialog)
            {
                if (e.Key == Key.Enter)
                    Ok_Click(null, null);
                else if (e.Key == Key.Escape)
                    Cancel_Click(null, null);
            }
        }

        /// <summary>
        /// What happens when 'OK' is pressed on the dialog.
        /// </summary>
        protected virtual void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Closes the dialog and sets the DialogResult to Cancel.
        /// </summary>
        protected virtual void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        public new event PropertyChangedEventHandler PropertyChanged;
        internal void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}