using AutoMapper;
using Business_Logic_Layer.Models;
using Business_Logic_Layer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.ViewModels.Course;
using Presentation_Layer.ViewModels.Course.Shared;


namespace Presentation_Layer.Controllers
{
    [Authorize]
    public class CourseController(
        ICourseService courseService,
        IMapper mapper,
        IAccountService accountService)
        : Controller
    {
        private const int PageSize = 12;

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var paginationCourseModel = await courseService.GetAllBySearchQueryWithPaginationAsync(PageSize);
            var viewModel = mapper.Map<AllCoursesViewModel>(paginationCourseModel);
            viewModel.CourseListWithPagination.Pagination.PageSize = PageSize;
            viewModel.CourseListWithPagination.Pagination.PageNumber = 1;
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> PaginateAllCourses(int pageNumber = 1, string searchQuery = "")
        {
            var paginationCourseModel = await courseService.GetAllBySearchQueryWithPaginationAsync(PageSize, searchQuery, pageNumber);
            var viewModel = mapper.Map<CourseListWithPaginationViewModel>(paginationCourseModel);
            viewModel.Pagination.PageSize = PageSize;
            viewModel.Pagination.PageNumber = pageNumber;
            return PartialView("PartialViews/_CourseListAndPagination", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Applied()
        {
            var paginationCourseModel = await courseService.GetAllAppliedByUserBySearchQueryWithPaginationAsync(User, PageSize);
            var viewModel = mapper.Map<AppliedCoursesViewModel>(paginationCourseModel);
            viewModel.CourseListWithPagination.Pagination.PageSize = PageSize;
            viewModel.CourseListWithPagination.Pagination.PageNumber = 1;
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> PaginateAppliedCourses(int pageNumber = 1, string searchQuery = "")
        { 
            var paginationCourseModel = await courseService.GetAllAppliedByUserBySearchQueryWithPaginationAsync(User, PageSize, searchQuery, pageNumber);
            var viewModel = mapper.Map<CourseListWithPaginationViewModel>(paginationCourseModel);
            viewModel.Pagination.PageSize = PageSize;
            viewModel.Pagination.PageNumber = pageNumber;
            return PartialView("PartialViews/_CourseListAndPagination", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AppliedByTeacherCourses()
        {
            var paginationCourseModel = await courseService.GetAllAppliedByTeacherBySearchQueryWithPaginationAsync(User, PageSize);
            var viewModel = mapper.Map<AppliedCoursesViewModel>(paginationCourseModel);
            viewModel.CourseListWithPagination.Pagination.PageSize = PageSize;
            viewModel.CourseListWithPagination.Pagination.PageNumber = 1;
            return View("Applied", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> PaginateAppliedByTeacherCourses(int pageNumber = 1, string searchQuery = "")
        {
            var paginationCourseModel = await courseService.GetAllAppliedByTeacherBySearchQueryWithPaginationAsync(User, PageSize, searchQuery, pageNumber);
            var viewModel = mapper.Map<CourseListWithPaginationViewModel>(paginationCourseModel);
            viewModel.Pagination.PageSize = PageSize;
            viewModel.Pagination.PageNumber = pageNumber;
            return PartialView("PartialViews/_CourseListAndPagination", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ApplyForCourse(Guid courseId)
        {
            await courseService.AddUserForCourseAsync(User, courseId);
            return RedirectToAction("Details", new { id = courseId});
        }

        [HttpPost]
        public async Task<IActionResult> ApplyForCoursesAsTeacher(Guid courseId)
        {
            await courseService.AddTeacherForCourseAsync(User, courseId);
            return RedirectToAction("Details", new { id = courseId });
        }

        [HttpPost]
        public async Task<IActionResult> LeaveCourse(Guid courseId)
        {
            await courseService.RemoveUserFromCourseAsync(User, courseId);
            return RedirectToAction("Details", new { id = courseId });
        }

        [HttpPost]
        public async Task<IActionResult> LeaveCourseAsTeacher(Guid courseId)
        {
            await courseService.RemoveTeacherFromCourseAsync(User, courseId);
            return RedirectToAction("Details", new { id = courseId });
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var courseModel = await courseService.GetByIdWithDetailsAsync(id, User);
            var courseViewModel = mapper.Map<CourseDetailsViewModel>(courseModel);
            return View(courseViewModel);
        }

        [HttpPost]
        public async Task<JsonResult> Create(CourseViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var courseModel = mapper.Map<CourseModel>(viewModel);
                var createdCourseId = await courseService.CreateAsync(courseModel, accountService.GetUserId(User));
                return Json(new { success = true, courseId = createdCourseId });
            }

            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors) });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await courseService.SoftDeleteAsync(id, User);
            return RedirectToAction("AppliedByTeacherCourses");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLongDescription(Guid courseId, string longDescription)
        {
            await courseService.UpdateLongDescriptionAsync(courseId, longDescription);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserFromCourse(string userId, Guid courseId)
        {
            await courseService.RemoveUserFromCourseAsync(userId, courseId);
            return Json(new { success = true });
        }
    }
}