using BazaSmyczy.Core.Consts;
using BazaSmyczy.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using System;

namespace BazaSmyczy
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                //.AddJsonFile($"{AppDir.ConfigDir}/appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"{AppDir.ConfigDir}/appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange:true)
                .AddJsonFile($"{AppDir.ConfigDir}/hosting.json", optional: true);

            env.ConfigureNLog();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContexts(Configuration);
            services.AddOptions();
            services.ConfigureAppOptions(Configuration);

            services.ConfigureIdentity();

            services.AddMvc();

            services.AddInterfaces(Configuration);
        }

        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();
            app.AddNLogWeb();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            serviceProvider.UpdateDb();

            app.UseStaticFiles();

            app.UseIdentity();

            app.UseRegistrationEndpoint(Configuration);
            app.UseMvcWithDefaultRoute();

            await serviceProvider.CreateRoles();
            await serviceProvider.CreateAdminUser(Configuration);
        }
    }
}
