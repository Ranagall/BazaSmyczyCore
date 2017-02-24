using BazaSmyczy.Core.Services;
using BazaSmyczy.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BazaSmyczy.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddDbContexts(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<LeashDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<TokenDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        }

        public static void AddInterfaces(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddTransient<IImageUtils, ImageUtils>();

            services.AddTransient<IUploadManager, UploadManager>();
        }
    }
}
