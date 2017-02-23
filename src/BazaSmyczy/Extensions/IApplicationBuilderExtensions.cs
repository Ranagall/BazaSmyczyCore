using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace BazaSmyczy.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static void UseRegistrationEndpoint(this IApplicationBuilder app, IConfigurationRoot configuration)
        {
            if (!configuration.GetValue<bool>("BazaSmyczyOptions:Endpoints:EnableRegisterEndpoint"))
            {
                app.UseMvc(route =>
                {
                    route.MapRoute(
                        name: "BlockRegistration",
                        template: "Account/Register/{id?}",
                        defaults: new { Controller = "Account", Action = "Login" });
                });
            }
        }
    }
}
