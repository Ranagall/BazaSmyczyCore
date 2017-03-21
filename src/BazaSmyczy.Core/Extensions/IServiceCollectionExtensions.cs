using BazaSmyczy.Core.Configs;
using BazaSmyczy.Core.Consts;
using BazaSmyczy.Core.Models;
using BazaSmyczy.Core.Services;
using BazaSmyczy.Core.Stores;
using BazaSmyczy.Core.Stores.Data;
using BazaSmyczy.Core.Stores.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BazaSmyczy.Core.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddDbContexts(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<LeashDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        }

        public static void ConfigureAppOptions(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.Configure<BazaSmyczyOptions>(configuration.GetSection("BazaSmyczyOptions"));
            services.Configure<EmailClientConfig>(configuration.GetSection("EmailClient"));
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }

        public static void AddInterfaces(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddSingleton(configuration);
            services.AddStores();

            services.AddTransient<ILeashService, LeashService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IManageService, ManageService>();

            services.AddTransient<IImageUtils, ImageUtils>();

            services.AddTransient<IUploadManager, UploadManager>();

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<INotificationComposer, NotificationComposer>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        private static void AddStores(this IServiceCollection services)
        {
            services.AddScoped<ILeashProvider, LeashProvider>();
            services.AddTransient<ILeashesStore, LeashesStore>();
        }
    }
}
