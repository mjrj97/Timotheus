using System;
using Avalonia;

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
                System.IO.File.AppendAllText("log.txt", "[" + DateTime.Now + "]: " +  e.Message);
            }
#endif
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect();
    }
}