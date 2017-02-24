using BazaSmyczy.Core.Config;
using BazaSmyczy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace BazaSmyczy.Extensions
{
    public static class IServiceProviderExtensions
    {
        public static async Task CreateRoles(this IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Admin", "Member" };

            foreach (var role in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        public static async Task CreateAdminUser(this IServiceProvider serviceProvider, IConfigurationRoot configuration)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var adminConfig = configuration.GetSection("BazaSmyczyOptions:AdminAccount").Get<AdminAccountConfig>();
            var user = new ApplicationUser { UserName = adminConfig.AdminUsername, Email = adminConfig.AdminEmail };

            if (await userManager.FindByNameAsync(adminConfig.AdminUsername) == null)
            {
                var result = await userManager.CreateAsync(user, adminConfig.AdminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
