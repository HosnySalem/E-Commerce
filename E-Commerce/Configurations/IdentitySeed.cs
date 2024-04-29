using E_Commerce.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace E_Commerce.Configurations
{
    public class IdentitySeed
    {
        public static async ValueTask SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (userManager is null)
            {
                throw new ArgumentNullException(nameof(userManager));
            }
            if (await userManager.FindByNameAsync("admin@example.com") == null)
            {
                var adminUser = new AppUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                };

                var result = await userManager.CreateAsync(adminUser, "P@ssw0rd");

                if (result.Succeeded)
                {
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                    }
                }
            }
        }
    }
}
