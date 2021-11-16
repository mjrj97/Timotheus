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
}