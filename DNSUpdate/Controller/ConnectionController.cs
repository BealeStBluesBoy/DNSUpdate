using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DNSUpdate.Controller
{
    static class ConnectionController
    {
        public static bool Update(string domain, string token)
        {
            WebClient Cliente = new WebClient();
            try
            {
                var Response = Cliente.DownloadString("https://www.duckdns.org/update?domains=" + domain + "&token=" + token + "&ip=");
                if (Response is string)
                {
                    return Response == "OK";
                }
            }
            catch { }
            return false;
        }

        public static async Task<string> GetExternalIP()
        {
            HttpClient Cliente = new HttpClient();
            try
            {
                string ip = await Cliente.GetStringAsync(new Uri("https://checkip.amazonaws.com/"));
                return ip.Replace("\n", String.Empty);
            }
            catch { }
            return "No Connection!";
        }
    }
}
