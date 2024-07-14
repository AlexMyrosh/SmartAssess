using Microsoft.AspNetCore.Identity;
using Presentation_Layer.RoleHierarchy;

namespace Presentation_Layer.Extentions
{
    public static class IHostExtensions
    {
        public static async Task InitializeWebApplicationAsync(this IHost host)
        {
            using var serviceScope = host.Services.CreateScope();
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(Roles.Student))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Student));
            }

            if (!await roleManager.RoleExistsAsync(Roles.Teacher))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Teacher));
            }

            if (!await roleManager.RoleExistsAsync(Roles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            }
        }
    }
}