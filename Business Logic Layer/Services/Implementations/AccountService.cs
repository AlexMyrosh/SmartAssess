using Business_Logic_Layer.Services.Interfaces;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Identity;

namespace Business_Logic_Layer.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<UserEntity> _userManager;

        public AccountService(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> CreateAsync(UserEntity user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Student");
            }

            return result;
        }
    }
}
