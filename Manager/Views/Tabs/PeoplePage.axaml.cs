using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System;
using Timotheus.Utility;
using Timotheus.ViewModels;
using Timotheus.Views.Dialogs;

namespace Timotheus.Views.Tabs
{
    public partial class PeoplePage : Tab
    {
        public PeopleViewModel People
        {
            get
            {
                return (PeopleViewModel)ViewModel;
            }
            set
            {
                ViewModel = value;
            }
        }

        readonly IBrush StdLight = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        readonly IBrush StdDark = new SolidColorBrush(Color.FromRgb(230, 230, 230));

        public PeoplePage()
        {
            LoadingTitle = Localization.Localization.InsertKey_LoadPeople;
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
                        People.AddPerson(dialog.ConsentName, dialog.ConsentDate, dialog.ConsentVersion, dialog.ConsentComment);
                    }
                }
                else
                    People.AddPerson(dialog.ConsentName, dialog.ConsentDate, dialog.ConsentVersion, dialog.ConsentComment);
            }
        }

        /// <summary>
        /// Opens a SaveFileDialog to save the current Calendar.
        /// </summary>
        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new();
            FileDialogFilter filter = new();
            string result;

            filter.Extensions.Add("csv");
            filter.Name = "CSV (.csv)";

            saveFileDialog.Filters = new()
            {
                filter
            };

            result = await saveFileDialog.ShowAsync(MainWindow.Instance);
            if (result != null)
            {
                try
                {
                    People.Save(result);
                }
                catch (Exception ex)
                {
                    Timotheus.Log(ex);
                    MainWindow.Instance.Error(Localization.Localization.Exception_Saving, ex.Message);
                }
            }
        }

        private async void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();

            FileDialogFilter txtFilter = new();
            txtFilter.Extensions.Add("csv");
            txtFilter.Name = "CSV (.csv)";

            openFileDialog.Filters = new()
            {
                txtFilter
            };

            string[] result = await openFileDialog.ShowAsync(MainWindow.Instance);
            if (result != null && result.Length > 0)
            {
                People = new(result[0]);
                DataContext = People;
                Update();
                MainViewModel.Instance.Keys.Update("Person-File", result[0]);
            }
        }

        private void ToggleActivePerson_Click(object sender, RoutedEventArgs e)
        {
            PersonViewModel person = (PersonViewModel)((Button)e.Source).DataContext;
            person.Active = !person.Active;
            Update();
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

        private async void RemovePerson_Click(object sender, RoutedEventArgs e)
        {
            PersonViewModel person = (PersonViewModel)((Button)e.Source).DataContext;
            if (person != null)
            {
                MessageBox msDialog = new()
                {
                    DialogTitle = Localization.Localization.Exception_Warning,
                    DialogText = Localization.Localization.People_Delete.Replace("#", person.Name)
                };
                await msDialog.ShowDialog(MainWindow.Instance);
                if (msDialog.DialogResult == DialogResult.OK)
                {
                    People.RemovePerson(person);
                }
            }
        }

        private void ToggleInactive_Click(object sender, RoutedEventArgs e)
        {
            People.ShowInactive = !People.ShowInactive;
            Update();
        }

        private void SearchPeople(object sender, KeyEventArgs e)
        {
            Update();
        }

        public override void Load()
        {
            if (MainViewModel.Instance.Keys.Retrieve("Person-File") != string.Empty)
            {
                try
                {
                    People = new(MainViewModel.Instance.Keys.Retrieve("Person-File"));
                }
                catch (Exception ex) 
                {
                    Timotheus.Log(ex);
                    People = new();
                }
            }
            else
                People = new();
        }

        public override void Update()
        {
            People.UpdatePeopleTable();
        }

        public override bool HasBeenChanged()
        {
            return People.HasBeenChanged;
        }
    }
}