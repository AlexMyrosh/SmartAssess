using Microsoft.AspNetCore.Identity;

namespace Presentation_Layer.Extensions
{
    public static class IHostExtensions
    {
        public static async Task InitializeWebApplicationAsync(this IHost host)
        {
            using var serviceScope = host.Services.CreateScope();
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(Data_Access_Layer.Roles.Roles.Student))
            {
                await roleManager.CreateAsync(new IdentityRole(Data_Access_Layer.Roles.Roles.Student));
            }

            if (!await roleManager.RoleExistsAsync(Data_Access_Layer.Roles.Roles.Teacher))
            {
                await roleManager.CreateAsync(new IdentityRole(Data_Access_Layer.Roles.Roles.Teacher));
            }

            if (!await roleManager.RoleExistsAsync(Data_Access_Layer.Roles.Roles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(Data_Access_Layer.Roles.Roles.Admin));
            }
        }
    }
}