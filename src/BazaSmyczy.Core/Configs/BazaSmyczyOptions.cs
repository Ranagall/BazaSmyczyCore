namespace BazaSmyczy.Core.Configs
{
    public class BazaSmyczyOptions
    {
        public EndpointsOptions Endpoints { get; set; } = new EndpointsOptions();

        public string UploadsPath { get; set; }
    }
}
