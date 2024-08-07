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

        Task<bool> ResetPasswordEmailAsync(string email, string callbackUrl);

        Task<string> GenerateResetTokenAsync(string email);

        Task<IdentityResult> ResetPasswordAsync(string email, string code, string newPassword);

        Task<bool> IsUserExistByUsernameAsync(string username);

        Task<bool> IsUserExistByEmailAsync(string email);

        Task<string> GenerateEmailConfirmationTokenAsync(UserEntity user);

        Task<IdentityResult> ConfirmEmailAsync(string userId, string code);

        Task SendConfirmationEmailAsync(string email, string callbackUrl);

        Task<IdentityResult> ChangePasswordAsync(UserEntity user, string currentPassword, string newPassword);
    }
}
