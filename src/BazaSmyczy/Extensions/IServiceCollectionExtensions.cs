﻿using BazaSmyczy.Core.Configs;
using BazaSmyczy.Core.Services;
using BazaSmyczy.Data;
using Microsoft.AspNetCore.Http;
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
    }
}
