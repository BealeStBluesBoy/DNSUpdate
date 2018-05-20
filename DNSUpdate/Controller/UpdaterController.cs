using DNSUpdate.Class;
using System;
using System.Windows.Threading;

namespace DNSUpdate.Controller
{
    public static class UpdaterController
    {
        private static DispatcherTimer Timer;

        public static bool StartUpdater()
        {
            Settings settings = SettingsController.GetSettings();
            if (settings.Domain != "" && settings.Token != "" && settings.Interval != 0)
            {
                Timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMinutes(settings.Interval)
                };
                Timer.Tick += Timer_Tick;
                Timer.Start();
                LoggerController.LogEvent("Updater started");
                if (!ConnectionController.Update(settings.Domain, settings.Token))
                    LoggerController.LogEvent("No connection or invalid input data when trying to update");
                return true;
            }
            else
            {
                LoggerController.LogEvent("Can't start updater, invalid input");
                return false;
            }
        }

        public static bool StopUpdater()
        {
            if (Timer != null)
            {
                LoggerController.LogEvent("Updater stopped");
                Timer.Stop();
            }
            return true;
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            Settings settings = SettingsController.GetSettings();
            if (!ConnectionController.Update(settings.Domain, settings.Token))
                LoggerController.LogEvent("No connection when trying to update");
            else
                LoggerController.LogEvent("Automatic update");
        }

        public static bool IsRunning()
        {
            return Timer.IsEnabled;
        }

        public static bool UpdateNow(string domain, string token)
        {
            if (domain != "" && token != "" && ConnectionController.Update(domain, token))
            {
                LoggerController.LogEvent("Manual update");
                return true;
            }
            else
            {
                LoggerController.LogEvent("Manual update failed");
                return false;
            }
        }
    }
}
