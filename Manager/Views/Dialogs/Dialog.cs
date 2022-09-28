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
        /// Makes sure that the textbox only contain numbers.
        /// </summary>
        protected void IntBox(object sender, KeyEventArgs e)
        {
            // Source: https://stackoverflow.com/questions/14834260/wpf-key-is-digit-or-number
            if (e.Key >= Key.D0 && e.Key <= Key.D9) { } // it`s number
            else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) { } // it`s number
            else if (e.Key == Key.Escape || e.Key == Key.Tab || e.Key == Key.CapsLock || e.Key == Key.LeftShift || e.Key == Key.LeftCtrl ||
                e.Key == Key.LWin || e.Key == Key.LeftAlt || e.Key == Key.RightAlt || e.Key == Key.RightCtrl || e.Key == Key.RightShift ||
                e.Key == Key.Left || e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Right || e.Key == Key.Return || e.Key == Key.Delete ||
                e.Key == Key.System) { } // it`s a system key (add other key here if you want to allow)
            else
                e.Handled = true; // the key will sappressed
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