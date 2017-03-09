using BazaSmyczy.Core.Consts;
using Microsoft.AspNetCore.Hosting;
using NLog.Web;

namespace BazaSmyczy.Extensions
{
    public static class IHostingEnvironmentExtensions
    {
        //TODO: Find better name
        public static void ConfigureNLog(this IHostingEnvironment env)
        {
            if (env.IsProduction())
            {
                env.ConfigureNLog($"{AppDir.ConfigDir}/nlog.Production.config");
            }
            else
            {
                env.ConfigureNLog($"{AppDir.ConfigDir}/nlog.config");
            }
        }
    }
}
