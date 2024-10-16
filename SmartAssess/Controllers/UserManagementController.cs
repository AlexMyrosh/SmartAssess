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
        private const int PageSize = 10;

        [HttpGet]
        public async Task<IActionResult> AllUsers()
        {
            var paginationUserModel = await accountService.GetAllBySearchQueryWithPaginationAsync(User, PageSize);
            var viewModel = mapper.Map<AllUsersViewModel>(paginationUserModel);
            viewModel.Pagination.PageSize = PageSize;
            viewModel.Pagination.PageNumber = 1;
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> PaginateAllUsers(int pageNumber = 1, string searchQuery = "")
        {
            var paginationUserModel = await accountService.GetAllBySearchQueryWithPaginationAsync(User, PageSize, searchQuery, pageNumber);
            var viewModel = mapper.Map<AllUsersViewModel>(paginationUserModel);
            viewModel.Pagination.PageSize = PageSize;
            viewModel.Pagination.PageNumber = pageNumber;
            return PartialView("PartialViews/_UserListAndPagination", viewModel);
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
            await accountService.SoftDeleteAsync(userId, User);
            return RedirectToAction("AllUsers");
        }
    }
}
