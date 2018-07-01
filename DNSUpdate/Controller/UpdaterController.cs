using DNSUpdate.Class;
using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace DNSUpdate.Controller
{
    public static class UpdaterController
    {
        private static DispatcherTimer Timer = new DispatcherTimer();

        public async static Task<bool> StartUpdater()
        {
            Settings settings = SettingsController.GetSettings();
            if (settings.Domain != "" && settings.Token != "" && settings.Interval != 0)
            {
                Timer.Interval = TimeSpan.FromMinutes(settings.Interval);
                Timer.Tick += Timer_Tick;
                Timer.Start();
                LoggerController.LogEvent("Updater started");
                if (!await ConnectionController.Update(settings.Domain, settings.Token))
                {
                    LoggerController.LogEvent("No connection or invalid input data when trying to update");
                    return false;
                }
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

        private async static void Timer_Tick(object sender, EventArgs e)
        {
            Settings settings = SettingsController.GetSettings();
            if (!await ConnectionController.Update(settings.Domain, settings.Token))
                LoggerController.LogEvent("No connection when trying to update");
            else
                LoggerController.LogEvent("Automatic update");
        }

        public static bool IsRunning()
        {
            return Timer.IsEnabled;
        }

        public async static Task<bool> UpdateNow(string domain, string token)
        {
            if (domain != "" && token != "" && await ConnectionController.Update(domain, token))
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
