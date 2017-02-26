namespace BazaSmyczy.Core.Config
{
    public class BazaSmyczyOptions
    {
        public EndpointsOptions Endpoints { get; set; } = new EndpointsOptions();
        public AdminAccountConfig AdminAccount { get; set; }
        public EmailClientConfig EmailClient { get; set; }

        public string UploadsPath { get; set; }
    }
}
