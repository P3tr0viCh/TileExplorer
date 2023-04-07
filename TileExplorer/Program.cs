using Bluegrams.Application;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace TileExplorer
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            PortableSettingsProvider.SettingsFileName = "portable.config";
            PortableSettingsProviderBase.SettingsDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                Path.GetFileNameWithoutExtension(Application.ExecutablePath));
            Debug.WriteLine(PortableSettingsProviderBase.SettingsDirectory);
            Directory.CreateDirectory(PortableSettingsProviderBase.SettingsDirectory);
            PortableSettingsProvider.ApplyProvider(Properties.Settings.Default);
            
            Application.Run(new Main());
        }
    }
}