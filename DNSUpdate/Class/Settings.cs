namespace DNSUpdate.Class
{
    class Settings
    {
        public string Domain { get; }
        public string Token { get; }
        public byte Interval { get; }

        public Settings(string domain, string token, byte interval)
        {
            Domain = domain;
            Token = token;
            Interval = interval;
        }
    }
}
