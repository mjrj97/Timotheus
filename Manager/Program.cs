using Avalonia;
using Avalonia.Controls;
using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;
using System.Security.Principal;
using System.Security.AccessControl;
using System.Runtime.InteropServices;
using Timotheus.Views.Dialogs;
using Timotheus.Utility;

namespace Timotheus
{
    internal sealed class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        internal static void Main(string[] args)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Source: https://stackoverflow.com/questions/229565/what-is-a-good-pattern-for-using-a-global-mutex-in-c/229567
                MutexAccessRule allowEveryoneRule = new(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
                MutexSecurity securitySettings = new();
                securitySettings.AddAccessRule(allowEveryoneRule);

                Mutex mutex = new(false, "Timotheus");
                mutex.SetAccessControl(securitySettings);
                using (mutex)
                {
                    bool hasHandle = false;
                    try
                    {
                        try
                        {
                            hasHandle = mutex.WaitOne(10, false);
                            if (hasHandle == false)
                            {
                                SendArgsToInstance(args);
                                ShowExistingWindow();
                                throw new TimeoutException("Timeout waiting for exclusive access");
                            }
                        }
                        catch (AbandonedMutexException)
                        {
                            hasHandle = true;
                        }

                        Run(args);
                    }
                    finally
                    {
                        if (hasHandle)
                            mutex.ReleaseMutex();
                    }
                }
            }
            else
                Run(args);
        }

        /// <summary>
        /// Creates the application
        /// </summary>
        internal static void Run(string[] args)
        {
#if !DEBUG
            try
            {
#endif
            Timotheus.Initalize();
            AppBuilder builder = BuildAvaloniaApp();
            
            DesktopLifetime lifetime = new()
            {
                Args = args,
                ShutdownMode = ShutdownMode.OnLastWindowClose
            };
            builder.SetupWithLifetime(lifetime);
            lifetime.Start(args);
#if !DEBUG
            }
            catch (Exception e)
            {
                Program.Log(e);
            }
#endif
        }

        #region Windows
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int SW_SHOWNORMAL = 1;

        /// <summary>
        /// Shows the window of the single-instance that is already open
        /// </summary>
        private static void ShowExistingWindow()
        {
            var currentProcess = Process.GetCurrentProcess();
            var processes = Process.GetProcessesByName(currentProcess.ProcessName);
            foreach (var process in processes)
            {
                // the single-instance already open should have a MainWindowHandle
                if (process.MainWindowHandle != IntPtr.Zero)
                {
                    // restores the window in case it was minimized
                    ShowWindow(process.MainWindowHandle, SW_SHOWNORMAL);

                    // brings the window to the foreground
                    SetForegroundWindow(process.MainWindowHandle);

                    return;
                }
            }
        }

        /// <summary>
        /// Sends arguments to the singleton instance of the application.
        /// </summary>
        private static void SendArgsToInstance(string[] args)
        {
            try
            {
                IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddr = ipHost.AddressList[0];
                IPEndPoint localEndPoint = new(ipAddr, 17045);

                Socket sender = new(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    sender.Connect(localEndPoint);

                    string message = string.Empty;
                    for (int i = 0; i < args.Length; i++)
                    {
                        message += args[i] + "\n";
                    }
                    byte[] messageSent = Encoding.ASCII.GetBytes(message + "<EOF>");
                    int byteSent = sender.Send(messageSent);

                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
                catch (ArgumentNullException ane)
                {
                    Log(ane);
                }
                catch (SocketException se)
                {
                    Log(se);
                }
                catch (Exception e)
                {
                    Log(e);
                }
            }
            catch (Exception e)
            {
                Log(e);
            }
        }
        #endregion

        // This method is needed for IDE previewer infrastructure
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>().UsePlatformDetect();

        /// <summary>
        /// Display an error dialog
        /// </summary>
        /// <param name="title">Title of the dialog</param>
        /// <param name="exception">Error that occured and which should be shown</param>
        /// <param name="window">Parent window of the error</param>
        internal async static void Error(string title, Exception exception, Window window)
        {
            Log(exception);

            ErrorDialog msDialog = new()
            {
                DialogTitle = title,
                DialogText = exception.Message
            };
            if (!window.IsVisible)
                window.Show();

            await msDialog.ShowDialog(window);
        }

        /// <summary>
        /// Adds text to the current log.
        /// </summary>
        internal static void Log(string text)
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Timotheus/";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                File.AppendAllText(path + DateTime.Now.ToString("d") + ".txt", "[" + DateTime.Now + "]: " + text + "\n");
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Adds exception to the current log.
        /// </summary>
        internal static void Log(Exception e)
        {
            Log(e.ToString());
            if (e.InnerException != null)
                Log(e.InnerException);
        }
    }
}