using Avalonia.Input;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using System;
using System.IO;
using Timotheus.ViewModels;
using Timotheus.Views.Dialogs;

namespace Timotheus.Views.Tabs
{
    public partial class PeoplePage : Tab
    {
        public new PeopleViewModel ViewModel
        {
            get
            {
                return (PeopleViewModel)base.ViewModel;
            }
            set
            {
                base.ViewModel = value;
                value.UpdatePeopleTable();
            }
        }

        public PeoplePage()
        {
            AvaloniaXamlLoader.Load(this);
            Title = Localization.People_Page;
            LoadingTitle = Localization.InsertKey_LoadPeople;
            Icon = "avares://Timotheus/Resources/People.png";
            ViewModel = new PeopleViewModel();
        }

        private async void AddPerson_Click(object sender, RoutedEventArgs e)
        {
            AddConsentForm dialog = new();
            await dialog.ShowDialog(MainWindow.Instance);
            if (dialog.DialogResult == DialogResult.OK)
            {
                if (dialog.ConsentVersion == string.Empty)
                {
                    WarningDialog messageBox = new()
                    {
                        DialogTitle = Localization.Exception_Warning,
                        DialogText = Localization.AddConsentForm_EmptyVersion
                    };
                    await messageBox.ShowDialog(MainWindow.Instance);
                    if (messageBox.DialogResult == DialogResult.OK)
                    {
                        ViewModel.AddPerson(dialog.ConsentName, dialog.ConsentDate, dialog.ConsentVersion, dialog.ConsentComment);
                    }
                }
                else
                    ViewModel.AddPerson(dialog.ConsentName, dialog.ConsentDate, dialog.ConsentVersion, dialog.ConsentComment);
            }
        }

        /// <summary>
        /// Saves the current file. If no file is in use, a SaveFileDialog is opened.
        /// </summary>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = Keys.Retrieve("Person-File");
                if (!File.Exists(path))
                    SaveAs_Click(null, e);
                else
                {
                    ViewModel.Save(path);
                }
            }
            catch (Exception ex)
            {
                Program.Error(Localization.Exception_Saving, ex, MainWindow.Instance);
            }
        }

        /// <summary>
        /// Opens a SaveFileDialog to save the current Calendar.
        /// </summary>
        private async void SaveAs_Click(object sender, RoutedEventArgs e)
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
                    ViewModel.Save(result);
                    if (sender == null)
                        Keys.Update("Person-File", result);
                }
                catch (Exception ex)
                {
                    Program.Error(Localization.Exception_Saving, ex, MainWindow.Instance);
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
                ViewModel = new(result[0]);
                Keys.Update("Person-File", result[0]);
            }
        }

        private void ToggleActivePerson_Click(object sender, RoutedEventArgs e)
        {
            PersonViewModel person = (PersonViewModel)((Button)e.Source).DataContext;
            person.Active = !person.Active;
            NotifyPropertyChanged(nameof(ViewModel.People));
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
                WarningDialog msDialog = new()
                {
                    DialogTitle = Localization.Exception_Warning,
                    DialogText = Localization.People_Delete.Replace("#", person.Name)
                };
                await msDialog.ShowDialog(MainWindow.Instance);
                if (msDialog.DialogResult == DialogResult.OK)
                {
                    ViewModel.RemovePerson(person);
                }
            }
        }

        private void ToggleInactive_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ShowInactive = !ViewModel.ShowInactive;
            ViewModel.UpdatePeopleTable();
        }

        private void SearchPeople(object sender, KeyEventArgs e)
        {
            ViewModel.UpdatePeopleTable();
        }

        public override void Load()
        {
            if (Keys.Retrieve("Person-File") != string.Empty)
            {
                try
                {
                    ViewModel = new(Keys.Retrieve("Person-File"));
                }
                catch (Exception ex) 
                {
                    Program.Log(ex);
                    ViewModel = new();
                }
            }
            else
                ViewModel = new();
        }

        public override bool HasBeenChanged()
        {
            return ViewModel.HasBeenChanged;
        }
    }
}