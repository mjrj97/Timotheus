using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Timotheus.Views.Dialogs;

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
#if DEBUG
            Timotheus.Initalize();
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
#else
            try
            {
                Timotheus.Initalize();
                BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
            }
            catch (Exception e)
            {
                Timotheus.Log(e);
            }
#endif
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect();

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