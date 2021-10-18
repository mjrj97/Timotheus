using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;

namespace Timotheus
{
    public partial class MainWindow : Window
    {
        public Data data = new Data();
        DataGrid dataGrid;

        public MainWindow()
        {
            InitializeComponent();
            #if DEBUG
            this.AttachDevTools();
            #endif

            dataGrid = this.Find<DataGrid>("MyDataGrid");
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            /*OpenFileDialog openFileDialog = new OpenFileDialog();
            string[] result = await openFileDialog.ShowAsync(this);
            if (result != null)
            {
                string Message = "";
                foreach (string text in result)
                {
                    Message += text + "\n";
                }
                data.Caption = Message;
            }*/

            await MessageBox.Show(this, "Oh shit der er gået noget galt med dit program!", "Test title", MessageBox.MessageBoxButtons.YesNoCancel);
        }

        private async void OpenWindow_Click(object sender, RoutedEventArgs e)
        {
            NewPage newPage = new NewPage();
            await newPage.ShowDialog<string>(this);
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            data.Remove((Person)((Button)e.Source).DataContext);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = data;
        }
    }
}