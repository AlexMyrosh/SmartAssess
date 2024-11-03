using AutoMapper;
using Business_Logic_Layer.Services.Interfaces;
using Data_Access_Layer.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.ViewModels.Trash;

namespace Controllers
{
    public class TrashController(
        IAccountService _accountService,
        ICourseService _courseService,
        IExamService _examService,
        IMapper _mapper) : Controller
    {
        private const int PageSize = 10;

        [HttpGet]
        [Authorize(Roles = $"{RoleNames.Admin}")]
        public async Task<IActionResult> Index(string tab = "Deleted courses")
        {
            var deletedCourses = await _courseService.GetAllDeletedBySearchQueryWithPaginationAsync(PageSize);
            var deletedExams = await _examService.GetAllDeletedBySearchQueryWithPaginationAsync(PageSize);
            var deletedUsers = await _accountService.GetAllDeletedBySearchQueryWithPaginationAsync(PageSize);
            var viewModel = new TrashViewModel
            {
                DeletedCoursesWithPagination = _mapper.Map<DeletedCourseListWithPaginationViewModel>(deletedCourses),
                DeletedExamsWithPagination = _mapper.Map<DeletedExamListWithPaginationViewModel>(deletedExams),
                DeletedUsersWithPagination = _mapper.Map<DeletedUserListWithPaginationViewModel>(deletedUsers)
            };

            viewModel.DeletedCoursesWithPagination.Pagination.PageSize = PageSize;
            viewModel.DeletedCoursesWithPagination.Pagination.PageNumber = 1;
            viewModel.DeletedExamsWithPagination.Pagination.PageSize = PageSize;
            viewModel.DeletedExamsWithPagination.Pagination.PageNumber = 1;
            viewModel.DeletedUsersWithPagination.Pagination.PageSize = PageSize;
            viewModel.DeletedUsersWithPagination.Pagination.PageNumber = 1;

            viewModel.TabToOpen = tab;

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleNames.Admin}")]
        public async Task<IActionResult> PaginateDeletedCourses(int pageNumber = 1, string searchQuery = "")
        {
            var paginationCourseModel = await _courseService.GetAllDeletedBySearchQueryWithPaginationAsync(PageSize, searchQuery, pageNumber);
            var viewModel = _mapper.Map<DeletedCourseListWithPaginationViewModel>(paginationCourseModel);
            viewModel.Pagination.PageSize = PageSize;
            viewModel.Pagination.PageNumber = pageNumber;
            return PartialView("PartialViews/_DeletedCourseListAndPagination", viewModel);
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleNames.Admin}")]
        public async Task<IActionResult> PaginateDeletedExams(int pageNumber = 1, string searchQuery = "")
        {
            var paginationExamModel = await _examService.GetAllDeletedBySearchQueryWithPaginationAsync(PageSize, searchQuery, pageNumber);
            var viewModel = _mapper.Map<DeletedExamListWithPaginationViewModel>(paginationExamModel);
            viewModel.Pagination.PageSize = PageSize;
            viewModel.Pagination.PageNumber = pageNumber;
            return PartialView("PartialViews/_DeletedExamListAndPagination", viewModel);
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleNames.Admin}")]
        public async Task<IActionResult> PaginateDeletedUsers(int pageNumber = 1, string searchQuery = "")
        {
            var paginationUserModel = await _accountService.GetAllDeletedBySearchQueryWithPaginationAsync(PageSize, searchQuery, pageNumber);
            var viewModel = _mapper.Map<DeletedUserListWithPaginationViewModel>(paginationUserModel);
            viewModel.Pagination.PageSize = PageSize;
            viewModel.Pagination.PageNumber = pageNumber;
            return PartialView("PartialViews/_DeletedUserListAndPagination", viewModel);
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleNames.Admin}")]
        public async Task<IActionResult> RestoreCourse(Guid courseId)
        {
            await _courseService.RestoreAsync(courseId);
            return RedirectToAction("Index", new {tab = "Deleted courses" });
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleNames.Admin}")]
        public async Task<IActionResult> DeleteCoursePermanently(Guid courseId)
        {
            await _courseService.HardDeleteAsync(courseId);
            return RedirectToAction("Index", new { tab = "Deleted courses" });
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleNames.Admin}")]
        public async Task<IActionResult> RestoreExam(Guid examId)
        {
            await _examService.RestoreAsync(examId);
            return RedirectToAction("Index", new { tab = "Deleted exams" });
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleNames.Admin}")]
        public async Task<IActionResult> DeleteExamPermanently(Guid examId)
        {
            await _examService.HardDeleteAsync(examId);
            return RedirectToAction("Index", new { tab = "Deleted exams" });
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleNames.Admin}")]
        public async Task<IActionResult> RestoreUser(string userId)
        {
            await _accountService.RestoreAsync(userId);
            return RedirectToAction("Index", new { tab = "Deleted users" });
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleNames.Admin}")]
        public async Task<IActionResult> DeleteUserPermanently(string userId)
        {
            RemoveUserPhoto(userId);
            await _accountService.HardDeleteAsync(userId);
            return RedirectToAction("Index", new { tab = "Deleted users" });
        }

        private void RemoveUserPhoto(string userId)
        {
            var photoDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            var file = Directory.GetFiles(photoDirectory, $"{userId}.*").FirstOrDefault();
            if (System.IO.File.Exists(file))
            {
                System.IO.File.Delete(file);
            }
        }
    }
}
