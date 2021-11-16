using Avalonia.Controls;
using System.ComponentModel;

namespace Timotheus.Utility
{
    public abstract class Dialog : Window, INotifyPropertyChanged
    {
        public DialogResult DialogResult = DialogResult.None;

        public new event PropertyChangedEventHandler PropertyChanged;
        internal void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum DialogResult
    {
        None = 0,
        OK = 1,
        Cancel = 2,
        Abort = 3,
        Retry = 4,
        Ignore = 5,
        Yes = 6,
        No = 7
    }
}
