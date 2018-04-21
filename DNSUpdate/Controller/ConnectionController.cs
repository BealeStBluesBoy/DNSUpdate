using System.Net;

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

        public string GetExternalIP()
        {
            try
            {
                var ip = Cliente.DownloadString("http://checkip.amazonaws.com/");
                if (ip is string)
                {
                    return ip;
                }
            }
            catch { }
            return "No connection!";
        }
    }
}
