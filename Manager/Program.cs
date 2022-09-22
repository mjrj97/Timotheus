using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Timotheus.Views.Dialogs;
using Avalonia.Controls.ApplicationLifetimes;
using Timotheus.Utility;

namespace Timotheus
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                File.AppendAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/args.txt", $"{DateTime.Now}: {args[0]}\n");
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Source: https://stackoverflow.com/questions/229565/what-is-a-good-pattern-for-using-a-global-mutex-in-c/229567
                MutexAccessRule allowEveryoneRule = new(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
                MutexSecurity securitySettings = new();
                securitySettings.AddAccessRule(allowEveryoneRule);

                // edited by MasonGZhwiti to prevent race condition on security settings via VanNguyen
                Mutex mutex = new(false, "Timotheus");
                mutex.SetAccessControl(securitySettings);
                using (mutex)
                {
                    // edited by acidzombie24
                    bool hasHandle = false;
                    try
                    {
                        try
                        {
                            // note, you may want to time out here instead of waiting forever
                            // edited by acidzombie24
                            // mutex.WaitOne(Timeout.Infinite, false);
                            hasHandle = mutex.WaitOne(10, false);
                            if (hasHandle == false)
                            {
                                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp.tnote");
                                if (File.Exists(path))
                                    File.Delete(path);
                                string lines = string.Empty;
                                for (int i = 0; i < args.Length; i++)
                                {
                                    lines += args[i];
                                    if (i != args.Length - 1)
                                        lines += "\n";
                                }
                                File.WriteAllText(path, lines);
                                ShowExistingWindow();
                                throw new TimeoutException("Timeout waiting for exclusive access");
                            }
                        }
                        catch (AbandonedMutexException)
                        {
                            // Log the fact that the mutex was abandoned in another process,
                            // it will still get acquired
                            hasHandle = true;
                        }

                        Run(args);
                    }
                    finally
                    {
                        // edited by acidzombie24, added if statement
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
        public static void Run(string[] args)
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
            //builder.StartWithClassicDesktopLifetime(args);
#if !DEBUG
            }
            catch (Exception e)
            {
                Program.Log(e);
            }
#endif
        }

        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int SW_SHOWNORMAL = 1;

        // Shows the window of the single-instance that is already open
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

        // This method is needed for IDE previewer infrastructure
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>().UsePlatformDetect();

        /// <summary>
        /// Display an error dialog
        /// </summary>
        /// <param name="title">Title of the dialog</param>
        /// <param name="exception">Error that occured and which should be shown</param>
        /// <param name="window">Parent window of the error</param>
        public async static void Error(string title, Exception exception, Window window)
        {
            Log(exception);

            ErrorDialog msDialog = new()
            {
                DialogTitle = title,
                DialogText = exception.Message
            };
            await msDialog.ShowDialog(window);
        }

        /// <summary>
        /// Adds text to the current log.
        /// </summary>
        public static void Log(string text)
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
        public static void Log(Exception e)
        {
            Log(e.ToString());
            if (e.InnerException != null)
                Log(e.InnerException);
        }
    }
}