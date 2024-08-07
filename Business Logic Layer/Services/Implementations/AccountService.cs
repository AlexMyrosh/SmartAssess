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

        public async Task<IdentityResult> CreateAsync(UserEntity user, string password)
        {
            var identityResult = await _userManager.CreateAsync(user, password);
            if (identityResult is null)
            {
                throw new NullReferenceException("identityResult is null");
            }

            if (identityResult.Succeeded)
            {
                // TODO: Replace "Student" by enum or const value
                await _userManager.AddToRoleAsync(user, "Student");
            }

            return identityResult;
        }

        public async Task<UserEntity?> GetUserAsync(ClaimsPrincipal userPrincipal)
        {
            var userEntity = await _userManager.GetUserAsync(userPrincipal);
            return userEntity;
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

        public async Task<string> GenerateEmailConfirmationTokenAsync(UserEntity user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return code;
        }

        public async Task<string> GenerateChangeEmailTokenAsync(UserEntity user, string newEmail)
        {
            var code = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
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

        public async Task<IdentityResult> ChangePasswordAsync(UserEntity user, string currentPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result;
        }

        public async Task<UserEntity?> GetUserAsync(string id)
        {
            var result = await _userManager.FindByIdAsync(id);
            return result;
        }

        public async Task<IdentityResult> ChangeEmailAsync(UserEntity user, string email, string token)
        {
            var result = await _userManager.ChangeEmailAsync(user, email, token);
            return result;
        }
    }
}
