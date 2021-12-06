using Avalonia.Controls;
using System.ComponentModel;

namespace Timotheus.Utility
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

        public new event PropertyChangedEventHandler PropertyChanged;
        internal void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}