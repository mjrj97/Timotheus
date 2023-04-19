using Avalonia;
using Avalonia.Layout;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Media.Imaging;
using System;
using System.IO;
using System.Threading;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Timotheus.IO;
using Timotheus.Views;
using Timotheus.Views.Tabs;

namespace Timotheus.ViewModels
{
    public class Project : ViewModel
    {
        /// <summary>
        /// The title shown in the MainWindow
        /// </summary>
        public string WindowTitle
        {
            get
            {
                string windowTitle = "Timotheus";

                if (FilePath != null && FilePath != string.Empty)
                {
					windowTitle += " (" + FilePath + ")";
				}

				return windowTitle;
			}
        }

        /// <summary>
        /// The path to the current project's file.
        /// </summary>
        private string _filePath = string.Empty;
        public string FilePath 
        { 
            get 
            { 
                return _filePath;
            }
            protected set
            {
                _filePath = value;
                NotifyPropertyChanged(nameof(FilePath));
				NotifyPropertyChanged(nameof(WindowTitle));
			}
        }

        /// <summary>
        /// Directory of the project.
        /// </summary>
        public string DirectoryPath
        {
            get
            {
                return Path.GetDirectoryName(FilePath);
            }
        }

        private string InitialKeys;
        /// <summary>
        /// Register containing all the keys loaded at startup or manually from a key file (.tkey or .txt)
        /// </summary>
        public Register Keys { get; set; }

        private ObservableCollection<TabItem> _tabItems = new();
        /// <summary>
        /// Tabs to be shown in the view
        /// </summary>
        public ObservableCollection<TabItem> TabItems
        {
            get
            {
                return _tabItems;
            }
            set
            {
                _tabItems = value;
                NotifyPropertyChanged(nameof(TabItem));
            }
        }

        /// <summary>
        /// Contents of the tabs
        /// </summary>
        private List<Tab> Tabs { get; set; } = new List<Tab>();

        /// <summary>
        /// Worker that is used to track the progress of the inserting a key.
        /// </summary>
        public BackgroundWorker Loader { get; private set; } = new();

        public Project(string path, string password)
        {
            Loader.DoWork += Load;
            Loader.WorkerReportsProgress = true;

            Keys = new Register(path, password, ':');
			InitialKeys = Keys.ToString();
			Timotheus.Registry.Update("KeyPath", path);

			FilePath = path;

			TabItems.Add(GenerateTab<CalendarPage>());
			TabItems.Add(GenerateTab<FilesPage>());
			TabItems.Add(GenerateTab<PeoplePage>());
		}
        public Project()
        {
            Loader.DoWork += Load;
            Loader.WorkerReportsProgress = true;

            Keys = new Register(':');
            InitialKeys = Keys.ToString();

			TabItems.Add(GenerateTab<CalendarPage>());
			TabItems.Add(GenerateTab<FilesPage>());
			TabItems.Add(GenerateTab<PeoplePage>());
        }

        /// <summary>
        /// Adds a tab to the project with the specified tab type.
        /// </summary>
        /// <typeparam name="T">Tab type</typeparam>
        private TabItem GenerateTab<T>() where T : Tab, new()
        {
            TabItem tab = new();
            T page = new()
            {
                Project = this
            };
            Tabs.Add(page);

            DockPanel panel = new()
            {
                Margin = new Thickness(5)
            };

            IAssetLoader assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

            Image image = new()
            {
                Source = new Bitmap(assets.Open(new Uri(page.Icon))),
                VerticalAlignment = VerticalAlignment.Center,
                Height = 32
            };

            TextBlock text = new()
            {
                Text = page.Title,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 0, 0, 0)
            };

            panel.Children.Add(image);
            panel.Children.Add(text);

            tab.Header = panel;
            tab.Content = page;

            return tab;
		}

        /// <summary>
        /// "Inserts" the current key, and tries to open the Calendar and File sharing system.
        /// </summary>
        private void Load(object sender, DoWorkEventArgs e)
        {
            List<Thread> threads = new();
            for (int i = 0; i < TabItems.Count; i++)
            {
                Thread thread = new(Tabs[i].Load);
                threads.Add(thread);
                thread.Start();
            }

            for (int i = 0; i < threads.Count; i++)
            {
                threads[i].Join();
                if (sender != null && e != null)
                {
                    Loader.ReportProgress((int)(100 * ((float)(1 + i) / threads.Count)), Tabs[i].LoadingTitle);
                    Thread.Sleep(100);
                    if (Loader.CancellationPending == true)
                        return;
                }
            }
        }

        /// <summary>
        /// Save the encrypted key to the path.
        /// </summary>
        public void Save(string path, string password)
        {
            InitialKeys = Keys.ToString(); 
            Keys.Save(path, password);
            FilePath = path;
        }

        /// <summary>
        /// Returns the given path relative to the projects directory.
        /// </summary>
        public string GetProjectPath(string path)
        {
            if (path.StartsWith(DirectoryPath))
            {
                path = path.Substring(DirectoryPath.Length);
            }
            else
            {
				throw new Exception(Localization.Exception_NotInProjectFolder.Replace("#1", DirectoryPath));
			}

			return path;
        }

        /// <summary>
        /// Returns whether this is a good place for a key.
        /// </summary>
        public static bool IsSafeProjectPath(string path)
        {
            string path1 = Path.GetFullPath(path);

            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			string desktopDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
			string commonDocuments = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
			string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

			return path1 != desktop && path1 != desktopDirectory && path1 != commonDocuments && path1 != myDocuments;
        }

        /// <summary>
        /// Returns whether the user has made progress that hasn't been saved.
        /// </summary>
        public List<string> IsThereUnsavedProgress()
        {
            List<string> UnsavedProgress = new();

            if (FilePath != string.Empty && File.Exists(FilePath))
            {
                string dataFromMemory = Keys.ToString();

                if (InitialKeys != dataFromMemory)
                    UnsavedProgress.Add(Localization.ToolStrip_Key);
			}

            for (int i = 0; i < Tabs.Count; i++)
            {
                string message = Tabs[i].HasBeenChanged();
                if (message != string.Empty)
                    UnsavedProgress.Add(message);
            }

            return UnsavedProgress;
        }
    }
}