using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Business_Logic_Layer.Models;

namespace Business_Logic_Layer.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IdentityResult> CreateAsync(UserModel user, string password);

        Task<UserModel?> GetUserAsync(ClaimsPrincipal userPrincipal);

        Task<UserModel?> GetUserAsync(string id);

        Task<UserModel?> GetUserByEmailAsync(string email);

        Task<UserModel?> GetUserWithoutTrackingAsync(string id);

        Task<IdentityResult?> UpdateAsync(UserModel user);

        Task<bool> ResetPasswordEmailAsync(string email, string callbackUrl);

        Task ResetEmailAsync(string email, string callbackUrl);

        Task<string> GenerateResetTokenAsync(string email);

        Task<string> GenerateChangeEmailTokenAsync(string userId, string newEmail);

        Task<IdentityResult> ResetPasswordAsync(string email, string code, string newPassword);

        Task<bool> IsUserExistByUsernameAsync(string username);

        Task<bool> IsUserExistByEmailAsync(string email);

        Task<string> GenerateEmailConfirmationTokenAsync(UserModel user);

        Task<IdentityResult> ConfirmEmailAsync(string userId, string code);

        Task<IdentityResult> ChangeEmailAsync(UserModel user, string email, string token);

        Task<IdentityResult> ChangeEmailAsync(string userId, string email, string token);

        Task SendConfirmationEmailAsync(string email, string callbackUrl);

        Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);

        string GetUserId(ClaimsPrincipal claimsPrincipal);

        Task<bool> VerifyUserTokenAsync(string email, string token);

        Task<List<UserModel>> GetAllUsersAsync(ClaimsPrincipal userClaimsPrincipal, bool includeDeleted = false);

        Task UpdateUserRoleAsync(string userId, string roleName);

        Task SoftDeleteAsync(string userId);

        Task<List<UserModel>> GetAllRemovedUsersAsync();
    }
}
