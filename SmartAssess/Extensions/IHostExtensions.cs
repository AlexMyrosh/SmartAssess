using Microsoft.AspNetCore.Identity;
using Data_Access_Layer.Roles;

namespace Presentation_Layer.Extensions
{
    public static class IHostExtensions
    {
        public static async Task InitializeWebApplicationAsync(this IHost host)
        {
            using var serviceScope = host.Services.CreateScope();
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(RoleNames.Student))
            {
                await roleManager.CreateAsync(new IdentityRole(RoleNames.Student));
            }

            if (!await roleManager.RoleExistsAsync(RoleNames.Teacher))
            {
                await roleManager.CreateAsync(new IdentityRole(RoleNames.Teacher));
            }

            if (!await roleManager.RoleExistsAsync(RoleNames.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(RoleNames.Admin));
            }
        }
    }
}