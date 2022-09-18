using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using System;
using System.IO;
using System.Runtime.InteropServices;
using Timotheus.Views;

namespace Timotheus
{
    public class App : Application
    {
        private MainWindow window;
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
                watcher.Created += OnCreated;
                watcher.Filter = "*.tnote";
                watcher.IncludeSubdirectories = false;
                watcher.EnableRaisingEvents = true;
            }

            AvaloniaXamlLoader.Load(this);
            DataContext = this;
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                window = new MainWindow();
                desktop.MainWindow = window;
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            Dispatcher.UIThread.InvokeAsync(delegate
            {
                window.Show();
            });
        }

        void Open_TrayClick(object sender, EventArgs ags)
        {
            window.Show();
        }

        void Close_TrayClick(object sender, EventArgs ags)
        {
            window.Close();
        }
    }
}