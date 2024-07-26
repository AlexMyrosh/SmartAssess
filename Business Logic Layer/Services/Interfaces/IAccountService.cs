using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Business_Logic_Layer.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IdentityResult?> CreateAsync(UserEntity user, string password);

        Task<UserEntity?> GetUserAsync(ClaimsPrincipal userPrincipal);

        Task<IdentityResult?> UpdateAsync(UserEntity user);
    }
}
