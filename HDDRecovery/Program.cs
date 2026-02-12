using System;
using System.Windows.Forms;

namespace HDDRecovery
{
    /// <summary>
    /// Main program class containing the application entry point.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}
