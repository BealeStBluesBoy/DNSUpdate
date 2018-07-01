using System;
using System.IO;

namespace DNSUpdate.Controller
{
    static class LoggerController
    {
        static readonly string LogFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DNSUpdate\\DNSUpdate.log");

        public static void SetupLog()
        {
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\DNSUpdate");
            File.WriteAllText(LogFile, "");
        }

        public static void LogEvent(string logEvent)
        {
            File.AppendAllText(LogFile, string.Format("[{0}] {1}{2}", DateTime.Now.ToString(), logEvent, Environment.NewLine));
        }
    }
}
