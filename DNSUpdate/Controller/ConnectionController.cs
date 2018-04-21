using System;
using System.Net;
using System.Threading.Tasks;

namespace DNSUpdate
{
    class ConnectionController
    {
        WebClient Cliente = new WebClient();

        public bool Update(string domain, string token)
        {
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

        public async Task<string> GetExternalIP()
        {
            try
            {
                var ip = Task.Run(() => Cliente.DownloadString(new Uri("http://checkip.amazonaws.com/")));
                return await ip;
            }
            catch { }
            return "No connection!";
        }
    }
}
