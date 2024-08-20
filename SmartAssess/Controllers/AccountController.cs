using AutoMapper;
using Business_Logic_Layer.Models;
using Business_Logic_Layer.Services.Interfaces;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.ViewModels;

namespace Presentation_Layer.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly IAccountService _accountService;
        private readonly IPhoneMessageSender _phoneMessageSender;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, SignInManager<UserEntity> signInManager, IPhoneMessageSender phoneMessageSender, IMapper mapper)
        {
            _signInManager = signInManager;
            _mapper = mapper;
            _accountService = accountService;
            _phoneMessageSender = phoneMessageSender;
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
                var userModel = _mapper.Map<UserModel>(model);
                var userEntity = _mapper.Map<UserEntity>(userModel);
                var result = await _accountService.CreateAsync(userModel, model.Password);
                if (result.Succeeded)
                {
                    var confirmationToken = await _accountService.GenerateEmailConfirmationTokenAsync(userModel);
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
                    await _accountService.UpdateLastLoginDateAsync(User);
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
        [Authorize]
        public async Task<IActionResult> Details()
        {
            var currentUser = await _accountService.GetUserAsync(User);
            var viewModel = _mapper.Map<UserViewModel>(currentUser);
            return View(viewModel);
        }

        [HttpGet("Account/Details/{id}")]
        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            var currentUser = await _accountService.GetUserAsync(id);
            var viewModel = _mapper.Map<UserViewModel>(currentUser);
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateDescription(string userId, string description)
        {
            var userModel = new UserModel
            {
                Id = userId,
                AboutUser = description
            };
            await _accountService.UpdateAsync(userModel);
            return Json(new { success = true });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateFirstAndLastName(string userId, string firstName, string lastName)
        {
            await _accountService.UpdateAsync(new UserModel
            {
                Id = userId,
                FirstName = firstName,
                LastName = lastName
            });

            return Json(new { success = true });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateLocationAndEducationalInstitutionInfo(string userId, string country, string city, string educationalInstitution)
        {
            await _accountService.UpdateAsync(new UserModel
            {
                Id = userId,
                Country = country,
                City = city,
                EducationalInstitution = educationalInstitution
            });

            return Json(new { success = true });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Update()
        {
            var currentUser = await _accountService.GetUserAsync(User);
            var viewModel = _mapper.Map<UserViewModel>(currentUser);
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateImage(string userId, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var fileExtension = Path.GetExtension(file.FileName);
                var fileName = userId + fileExtension;
                var imagesFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images");
                if (!Directory.Exists(imagesFolderPath))
                {
                    Directory.CreateDirectory(imagesFolderPath);
                }

                var filePath = Path.Combine(imagesFolderPath, fileName);
                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                await _accountService.UpdateAsync(new UserModel
                {
                    Id = userId,
                    ImagePath = filePath
                });

                // Redirect back to user profile page
                return RedirectToAction("Details");
            }

            return RedirectToAction("Details");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveImage(string userId)
        {
            var user = await _accountService.GetUserWithoutTrackingAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(user.ImagePath))
            {
                if (System.IO.File.Exists(user.ImagePath))
                {
                    System.IO.File.Delete(user.ImagePath);
                }

                await _accountService.UpdateAsync(new UserModel
                {
                    Id = user.Id,
                    ImagePath = string.Empty 
                });
            }

            return RedirectToAction("Details");
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
                if (isUserExist)
                {
                    var resetToken = await _accountService.GenerateResetTokenAsync(model.Email);
                    var callbackUrl = Url.Action("ResetPassword", "Account", new { userEmail = model.Email, code = resetToken }, protocol: Request.Scheme);
                    await _accountService.ResetPasswordEmailAsync(model.Email, callbackUrl);
                }

                TempData["Notification"] = "If the email address is found in our system, we have sent a recovery email";
                return RedirectToAction("Login");
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

        [HttpGet]
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

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = string.Join("\n", ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage))) });
            }

            var result = await _accountService.ChangePasswordAsync(model.UserId, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return Json(new { success = true, message = "Password successfully updated" });
            }

            List<string> errorList = new();
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
                errorList.Add(error.Description);
            }

            return Json(new { success = false, message = string.Join("\n", errorList) });
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangeEmail()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<JsonResult> ChangeEmail(string userId, string newEmail)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = string.Join("\n", ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage))) });
            }

            var confirmationToken = await _accountService.GenerateChangeEmailTokenAsync(userId, newEmail);
            var callbackUrl = Url.Action("ConfirmEmailChange", "Account", new { userId, email = newEmail, token = confirmationToken }, protocol: Request.Scheme);
            await _accountService.ResetEmailAsync(newEmail, callbackUrl);
            return Json(new { success = true, message = "Confirmation email was sent" });
        }

        public async Task<IActionResult> ConfirmEmailChange(string? token, string? email, string? userId)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
            {
                TempData["ErrorNotification"] = "Invalid link, please try again later";
                return RedirectToAction("Details");
            }

            var result = await _accountService.ChangeEmailAsync(userId, email, token);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                TempData["ErrorNotification"] = "Something went wrong, please try again later";
                return RedirectToAction("Details");
            }

            TempData["SuccessNotification"] = $"Email confirmed and updated to {email}";
            return RedirectToAction("Details");
        }

        [HttpPost]
        public async Task<JsonResult> SendVerificationCode(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return Json(new { success = false, message = "Enter phone number" });
            }

            // Generate a verification code
            var verificationCode = _phoneMessageSender.GenerateVerificationCode();
            //await _phoneMessageSender.SendSmsAsync(phoneNumber, verificationCode);

            // Store the code and phone number in the session or database
            //HttpContext.Session.SetString("VerificationCode", verificationCode);
            //HttpContext.Session.SetString("PhoneNumber", phoneNumber);

            // Redirect to the confirmation page
            return Json(new { success = true, message = "Confirmation code was send" });
        }

        [HttpPost]
        public async Task<JsonResult> ConfirmVerificationCode(string userId, string code)
        {
            if (code is null || code.Length != 6)
            {
                return Json(new { success = false, message = "Enter verification code" });
            }

            // Redirect to the confirmation page
            return Json(new { success = true, message = "Phone number updated successfully" });
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