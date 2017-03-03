using BazaSmyczy.Core.Configs;
using BazaSmyczy.Core.Consts;
using BazaSmyczy.Core.Services;
using BazaSmyczy.Data;
using BazaSmyczy.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

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
        }

        public static void AddInterfaces(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddTransient<IImageUtils, ImageUtils>();

            services.AddTransient<IUploadManager, UploadManager>();

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<INotificationComposer, NotificationComposer>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static void ConfigureAppOptions(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.Configure<BazaSmyczyOptions>(configuration.GetSection("BazaSmyczyOptions"));
            services.Configure<EmailClientConfig>(configuration.GetSection("EmailClient"));
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var lockoutOptions = new LockoutOptions()
            {
                DefaultLockoutTimeSpan = TimeSpan.FromMinutes(IdentityConsts.LockoutDuration),
                MaxFailedAccessAttempts = IdentityConsts.MaxFailedAccessAttempts
            };

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.Lockout = lockoutOptions;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }
    }
}
