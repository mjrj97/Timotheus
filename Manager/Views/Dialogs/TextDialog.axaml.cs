using Avalonia.Markup.Xaml;
using Timotheus.Utility;

namespace Timotheus.Views.Dialogs
{
    public partial class TextDialog : Dialog
    {
        private string _text = string.Empty;
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                NotifyPropertyChanged(nameof(Text));
            }
        }

        public TextDialog()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = this;
        }
    }
}