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

            FileDialogFilter filter = new();
            filter.Extensions.Add("tkey");
            filter.Name = "Key files (.tkey)";

			openFileDialog.Filters = new()
			{
				filter
			};

			string[] result = await openFileDialog.ShowAsync(this);
            if (result != null && result.Length > 0)
                Path = result[0];

            DialogResult = DialogResult.OK;
        }
    }
}