using Microsoft.AspNetCore.Identity;
using Data_Access_Layer.Roles;

namespace Presentation_Layer.Roles
{
    public class RolesInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = [RoleNames.Admin, RoleNames.Teacher, RoleNames.Student];

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}