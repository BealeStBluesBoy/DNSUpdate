using System;
using System.IO;
using System.Threading.Tasks;

namespace DNSUpdate.Controller
{
    static class LoggerController
    {
        static string LogFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DNSUpdate\\DNSUpdate.log");

        public async static void SetupLog()
        {
            await Task.Run(() =>
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\DNSUpdate");
                File.WriteAllText(LogFile, "");
            });
        }

        public async static void LogEvent(string logEvent)
        {
            await Task.Run(() => {
                File.AppendAllText(LogFile, string.Format("[{0}] {1}{2}", DateTime.Now.ToString(), logEvent, Environment.NewLine));
            });
        }
    }
}
