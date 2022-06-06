using Avalonia.Controls;
using System.ComponentModel;
using Timotheus.ViewModels;

namespace Timotheus.Views
{
    public abstract class Tab : UserControl, INotifyPropertyChanged
    {
        public string LoadingTitle { get; set; }

        public ViewModel ViewModel { get; protected set; }

        public abstract void Load();

        public abstract void Update();

        public abstract bool HasBeenChanged();

        public new event PropertyChangedEventHandler PropertyChanged;
        internal void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
