using AutoMapper;
using Business_Logic_Layer.Models;
using Business_Logic_Layer.Models.Enums;
using Business_Logic_Layer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.ViewModels.Enums;
using Presentation_Layer.ViewModels.Shared;

namespace Presentation_Layer.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IAccountService _accountService;
        private readonly IUserExamPassService _userExamPassService;
        private readonly IMapper _mapper;

        private const int PageSize = 12;

        public CourseController(ICourseService courseService,  IMapper mapper, IAccountService accountService, IUserExamPassService userExamPassService)
        {
            _courseService = courseService;
            _mapper = mapper;
            _accountService = accountService;
            _userExamPassService = userExamPassService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var paginationCourseModel = await _courseService.GetAllBySearchQueryWithPaginationAsync(PageSize);
            var viewModel = _mapper.Map<PaginationCourseViewModel>(paginationCourseModel);
            viewModel.PageSize = PageSize;
            viewModel.PageNumber = 1;
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> PaginateCourses(int pageNumber = 1, string searchQuery = "")
        {
            var paginationCourseModel = await _courseService.GetAllBySearchQueryWithPaginationAsync(PageSize, searchQuery, pageNumber);
            var viewModel = _mapper.Map<PaginationCourseViewModel>(paginationCourseModel);
            viewModel.PageSize = PageSize;
            viewModel.PageNumber = pageNumber;
            return PartialView("PartialViews/_CourseListAndPagination", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AppliedCourses()
        {
            var paginationCourseModel = await _courseService.GetAllAppliedByUserBySearchQueryWithPaginationAsync(User, PageSize);
            var viewModel = _mapper.Map<PaginationCourseViewModel>(paginationCourseModel);
            viewModel.PageSize = PageSize;
            viewModel.PageNumber = 1;
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> PaginateAppliedCourses(int pageNumber = 1, string searchQuery = "")
        {
            var paginationCourseModel = await _courseService.GetAllAppliedByUserBySearchQueryWithPaginationAsync(User, PageSize, searchQuery, pageNumber);
            var viewModel = _mapper.Map<PaginationCourseViewModel>(paginationCourseModel);
            viewModel.PageSize = PageSize;
            viewModel.PageNumber = pageNumber;
            return PartialView("PartialViews/_CourseListAndPagination", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AppliedByTeacherCourses()
        {
            var paginationCourseModel = await _courseService.GetAllAppliedByTeacherBySearchQueryWithPaginationAsync(User, PageSize);
            var viewModel = _mapper.Map<PaginationCourseViewModel>(paginationCourseModel);
            viewModel.PageSize = PageSize;
            viewModel.PageNumber = 1;
            return View("AppliedCourses", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> PaginateAppliedByTeacherCourses(int pageNumber = 1, string searchQuery = "")
        {
            var paginationCourseModel = await _courseService.GetAllAppliedByTeacherBySearchQueryWithPaginationAsync(User, PageSize, searchQuery, pageNumber);
            var viewModel = _mapper.Map<PaginationCourseViewModel>(paginationCourseModel);
            viewModel.PageSize = PageSize;
            viewModel.PageNumber = pageNumber;
            return PartialView("PartialViews/_CourseListAndPagination", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ApplyForCourse(Guid courseId)
        {
            await _courseService.AddUserForCourseAsync(User, courseId);
            return RedirectToAction("Details", new { id = courseId});
        }

        [HttpPost]
        public async Task<IActionResult> ApplyForCoursesAsTeacher(Guid courseId)
        {
            await _courseService.AddTeacherForCourseAsync(User, courseId);
            return RedirectToAction("Details", new { id = courseId });
        }

        [HttpPost]
        public async Task<IActionResult> LeaveCourse(Guid courseId)
        {
            await _courseService.RemoveUserFromCourseAsync(User, courseId);
            return RedirectToAction("Details", new { id = courseId });
        }

        [HttpPost]
        public async Task<IActionResult> LeaveCourseAsTeacher(Guid courseId)
        {
            await _courseService.RemoveTeacherFromCourseAsync(User, courseId);
            return RedirectToAction("Details", new { id = courseId });
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var courseModel = await _courseService.GetByIdWithDetailsAsync(id);
            var courseViewModel = _mapper.Map<CourseViewModel>(courseModel);
            var currentUserId = _accountService.GetUserId(User);
            ViewBag.UserId = currentUserId;

            // TODO: Move to Business Logic
            foreach (var exam in courseViewModel.Exams)
            {
                exam.CurrentUserAttmptNumber = exam.UsersExamPasses.FirstOrDefault(x => x.User.Id == currentUserId).UserExamAttempts.Count;
                var startedExam = exam.UsersExamPasses.FirstOrDefault(x => x.User.Id == currentUserId).UserExamAttempts.FirstOrDefault(y => y.Status == ExamAttemptStatusViewModel.InProgress);
                if (startedExam != null && DateTimeOffset.Now - startedExam.AttemptStarterAt > startedExam.Exam?.ExamDuration)
                {
                    // set to complete
                    await _userExamPassService.SetStatusAsync(startedExam.Id.Value, ExamAttemptStatusModel.Completed);
                }
                else if(startedExam != null)
                {
                    exam.StartedByCurrentUserAttemptId = startedExam?.Id;
                }
            }

            return View(courseViewModel);
        }

        [HttpPost]
        public async Task<JsonResult> Create(CourseViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var courseModel = _mapper.Map<CourseModel>(viewModel);
                var createdCourseId = await _courseService.CreateAsync(courseModel, _accountService.GetUserId(User));
                return Json(new { success = true, courseId = createdCourseId });
            }

            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors) });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _courseService.SoftDeleteAsync(id);
            return RedirectToAction("AppliedByTeacherCourses");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLongDescription(Guid courseId, string longDescription)
        {
            await _courseService.UpdateLongDescriptionAsync(courseId, longDescription);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserFromCourse(string userId, Guid courseId)
        {
            await _courseService.RemoveUserFromCourseAsync(userId, courseId);
            return Json(new { success = true });
        }
    }
}
