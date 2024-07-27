using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Business_Logic_Layer.Models;
using Data_Access_Layer.Models;

namespace Business_Logic_Layer.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IdentityResult> CreateAsync(UserEntity user, string password);

        Task<UserEntity?> GetUserAsync(ClaimsPrincipal userPrincipal);

        Task<IdentityResult?> UpdateAsync(UserModel user);
    }
}
