using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using System;
using System.IO;
using System.Runtime.InteropServices;
using Timotheus.Utility;
using Timotheus.Views;

namespace Timotheus
{
    public class App : Application
    {
        private static MainWindow window;
        private FileSystemWatcher watcher;

        public override void Initialize()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                watcher = new(AppDomain.CurrentDomain.BaseDirectory)
                {
                    NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size
                };
                watcher.Changed += OnFileChanged;
                watcher.Filter = "*.tnote";
                watcher.IncludeSubdirectories = false;
                watcher.EnableRaisingEvents = true;
            }

            AvaloniaXamlLoader.Load(this);
            DataContext = this;
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is DesktopLifetime desktop)
            {
                window = new MainWindow();
                desktop.MainWindow = window;
            }

            base.OnFrameworkInitializationCompleted();
        }

        /// <summary>
        /// Event that is called when a .tnote file is created in the directory.
        /// </summary>
        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            Dispatcher.UIThread.InvokeAsync(delegate
            {
                string[] args = File.ReadAllLines(e.FullPath);
                window.Show(args);
            });
        }

        /// <summary>
        /// Called by the TrayIcon 'Open' button
        /// </summary>
        void Open_TrayClick(object sender, EventArgs ags)
        {
            window.Show();
        }

        /// <summary>
        /// Called by the TrayIcon 'Open' button
        /// </summary>
        void Close_TrayClick(object sender, EventArgs ags)
        {
            window.Close();
        }
    }
}