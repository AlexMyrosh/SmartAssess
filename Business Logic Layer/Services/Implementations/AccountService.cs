using System.Security.Claims;
using System.Text.Encodings.Web;
using AutoMapper;
using Business_Logic_Layer.Models;
using Business_Logic_Layer.Services.Interfaces;
using Data_Access_Layer.Models;
using Data_Access_Layer.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Business_Logic_Layer.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountService(UserManager<UserEntity> userManager, IEmailSender emailSender, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IdentityResult> CreateAsync(UserModel user, string password)
        {
            var userEntity = _mapper.Map<UserEntity>(user);
            var identityResult = await _userManager.CreateAsync(userEntity, password);
            if (identityResult is null)
            {
                throw new NullReferenceException("identityResult is null");
            }

            if (identityResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(userEntity, Data_Access_Layer.Roles.Roles.Student);
            }

            return identityResult;
        }

        public async Task<UserModel?> GetUserAsync(ClaimsPrincipal userPrincipal)
        {
            var userEntityId = (await _userManager.GetUserAsync(userPrincipal))?.Id;
            var userEntity = await _unitOfWork.UserRepository.GetByIdWithDetailsAsync(userEntityId);
            var userModel = _mapper.Map<UserModel>(userEntity);
            userModel.Roles = await _userManager.GetRolesAsync(userEntity);
            return userModel;
        }

        public async Task<IdentityResult?> UpdateAsync(UserModel user)
        {
            var userFromDb = await _unitOfWork.UserRepository.GetByIdAsync(user.Id);
            if (userFromDb is null)
            {
                throw new ArgumentException("Unable to get user by id", nameof(user.Id));
            }

            _mapper.Map(user, userFromDb);
            var identityResult = await _userManager.UpdateAsync(userFromDb);
            return identityResult;
        }

        public async Task<bool> ResetPasswordEmailAsync(string email, string callbackUrl)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
            {
                return false;
            }

            var htmlMessage = $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";
            await _emailSender.SendEmailAsync(email, "Password reset", htmlMessage);
            return true;
        }

        public async Task ResetEmailAsync(string email, string callbackUrl)
        {
            var htmlMessage = $"Please confirm email by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";
            await _emailSender.SendEmailAsync(email, "Email confirmation", htmlMessage);
        }

        public async Task<string> GenerateResetTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                throw new ArgumentException("Unable to get user by email", nameof(email));
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            return code;
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(UserModel user)
        {
            var userEntity = _mapper.Map<UserEntity>(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(userEntity);
            return code;
        }

        public async Task<string> GenerateChangeEmailTokenAsync(string userId, string newEmail)
        {
            var userEntity = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            var code = await _userManager.GenerateChangeEmailTokenAsync(userEntity, newEmail);
            return code;
        }

        public async Task<IdentityResult> ResetPasswordAsync(string email, string code, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                throw new ArgumentException("Unable to get user by email", nameof(email));
            }

            var result = await _userManager.ResetPasswordAsync(user, code, newPassword);
            return result;
        }

        public async Task<bool> VerifyUserTokenAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return false;
            }

            var result = await _userManager.VerifyUserTokenAsync(
                user, 
                _userManager.Options.Tokens.PasswordResetTokenProvider,
                "ResetPassword", 
                token);

            return result;
        }

        public async Task<bool> IsUserExistByUsernameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user != null;
        }

        public async Task<bool> IsUserExistByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string code)
        {
            var userEntity = await _userManager.FindByIdAsync(userId);
            if (userEntity is null)
            {
                throw new ArgumentException("Unable to get user by id", nameof(userId));
            }

            var identityResult = await _userManager.ConfirmEmailAsync(userEntity, code);
            return identityResult;
        }

        public async Task SendConfirmationEmailAsync(string email, string callbackUrl)
        {
            await _emailSender.SendEmailAsync(email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
        }

        public async Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var userEntity = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            var result = await _userManager.ChangePasswordAsync(userEntity, currentPassword, newPassword);
            return result;
        }

        public async Task<UserModel?> GetUserAsync(string id)
        {
            var userEntity = await _userManager.FindByIdAsync(id);
            var userModel = _mapper.Map<UserModel>(userEntity);
            return userModel;
        }

        public async Task<UserModel?> GetUserWithoutTrackingAsync(string id)
        {
            var userEntity = await _unitOfWork.UserRepository.GetByIdWithoutTrackingAsync(id);
            var userModel = _mapper.Map<UserModel>(userEntity);
            return userModel;
        }

        public async Task<IdentityResult> ChangeEmailAsync(UserModel user, string email, string token)
        {
            var userEntity = _mapper.Map<UserEntity>(user);
            var result = await _userManager.ChangeEmailAsync(userEntity, email, token);
            return result;
        }

        public async Task<IdentityResult> ChangeEmailAsync(string userId, string email, string token)
        {
            var userEntity = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            var result = await _userManager.ChangeEmailAsync(userEntity, email, token);
            return result;
        }

        public string GetUserId(ClaimsPrincipal claimsPrincipal)
        {
            var userId = _userManager.GetUserId(claimsPrincipal);
            return userId;
        }
    }
}
