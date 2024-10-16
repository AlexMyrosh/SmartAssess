using AutoMapper;
using Business_Logic_Layer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.ViewModels.Trash;
using Presentation_Layer.ViewModels.Trash.Shared;

namespace Presentation_Layer.Controllers
{
    public class TrashController(
        IAccountService _accountService,
        ICourseService _courseService,
        IExamService _examService,
        IMapper _mapper) : Controller
    {
        private const int PageSize = 10;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var deletedCourses = await _courseService.GetAllDeletedBySearchQueryWithPaginationAsync(PageSize);
            var deletedExams = await _examService.GetAllRemovedExamsAsync();
            var deletedUsers = await _accountService.GetAllRemovedUsersAsync();
            var viewModel = new TrashViewModel
            {
                DeletedCoursesWithPagination = _mapper.Map<DeletedCourseListWithPaginationViewModel>(deletedCourses),
                DeletedExams = _mapper.Map<List<ExamViewModel>>(deletedExams),
                DeletedUsers = _mapper.Map<List<UserViewModel>>(deletedUsers)
            };

            viewModel.DeletedCoursesWithPagination.Pagination.PageSize = PageSize;
            viewModel.DeletedCoursesWithPagination.Pagination.PageNumber = 1;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> PaginateDeletedCourses(int pageNumber = 1, string searchQuery = "")
        {
            var paginationCourseModel = await _courseService.GetAllDeletedBySearchQueryWithPaginationAsync(PageSize, searchQuery, pageNumber);
            var viewModel = _mapper.Map<DeletedCourseListWithPaginationViewModel>(paginationCourseModel);
            viewModel.Pagination.PageSize = PageSize;
            viewModel.Pagination.PageNumber = pageNumber;
            return PartialView("PartialViews/_DeletedCourseListAndPagination", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> RestoreCourse(Guid courseId)
        {
            await _courseService.RestoreAsync(courseId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCoursePermanently(Guid courseId)
        {
            await _courseService.HardDeleteAsync(courseId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RestoreExam(Guid examId)
        {
            await _examService.RestoreAsync(examId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteExamPermanently(Guid examId)
        {
            await _examService.HardDeleteAsync(examId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RestoreUser(string userId)
        {
            await _accountService.RestoreAsync(userId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserPermanently(string userId)
        {
            await _accountService.HardDeleteAsync(userId);
            return RedirectToAction("Index");
        }
    }
}
