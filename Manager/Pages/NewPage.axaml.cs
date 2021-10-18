using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Timotheus
{
    public partial class NewPage : Window
    {
        public NewPage()
        {
            InitializeComponent();
            #if DEBUG
            this.AttachDevTools();
            #endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
