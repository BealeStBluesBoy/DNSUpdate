using System;
using System.Net;
using System.Threading.Tasks;

namespace DNSUpdate.Controller
{
    static class ConnectionController
    {
        static WebClient Cliente = new WebClient();

        public async static Task<bool> Update(string domain, string token)
        {
            try
            {
                string response = await Cliente.DownloadStringTaskAsync("https://www.duckdns.org/update?domains=" + domain + "&token=" + token + "&ip=");
                if (response == "OK")
                    return true;
            }
            catch { }
            return false;
        }

        public static async Task<string> GetExternalIP()
        {
            try
            {
                string ip = await Cliente.DownloadStringTaskAsync("https://checkip.amazonaws.com/");
                return ip.Replace("\n", String.Empty);
            }
            catch { }
            return "No Connection!";
        }
    }
}
