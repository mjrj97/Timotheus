using Avalonia;
using Avalonia.Layout;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Media.Imaging;
using System;
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
            Timotheus.Registry.Update("KeyPath", path);

            AddTab<CalendarPage>(Keys);
            AddTab<FilesPage>(Keys);
            AddTab<PeoplePage>(Keys);
        }
        public Project()
        {
            Loader.DoWork += Load;
            Loader.WorkerReportsProgress = true;

            Keys = new Register(':');

            AddTab<CalendarPage>(Keys);
            AddTab<FilesPage>(Keys);
            AddTab<PeoplePage>(Keys);
        }

        /// <summary>
        /// Adds a tab to the project with the specified tab type.
        /// </summary>
        /// <typeparam name="T">Tab type</typeparam>
        /// <param name="Keys">Keys to be used by the tab.</param>
        private void AddTab<T>(Register Keys) where T : Tab, new()
        {
            TabItem tab = new();
            T page = new()
            {
                Keys = Keys
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

            TabItems.Add(tab);
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
        public void SaveKey(string path, string password)
        {
            Keys.Save(path, password);
        }

        /// <summary>
        /// Returns whether the user has made progress that hasn't been saved.
        /// </summary>
        public bool IsThereUnsavedProgress()
        {
            bool isThereUnsavedProgress = false;

            for (int i = 0; i < Tabs.Count; i++)
            {
                isThereUnsavedProgress |= Tabs[i].HasBeenChanged();
            }

            return isThereUnsavedProgress;
        }
    }
}