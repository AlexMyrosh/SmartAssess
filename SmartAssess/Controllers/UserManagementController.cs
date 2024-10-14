using AutoMapper;
using Business_Logic_Layer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.ViewModels.Account;
using Presentation_Layer.ViewModels.UserManagement;

namespace Presentation_Layer.Controllers
{
    public class UserManagementController(
        IAccountService accountService,
        IMapper mapper)
        : Controller
    {
        [HttpGet]
        public async Task<IActionResult> AllUsers()
        {
            var userModels = await accountService.GetAllUsersAsync(User);
            var viewModel = mapper.Map<AllUsersViewModel>(userModels);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserRole(string userId, string selectedRole)
        {
            await accountService.UpdateUserRoleAsync(userId, selectedRole);
            return RedirectToAction("AllUsers");
        }

        [HttpGet]
        public async Task<IActionResult> UserDetails(string userId)
        {
            var userModel = await accountService.GetUserAsync(userId);
            var viewModel = mapper.Map<AccountDetailsViewModel>(userModel);
            viewModel.IsInReadonlyMode = true;
            return View("~/Views/Account/Details.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            await accountService.SoftDeleteAsync(userId);
            return RedirectToAction("AllUsers");
        }
    }
}
