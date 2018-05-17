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
                Task<string> ip = Cliente.GetStringAsync(new Uri("http://checkip.amazonaws.com/"));
                return await ip;
            }
            catch { }
            return "No connection!";
        }
    }
}
