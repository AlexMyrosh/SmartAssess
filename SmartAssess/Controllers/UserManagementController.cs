using AutoMapper;
using Business_Logic_Layer.Services.Interfaces;
using Data_Access_Layer.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.ViewModels.Account;
using Presentation_Layer.ViewModels.UserManagement;

namespace Controllers
{
    public class UserManagementController(
        IAccountService accountService,
        IMapper mapper)
        : Controller
    {
        private const int PageSize = 10;

        [HttpGet]
        [Authorize(Roles = $"{RoleNames.Admin}")]
        public async Task<IActionResult> AllUsers()
        {
            var paginationUserModel = await accountService.GetAllBySearchQueryWithPaginationAsync(User, PageSize);
            var viewModel = mapper.Map<AllUsersViewModel>(paginationUserModel);
            viewModel.Pagination.PageSize = PageSize;
            viewModel.Pagination.PageNumber = 1;
            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleNames.Admin}")]
        public async Task<IActionResult> PaginateAllUsers(int pageNumber = 1, string searchQuery = "")
        {
            var paginationUserModel = await accountService.GetAllBySearchQueryWithPaginationAsync(User, PageSize, searchQuery, pageNumber);
            var viewModel = mapper.Map<AllUsersViewModel>(paginationUserModel);
            viewModel.Pagination.PageSize = PageSize;
            viewModel.Pagination.PageNumber = pageNumber;
            return PartialView("PartialViews/_UserListAndPagination", viewModel);
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleNames.Admin}")]
        public async Task<IActionResult> UpdateUserRole(string userId, string selectedRole)
        {
            await accountService.UpdateUserRoleAsync(userId, selectedRole);
            return RedirectToAction("AllUsers");
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleNames.Teacher},{RoleNames.Admin}")]
        public async Task<IActionResult> UserDetails(string userId)
        {
            var userModel = await accountService.GetUserAsync(userId, true);
            var viewModel = mapper.Map<AccountDetailsViewModel>(userModel);
            viewModel.IsInReadonlyMode = true;
            return View("~/Views/Account/Details.cshtml", viewModel);
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleNames.Admin}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            await accountService.SoftDeleteAsync(userId, User);
            return RedirectToAction("AllUsers");
        }
    }
}
