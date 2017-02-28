namespace BazaSmyczy.Core.Configs
{
    public class EmailClientConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public bool UseSsl { get; set; }
    }
}
