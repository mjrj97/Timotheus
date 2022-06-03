using Avalonia.Controls;
using Timotheus.ViewModels;

namespace Timotheus.Views
{
    public abstract class Tab : UserControl
    {
        public string LoadingTitle { get; set; }

        protected MainViewModel MVM
        {
            get
            {
                return MainViewModel.Instance;
            }
        }

        public abstract void Load();
    }
}
