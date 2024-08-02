﻿using AutoMapper;
using Business_Logic_Layer.Models;
using Business_Logic_Layer.Services.Implementations;
using Business_Logic_Layer.Services.Interfaces;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Presentation_Layer.ViewModels;
using System.Text;

namespace Presentation_Layer.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, SignInManager<UserEntity> signInManager, IMapper mapper)
        {
            _signInManager = signInManager;
            _mapper = mapper;
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userEntity = _mapper.Map<UserEntity>(model);
                var result = await _accountService.CreateAsync(userEntity, model.Password);
                if (result.Succeeded)
                {
                    var confirmationToken = await _accountService.GenerateEmailConfirmationTokenAsync(userEntity);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userEntity.Id, code = confirmationToken }, protocol: HttpContext.Request.Scheme);
                    await _accountService.SendConfirmationEmailAsync(userEntity.Email, callbackUrl);
                    await _signInManager.SignInAsync(userEntity, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid login attempt");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Details()
        {
            var currentUser = await _accountService.GetUserAsync(User);
            var viewModel = _mapper.Map<UserViewModel>(currentUser);
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Update()
        {
            var currentUser = await _accountService.GetUserAsync(User);
            var viewModel = _mapper.Map<UserViewModel>(currentUser);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserViewModel user)
        {
            var userModel = _mapper.Map<UserModel>(user);
            var result = await _accountService.UpdateAsync(userModel);
            if (result.Succeeded)
            {
                return RedirectToAction("Details");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(user);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isUserExist = await _accountService.IsUserExistByEmailAsync(model.Email);
                if (!isUserExist)
                {
                    ModelState.AddModelError("", "User with this email is not found, please check email or try later");
                    return View(model);
                }

                var resetToken = await _accountService.GenerateResetTokenAsync(model.Email);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userEmail = model.Email, code = resetToken }, protocol: Request.Scheme);
                var result = await _accountService.ResetPasswordEmailAsync(model.Email, callbackUrl);
                if (result)
                {
                    var notification = "Email for password reset it sent, please check your email";
                    return View("_SuccessfulNotification", notification);
                }
                else
                {
                    var notification = "Something went wrong, please check that email is correct or wait and try later";
                    return View("_FailedNotification", notification);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string userEmail, string code)
        {
            var viewModel = new ResetPasswordViewModel
            {
                Email = userEmail,
                Code = code
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _accountService.ResetPasswordAsync(model.Email, model.Code, model.NewPassword);
            if (result.Succeeded)
            {
                TempData["Notification"] = "Password updated";
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            var result = await _accountService.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                var successNotification = "Email confirmed successfully";
                return View("_SuccessfulNotification", successNotification);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            var failedNotification = "Email confirmation failed";
            return View("_FailedNotification", failedNotification);
        }

        [HttpGet("validation/username")]
        public async Task<JsonResult> ValidateUsername(string username)
        {
            var exist = await _accountService.IsUserExistByUsernameAsync(username);
            return !exist ? Json(true) : Json($"User with \"{username}\" username is already exist");
        }

        [HttpGet("validation/email")]
        public async Task<JsonResult> ValidateEmail(string email)
        {
            var exist = await _accountService.IsUserExistByEmailAsync(email);
            return !exist ? Json(true) : Json($"User with \"{email}\" email is already exist");
        }

    }
}