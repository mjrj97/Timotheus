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

        /// <summary>
        /// Whether the tab has been changed since load.
        /// </summary>
        /// <returns></returns>
        public abstract bool HasBeenChanged();

        public new event PropertyChangedEventHandler PropertyChanged;
        internal void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
