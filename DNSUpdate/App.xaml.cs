using DNSUpdate.Controller;
using System.Windows;

namespace DNSUpdate
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            LoggerController.SetupLog();
            if (SettingsController.CreateSettings())
                LoggerController.LogEvent("Settings DB setup ok");
        }
    }
}
