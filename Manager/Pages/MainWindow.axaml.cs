using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Timotheus.Schedule;

namespace Timotheus
{
    public partial class MainWindow : Window
    {
        public Data data = new();
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
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string[] result = await openFileDialog.ShowAsync(this);
            if (result != null)
            {
                string Message = "";
                foreach (string text in result)
                {
                    Message += text + "\n";
                }
                data.Caption = Message;
            }

            await MessageBox.Show(this, "Oh shit der er gået noget galt med dit program!", "Test title", MessageBox.MessageBoxButtons.YesNoCancel);
        }

        /// <summary>
        /// Changes the selected year and calls UpdateTable.
        /// </summary>
        private void Period_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                Button button = (Button)sender;
                if (button.Name == "+")
                    data.UpdatePeriod(true);
                else if (button.Name == "-")
                    data.UpdatePeriod(false);
            }
        }

        private async void OpenCalendar_Click(object sender, RoutedEventArgs e)
        {
            Schedule.Calendar calendar = await OpenCalendar.Show(this);
            if (calendar != null)
                data.Calendar = calendar;
        }

        private void PeriodBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                data.calendarPeriod.SetPeriod(((TextBox)sender).Text);
        }

        private async void OpenWindow_Click(object sender, RoutedEventArgs e)
        {
            NewPage newPage = new NewPage();
            await newPage.ShowDialog<string>(this);
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            data.Remove((Event)((Button)e.Source).DataContext);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = data;
        }
    }
}