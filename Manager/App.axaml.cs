using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Timotheus.Utility;
using Timotheus.Views;
using MonoMac.AppKit;
using MonoMac.Foundation;

namespace Timotheus
{
    public class App : Application
    {
        private static MainWindow window;
        private readonly BackgroundWorker ArgListener = new();
        private bool FirstTime = true;

        public override void Initialize()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ArgListener.DoWork += ListenForArgs;
                ArgListener.RunWorkerAsync();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                UrlsOpened += ListenForArgs;

                NSApplication.Init();
                NSApplication.Notifications.ObserveDidBecomeActive(OpenMainWindow);
            }

            AvaloniaXamlLoader.Load(this);
            DataContext = this;
        }

        public void OpenMainWindow(object sender, NSNotificationEventArgs e)
        {
            Dispatcher.UIThread.InvokeAsync(delegate
            {
                if (!FirstTime)
                    window.Show();
                else
                    FirstTime = false;
            });
        }

        /// <summary>
        /// Called at the end of Avalonia initialization.
        /// </summary>
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
        /// Method where program is listening for new instances of Timotheus and their args. (Windows ONLY)
        /// </summary>
        private void ListenForArgs(object sender, DoWorkEventArgs e)
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint localEndPoint = new(ipAddr, 17045);

            Socket listener = new(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {   
                listener.Bind(localEndPoint);
                listener.Listen(10);

                while (!ArgListener.CancellationPending)
                {
                    Socket clientSocket = listener.Accept();

                    if (((IPEndPoint)clientSocket.RemoteEndPoint).Address.ToString() == ((IPEndPoint)clientSocket.LocalEndPoint).Address.ToString())
                    {
                        byte[] bytes = new byte[1024];
                        string data = null;

                        while (true)
                        {
                            int numByte = clientSocket.Receive(bytes);
                            data += Encoding.ASCII.GetString(bytes, 0, numByte);
                            if (data.IndexOf("<EOF>") > -1)
                                break;
                        }

                        string[] args = data.Split('\n');
                        Dispatcher.UIThread.InvokeAsync(delegate
                        {
                            window.Start(args);
                        });
                    }

                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Method where program is listening for new instances of Timotheus and their args. (macOS ONLY)
        /// </summary>
        private void ListenForArgs(object sender, UrlOpenedEventArgs e)
        {
            string[] args = new string[e.Urls.Length];
            for (int i = 0; i < e.Urls.Length; i++)
            {
                if (e.Urls[i].StartsWith("file://"))
                    args[i] = e.Urls[i][7..];
                else
                    args[i] = e.Urls[i];
            }

            Dispatcher.UIThread.InvokeAsync(delegate
            {
                window.Start(args);
            });
        }

        /// <summary>
        /// Called by the TrayIcon 'Open' button
        /// </summary>
        void Open_TrayClick(object sender, EventArgs ags)
        {
            window.WindowState = Avalonia.Controls.WindowState.Normal; 
            window.Show();
        }

        /// <summary>
        /// Called by the TrayIcon 'Open' button
        /// </summary>
        void Close_TrayClick(object sender, EventArgs ags)
        {
            window.CloseTray();
        }
    }
}