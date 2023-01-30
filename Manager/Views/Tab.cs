using Avalonia.Media;
using Avalonia.Controls;
using Avalonia.Threading;
using System.ComponentModel;
using Timotheus.IO;
using Timotheus.ViewModels;

namespace Timotheus.Views
{
    public abstract class Tab : UserControl, INotifyPropertyChanged
    {
        #region Colors
        protected readonly IBrush NewLight = new SolidColorBrush(Color.FromRgb(230, 255, 230));
        protected readonly IBrush NewDark = new SolidColorBrush(Color.FromRgb(210, 255, 210));

        protected readonly IBrush UpdateLight = new SolidColorBrush(Color.FromRgb(255, 255, 230));
        protected readonly IBrush UpdateDark = new SolidColorBrush(Color.FromRgb(255, 255, 200));

        protected readonly IBrush DeleteLight = new SolidColorBrush(Color.FromRgb(255, 230, 230));
        protected readonly IBrush DeleteDark = new SolidColorBrush(Color.FromRgb(255, 210, 210));

        protected readonly IBrush StdLight = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        protected readonly IBrush StdDark = new SolidColorBrush(Color.FromRgb(240, 240, 240));
        #endregion

        /// <summary>
        /// Title of the tab shown in the TabControl.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Icon of the tab shown in the TabControl.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Title of the tab when loading the project.
        /// </summary>
        public string LoadingTitle { get; set; }
        
        public Register Keys { get; set; }

        public ViewModel ViewModel 
        { 
            get
            {
                return (ViewModel)DataContext;
            }
            protected set
            {
                Dispatcher.UIThread.InvokeAsync(delegate
                {
                    DataContext = value;
                });
            }
        }

        public Tab() { }

        /// <summary>
        /// Makes the tab load it's contents using it's current keys.
        /// </summary>
        public abstract void Load();

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