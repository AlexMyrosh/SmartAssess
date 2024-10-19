using AutoMapper;
using Business_Logic_Layer.Models;
using Business_Logic_Layer.Services.Interfaces;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.ViewModels.Account;

namespace Presentation_Layer.Controllers
{
    public class AccountController(
        IAccountService accountService,
        SignInManager<UserEntity> signInManager,
        IMapper mapper)
        : Controller
    {
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
                var userModel = mapper.Map<UserModel>(model);
                var result = await accountService.CreateAsync(userModel, model.Password);
                if (result.Succeeded)
                {
                    var createdUser = await accountService.GetUserByEmailAsync(model.Email);
                    Task.Run(async () => await SendConfirmationEmailAsync(createdUser));

                    var userEntity = mapper.Map<UserEntity>(createdUser);

                    await signInManager.SignInAsync(userEntity, isPersistent: false);
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
                var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid login attempt");
            }

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details()
        {
            var currentUser = await accountService.GetUserAsync(User);
            var viewModel = mapper.Map<AccountDetailsViewModel>(currentUser);
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
            var result = await accountService.UpdateAsync(userModel);
            if (result is { Succeeded: true })
            {
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateFirstAndLastName(string userId, string firstName, string lastName)
        {
            var result = await accountService.UpdateAsync(new UserModel
            {
                Id = userId,
                FirstName = firstName,
                LastName = lastName
            });

            if (result is { Succeeded: true })
            {
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateLocationAndEducationalInstitutionInfo(string userId, string country, string city, string educationalInstitution)
        {
            var result = await accountService.UpdateAsync(new UserModel
            {
                Id = userId,
                Country = country,
                City = city,
                EducationalInstitution = educationalInstitution
            });

            if (result is { Succeeded: true })
            {
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateImage(string userId, IFormFile file)
        {
            if (file.Length > 0)
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

                await accountService.UpdateAsync(new UserModel
                {
                    Id = userId,
                    ImagePath = filePath
                });

                return RedirectToAction("Details");
            }

            return RedirectToAction("Details");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveImage(string userId)
        {
            var user = await accountService.GetUserWithoutTrackingAsync(userId);
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

                await accountService.UpdateAsync(new UserModel
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
                var isUserExist = await accountService.IsUserExistByEmailAsync(model.Email);
                if (isUserExist)
                {
                    var resetToken = await accountService.GenerateResetTokenAsync(model.Email);
                    var callbackUrl = Url.Action("ResetPassword", "Account", new { userEmail = model.Email, code = resetToken }, protocol: Request.Scheme);
                    var result = await accountService.ResetPasswordEmailAsync(model.Email, callbackUrl);
                    if (!result)
                    {
                        Thread.Sleep(5000);
                    }
                }
                else
                {
                    Thread.Sleep(5000);
                }

                TempData["Notification"] = "If the email address is found in the system - a recovery email has been sent";
                return RedirectToAction("Login");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string userEmail, string code)
        {
            if (!await accountService.VerifyUserTokenAsync(userEmail, code))
            {
                return RedirectToAction("Error404", "Error");
            }

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

            var result = await accountService.ResetPasswordAsync(model.Email, model.Code, model.NewPassword);
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
            var result = await accountService.ConfirmEmailAsync(userId, code);
            var isUserSignedIn = signInManager.IsSignedIn(User);
            if (result.Succeeded)
            {
                TempData["SuccessNotification"] = "Email confirmed successfully";
                if (isUserSignedIn)
                {
                    return RedirectToAction("Details");
                }

                return RedirectToAction("Index", "Home");
            }

            TempData["ErrorNotification"] = "Email confirmation failed";
            if (isUserSignedIn)
            {
                return RedirectToAction("Details");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        public async Task<JsonResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = string.Join("\n", ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage))) });
            }

            var result = await accountService.ChangePasswordAsync(model.UserId, model.CurrentPassword, model.NewPassword);
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

        [HttpPost]
        [Authorize]
        public async Task<JsonResult> ChangeEmail(string userId, string newEmail)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = string.Join("\n", ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage))) });
            }

            var confirmationToken = await accountService.GenerateChangeEmailTokenAsync(userId, newEmail);
            var callbackUrl = Url.Action("ConfirmEmailChange", "Account", new { userId, email = newEmail, token = confirmationToken }, protocol: Request.Scheme);
            await accountService.ResetEmailAsync(newEmail, callbackUrl);
            return Json(new { success = true, message = "Confirmation email was sent" });
        }

        [HttpPost]
        [Authorize]
        public async Task<JsonResult> ChangeEmailValidation(ChangeEmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = string.Join("\n", ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage))) });
            }

            var isEmailTaken = await accountService.IsUserExistByEmailAsync(model.NewEmail);
            if (isEmailTaken)
            {
                return Json(new { success = false, message = "Email is taken" });
            }

            return Json(new { success = true, message = "If email exist - confirmation message was sent" });
        }

        public async Task<IActionResult> ConfirmEmailChange(string? token, string? email, string? userId)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
            {
                TempData["ErrorNotification"] = "Invalid link, please try again later";
                return RedirectToAction("Details");
            }

            var result = await accountService.ChangeEmailAsync(userId, email, token);
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

        private async Task SendConfirmationEmailAsync(UserModel userEntity)
        {
            var confirmationToken = await accountService.GenerateEmailConfirmationTokenAsync(userEntity.Id);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userEntity.Id, code = confirmationToken }, protocol: HttpContext.Request.Scheme);
            await accountService.SendConfirmationEmailAsync(userEntity.Email, callbackUrl);
        }
    }
}