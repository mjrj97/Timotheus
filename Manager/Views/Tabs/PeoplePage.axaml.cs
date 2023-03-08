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
        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = Keys.Retrieve("Person-File");
                if (!File.Exists(path))
                    SaveAs_Click(null, e);
                else
                {
                    ViewModel.Save(path);
					MessageDialog confirmation = new()
					{
						DialogTitle = Localization.Confirmation,
						DialogText = Localization.Confirmation_SaveConsentForms,
                        DialogShowCancel = false
					};
					await confirmation.ShowDialog(MainWindow.Instance);
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

					MessageDialog confirmation = new()
					{
						DialogTitle = Localization.Confirmation,
						DialogText = Localization.Confirmation_SaveConsentForms,
						DialogShowCancel = false
					};
					await confirmation.ShowDialog(MainWindow.Instance);

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
            bool open = true;

            if (HasBeenChanged() || Keys.Retrieve("Person-File") != string.Empty)
            {
				WarningDialog warning = new()
				{
					DialogTitle = Localization.Exception_Warning,
					DialogText = Localization.AddConsentForm_OpenWarning,
					DialogShowCancel = true
				};
				await warning.ShowDialog(MainWindow.Instance);
                open = warning.DialogResult == DialogResult.OK;
			}

            if (open)
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
        }

        private void ToggleActivePerson_Click(object sender, RoutedEventArgs e)
        {
            PersonViewModel person = (PersonViewModel)((Button)e.Source).DataContext;
            person.Active = !person.Active;
			ViewModel.UpdatePeopleTable();
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

        private async void EditPerson_Click(object sender, RoutedEventArgs e)
        {
            PersonViewModel person = (PersonViewModel)((Button)e.Source).DataContext;
            if (person != null)
            {
                AddConsentForm dialog = new()
                {
                    ConsentName = person.Name,
                    ConsentDate = person.SortableDate,
                    ConsentVersion = person.Version,
                    ConsentComment = person.Comment,
                    Title = Localization.EditPersonDialog,
                    ButtonName = Localization.AddConsentForm_EditButton
                };

                await dialog.ShowDialog(MainWindow.Instance);
                if (dialog.DialogResult == DialogResult.OK)
                {
                    person.Name = dialog.ConsentName;
                    person.SortableDate = dialog.ConsentDate;
                    person.Version = dialog.ConsentVersion;
                    person.Comment = dialog.ConsentComment;
                }
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

		/// <summary>
		/// Marks the selected person for deletion.
		/// </summary>
		private async void EditPerson_ContextMenu_Click(object sender, RoutedEventArgs e)
		{
			PersonViewModel person = ViewModel.Selected;
			if (sender != null)
			{
				try
				{
					if (person != null)
					{
						AddConsentForm dialog = new()
						{
							ConsentName = person.Name,
							ConsentDate = person.SortableDate,
							ConsentVersion = person.Version,
							ConsentComment = person.Comment,
							Title = Localization.EditPersonDialog,
							ButtonName = Localization.AddConsentForm_EditButton
						};

						await dialog.ShowDialog(MainWindow.Instance);
						if (dialog.DialogResult == DialogResult.OK)
						{
							person.Name = dialog.ConsentName;
							person.SortableDate = dialog.ConsentDate;
							person.Version = dialog.ConsentVersion;
							person.Comment = dialog.ConsentComment;
						}
					}
				}
				catch (Exception ex)
				{
					Program.Error(Localization.Exception_Name, ex, MainWindow.Instance);
				}
			}
		}

		/// <summary>
		/// Marks the selected person for deletion.
		/// </summary>
		private async void RemovePerson_ContextMenu_Click(object sender, RoutedEventArgs e)
		{
			PersonViewModel person = ViewModel.Selected;
			if (sender != null)
			{
				try
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
				catch (Exception ex)
				{
					Program.Error(Localization.Exception_Name, ex, MainWindow.Instance);
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