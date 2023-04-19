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
		/// <summary>
		/// This is the context of this tab.
		/// </summary>
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

		/// <summary>
		/// Constructor for the tab.
		/// </summary>
        public PeoplePage()
        {
            AvaloniaXamlLoader.Load(this);
            Title = Localization.People_Page;
            LoadingTitle = Localization.InsertKey_LoadPeople;
            Icon = "avares://Timotheus/Resources/People.png";
            ViewModel = new PeopleViewModel();
        }

		/// <summary>
		/// Load the tab according to the key.
		/// </summary>
		public override void Load()
		{
			string path = Project.Keys.Retrieve("Person-File").Replace('\\', '/');
			if (path != string.Empty)
			{
				try
				{
					ViewModel = new(Project.DirectoryPath + path);
				}
				catch (DirectoryNotFoundException)
				{
					Program.Error(Localization.Exception_Name, new Exception(Localization.Exception_CantFindPeople.Replace("#1", path)), MainWindow.Instance);
					ViewModel = new();
				}
				catch (FileNotFoundException)
				{
					Program.Error(Localization.Exception_Name, new Exception(Localization.Exception_CantFindPeople.Replace("#1", path)), MainWindow.Instance);
					ViewModel = new();
				}
				catch (Exception exception)
				{
					Program.Error(Localization.Exception_Name, exception, MainWindow.Instance);
					ViewModel = new();
				}
			}
			else
				ViewModel = new();
		}

		/// <summary>
		/// Add a person to the list.
		/// </summary>
		private async void AddPerson_Click(object sender, RoutedEventArgs e)
        {
			try
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
			catch (Exception exception)
			{
				Program.Error(Localization.Exception_Name, exception, MainWindow.Instance);
			}
        }

        /// <summary>
        /// Saves the current file. If no file is in use, a SaveFileDialog is opened.
        /// </summary>
        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
				if (Project.FilePath == string.Empty)
					throw new Exception(Localization.Exception_MustSaveKeyFirst);

				if (!File.Exists(ViewModel.Path))
                    SaveAs_Click(null, e);
                else
                {
                    ViewModel.Save(ViewModel.Path);
					MessageDialog confirmation = new()
					{
						DialogTitle = Localization.Confirmation,
						DialogText = Localization.Confirmation_SaveConsentForms,
                        DialogShowCancel = false
					};
					await confirmation.ShowDialog(MainWindow.Instance);
                }
            }
            catch (Exception exception)
            {
                Program.Error(Localization.Exception_Saving, exception, MainWindow.Instance);
            }
        }

        /// <summary>
        /// Opens a SaveFileDialog to save the current list of people.
        /// </summary>
        private async void SaveAs_Click(object sender, RoutedEventArgs e)
        {
			try
			{
				if (Project.FilePath == string.Empty)
					throw new Exception(Localization.Exception_MustSaveKeyFirst);

				SaveFileDialog saveFileDialog = new();
				FileDialogFilter filter = new();
				filter.Extensions.Add("csv");
				filter.Name = "CSV (.csv)";

				saveFileDialog.Filters = new()
				{
					filter
				};

				string result = await saveFileDialog.ShowAsync(MainWindow.Instance);
				if (result != null)
				{
					string path = Project.GetProjectPath(result);
					Project.Keys.Update("Person-File", path.Replace('\\', '/'));

					ViewModel.Save(result);

					MessageDialog confirmation = new()
					{
						DialogTitle = Localization.Confirmation,
						DialogText = Localization.Confirmation_SaveConsentForms,
						DialogShowCancel = false
					};
					await confirmation.ShowDialog(MainWindow.Instance);
				}
			}
			catch (Exception exception)
			{
				Program.Error(Localization.Exception_Saving, exception, MainWindow.Instance);
			}
        }

		/// <summary>
		/// Opens a OpenFileDialog, where the user can select a file containing the list of people.
		/// </summary>
        private async void Open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
				if (Project.FilePath == string.Empty)
					throw new Exception(Localization.Exception_MustSaveKeyFirst);

				bool open = true;

				if (ViewModel.People.Count > 0)
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
						string path = Project.GetProjectPath(result[0]);
						Project.Keys.Update("Person-File", path.Replace('\\', '/'));
                        Load();
					}
				}
			}
			catch (Exception exception)
			{
				Program.Error(Localization.Exception_Opening, exception, MainWindow.Instance);
			}
        }

		/// <summary>
		/// Toggles whether a person is active.
		/// </summary>
        private void ToggleActivePerson_Click(object sender, RoutedEventArgs e)
        {
			try
			{
				PersonViewModel person = (PersonViewModel)((Button)e.Source).DataContext;
				person.Active = !person.Active;
				ViewModel.UpdatePeopleTable();
			}
			catch (Exception exception)
			{
				Program.Error(Localization.Exception_Name, exception, MainWindow.Instance);
			}
		}

		/// <summary>
		/// Toggles whether a person is active.
		/// </summary>
		private void ToggleInactive_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.ShowInactive = !ViewModel.ShowInactive;
			ViewModel.UpdatePeopleTable();
		}

		/// <summary>
		/// Opens a dialog where the user can edit a given person.
		/// </summary>
		private async void EditPerson_Click(object sender, RoutedEventArgs e)
        {
			try
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
			catch (Exception exception)
			{
				Program.Error(Localization.Exception_Name, exception, MainWindow.Instance);
			}
        }

		/// <summary>
		/// Opens a dialog where the user can edit a given person.
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
		private async void RemovePerson_Click(object sender, RoutedEventArgs e)
        {
            PersonViewModel person = (PersonViewModel)((Button)e.Source).DataContext;
            if (person != null)
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
				catch (Exception exception)
				{
					Program.Error(Localization.Exception_Name, exception, MainWindow.Instance);
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

		/// <summary>
		/// Handles when the user is writing in the search field.
		/// </summary>
        private void SearchPeople(object sender, KeyEventArgs e)
        {
            ViewModel.UpdatePeopleTable();
        }

		/// <summary>
		/// Handles the loading of the datagrid rows.
		/// </summary>
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

		/// <summary>
		/// Whether the list of people has been changed.
		/// </summary>
		public override string HasBeenChanged()
        {
            return ViewModel.HasBeenChanged ? Title : string.Empty;
        }
    }
}