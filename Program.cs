using System;
using System.Windows.Forms;
using Timotheus.Forms;

namespace Timotheus
{
    /// <summary>
    /// Root class of the program which holds the main method.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// Starting point of the program, and loads the main window.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }
}