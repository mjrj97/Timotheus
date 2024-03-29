using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Timotheus.Views.Dialogs
{
    public partial class FirstTimeSetup : Dialog
    {
        public string Path { get; private set; }

        public FirstTimeSetup()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override async void Ok_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();

            FileDialogFilter txtFilter = new();
            txtFilter.Extensions.Add("txt");
            txtFilter.Extensions.Add("tkey");
            txtFilter.Name = "Key files (.txt, .tkey)";

            openFileDialog.Filters = new();
            openFileDialog.Filters.Add(txtFilter);

            string[] result = await openFileDialog.ShowAsync(this);
            if (result != null && result.Length > 0)
                Path = result[0];

            DialogResult = DialogResult.OK;
        }
    }
}