using System.Security.Claims;
using System.Text.Encodings.Web;
using AutoMapper;
using Business_Logic_Layer.Models;
using Business_Logic_Layer.Services.Interfaces;
using Data_Access_Layer.Models;
using Data_Access_Layer.Roles;
using Data_Access_Layer.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business_Logic_Layer.Services.Implementations
{
    public class AccountService(
        UserManager<UserEntity> userManager,
        IEmailSender emailSender,
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : IAccountService
    {
        public async Task<IdentityResult> CreateAsync(UserModel user, string password)
        {
            var userEntity = mapper.Map<UserEntity>(user);
            var identityResult = await userManager.CreateAsync(userEntity, password);
            if (identityResult is null)
            {
                throw new NullReferenceException("identityResult is null");
            }

            if (identityResult.Succeeded)
            {
                await userManager.AddToRoleAsync(userEntity, RoleNames.Student);
            }

            return identityResult;
        }

        public async Task<UserModel?> GetUserAsync(ClaimsPrincipal userPrincipal)
        {
            var userEntityId = (await userManager.GetUserAsync(userPrincipal))?.Id;
            var userEntity = await unitOfWork.UserRepository.GetByIdWithDetailsAsync(userEntityId);
            var userModel = mapper.Map<UserModel>(userEntity);
            userModel.Roles = await userManager.GetRolesAsync(userEntity);
            if (userModel.Roles.Contains(RoleNames.Teacher))
            {
                userModel.Courses = mapper.Map<List<CourseModel>>(userEntity.TeachingCourses);
            }
            
            return userModel;
        }

        public async Task<IdentityResult?> UpdateAsync(UserModel user)
        {
            var userFromDb = await unitOfWork.UserRepository.GetByIdAsync(user.Id);
            if (userFromDb is null)
            {
                throw new ArgumentException("Unable to get user by id", nameof(user.Id));
            }

            user.EducationalInstitution = user.EducationalInstitution is null ? string.Empty : user.EducationalInstitution;
            mapper.Map(user, userFromDb);
            var identityResult = await userManager.UpdateAsync(userFromDb);
            return identityResult;
        }

        public async Task<bool> ResetPasswordEmailAsync(string email, string callbackUrl)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null || !await userManager.IsEmailConfirmedAsync(user))
            {
                return false;
            }

            var htmlMessage = $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";
            await emailSender.SendEmailAsync(email, "Password reset", htmlMessage);
            return true;
        }

        public async Task ResetEmailAsync(string email, string callbackUrl)
        {
            var htmlMessage = $"Please confirm email by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";
            await emailSender.SendEmailAsync(email, "Email confirmation", htmlMessage);
        }

        public async Task<string> GenerateResetTokenAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
            {
                throw new ArgumentException("Unable to get user by email", nameof(email));
            }

            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            return code;
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(UserModel user)
        {
            var userEntity = mapper.Map<UserEntity>(user);
            var code = await userManager.GenerateEmailConfirmationTokenAsync(userEntity);
            return code;
        }

        public async Task<string> GenerateChangeEmailTokenAsync(string userId, string newEmail)
        {
            var userEntity = await unitOfWork.UserRepository.GetByIdAsync(userId);
            var code = await userManager.GenerateChangeEmailTokenAsync(userEntity, newEmail);
            return code;
        }

        public async Task<IdentityResult> ResetPasswordAsync(string email, string code, string newPassword)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
            {
                throw new ArgumentException("Unable to get user by email", nameof(email));
            }

            var result = await userManager.ResetPasswordAsync(user, code, newPassword);
            return result;
        }

        public async Task<bool> VerifyUserTokenAsync(string email, string token)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return false;
            }

            var result = await userManager.VerifyUserTokenAsync(
                user, 
                userManager.Options.Tokens.PasswordResetTokenProvider,
                "ResetPassword", 
                token);

            return result;
        }

        public async Task<List<UserModel>> GetAllUsersAsync(ClaimsPrincipal userClaimsPrincipal, bool includeDeleted = false)
        {
            var userId = userManager.GetUserId(userClaimsPrincipal);
            var userEntities = await userManager.Users.Where(x=>x.Id != userId && (x.IsDeleted == false || x.IsDeleted == includeDeleted)).ToListAsync();
            
            foreach (var userEntity in userEntities)
            {
                userEntity.Role = (await userManager.GetRolesAsync(userEntity)).First();
            }

            var userModels = mapper.Map<List<UserModel>>(userEntities);
            
            return userModels;
        }

        public async Task UpdateUserRoleAsync(string userId, string roleName)
        {
            var userEntity = userManager.Users.FirstOrDefault(u => u.Id == userId);
            if (userEntity is null)
            {
                return;
            }

            var currentRoles = await userManager.GetRolesAsync(userEntity);
            await userManager.RemoveFromRolesAsync(userEntity, currentRoles);
            await userManager.AddToRoleAsync(userEntity, roleName);
        }

        public async Task SoftDeleteAsync(string userId)
        {
            await unitOfWork.UserRepository.SoftDeleteAsync(userId);
            await unitOfWork.SaveAsync();
        }

        public async Task<List<UserModel>> GetAllRemovedUsersAsync()
        {
            var userEntities = await userManager.Users.Where(x => x.IsDeleted).ToListAsync();
            var userModels = mapper.Map<List<UserModel>>(userEntities);
            return userModels;
        }

        public async Task<bool> IsUserExistByUsernameAsync(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            return user != null;
        }

        public async Task<bool> IsUserExistByEmailAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string code)
        {
            var userEntity = await userManager.FindByIdAsync(userId);
            if (userEntity is null)
            {
                throw new ArgumentException("Unable to get user by id", nameof(userId));
            }

            var identityResult = await userManager.ConfirmEmailAsync(userEntity, code);
            return identityResult;
        }

        public async Task SendConfirmationEmailAsync(string email, string callbackUrl)
        {
            await emailSender.SendEmailAsync(email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
        }

        public async Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var userEntity = await unitOfWork.UserRepository.GetByIdAsync(userId);
            var result = await userManager.ChangePasswordAsync(userEntity, currentPassword, newPassword);
            return result;
        }

        public async Task<UserModel?> GetUserAsync(string id)
        {
            var userEntity = await unitOfWork.UserRepository.GetByIdWithDetailsAsync(id);
            var userModel = mapper.Map<UserModel>(userEntity);
            return userModel;
        }

        public async Task<UserModel?> GetUserWithoutTrackingAsync(string id)
        {
            var userEntity = await unitOfWork.UserRepository.GetByIdWithoutTrackingAsync(id);
            var userModel = mapper.Map<UserModel>(userEntity);
            return userModel;
        }

        public async Task<IdentityResult> ChangeEmailAsync(UserModel user, string email, string token)
        {
            var userEntity = mapper.Map<UserEntity>(user);
            var result = await userManager.ChangeEmailAsync(userEntity, email, token);
            return result;
        }

        public async Task<IdentityResult> ChangeEmailAsync(string userId, string email, string token)
        {
            var userEntity = await unitOfWork.UserRepository.GetByIdAsync(userId);
            var result = await userManager.ChangeEmailAsync(userEntity, email, token);
            return result;
        }

        public string GetUserId(ClaimsPrincipal claimsPrincipal)
        {
            var userId = userManager.GetUserId(claimsPrincipal);
            return userId;
        }

        public async Task<UserModel?> GetUserByEmailAsync(string email)
        {
            var userEntity = await userManager.FindByEmailAsync(email);
            var userModel = mapper.Map<UserModel>(userEntity);
            return userModel;
        }
    }
}
