using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Timotheus.Utility;
using Timotheus.ViewModels;
using Timotheus.Views.Dialogs;

namespace Timotheus.Views.Tabs
{
    public partial class PeoplePage : UserControl
    {
        private MainViewModel MVM
        {
            get
            {
                return DataContext as MainViewModel;
            }
        }

        readonly IBrush StdLight = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        readonly IBrush StdDark = new SolidColorBrush(Color.FromRgb(230, 230, 230));

        public PeoplePage()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void AddPerson_Click(object sender, RoutedEventArgs e)
        {
            AddConsentForm dialog = new();
            await dialog.ShowDialog(MainWindow.Instance);
            if (dialog.DialogResult == DialogResult.OK)
            {
                if (dialog.ConsentVersion == string.Empty)
                {
                    MessageBox messageBox = new()
                    {
                        DialogTitle = Localization.Localization.Exception_Warning,
                        DialogText = Localization.Localization.AddConsentForm_EmptyVersion
                    };
                    await messageBox.ShowDialog(MainWindow.Instance);
                    if (messageBox.DialogResult == DialogResult.OK)
                    {
                        MVM.AddPerson(dialog.ConsentName, dialog.ConsentDate, dialog.ConsentVersion, dialog.ConsentComment);
                    }
                }
                else
                    MVM.AddPerson(dialog.ConsentName, dialog.ConsentDate, dialog.ConsentVersion, dialog.ConsentComment);
            }
        }

        private void ToggleActivePerson_Click(object sender, RoutedEventArgs e)
        {
            PersonViewModel person = (PersonViewModel)((Button)e.Source).DataContext;
            person.Active = !person.Active;
            MVM.UpdatePeopleTable();
        }

        private void People_RowLoading(object sender, DataGridRowEventArgs e)
        {
            if (e.Row.DataContext is PersonViewModel person)
            {
                if (person.Active)
                    e.Row.Background = StdLight;
                else
                    e.Row.Background = StdDark;
            }
        }

        private void RemovePerson_Click(object sender, RoutedEventArgs e)
        {
            PersonViewModel person = (PersonViewModel)((Button)e.Source).DataContext;
            if (person != null)
            {
                MVM.Remove(person);
            }
        }

        private void ToggleInactive_Click(object sender, RoutedEventArgs e)
        {
            MVM.ShowInactive = !MVM.ShowInactive;
            MVM.UpdatePeopleTable();
        }

        private void SearchPeople(object sender, KeyEventArgs e)
        {
            MVM.UpdatePeopleTable();
        }
    }
}