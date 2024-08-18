using AutoMapper;
using Business_Logic_Layer.Models;
using Business_Logic_Layer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.ViewModels;

namespace Presentation_Layer.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public CourseController(ICourseService courseService,  IMapper mapper, IAccountService accountService)
        {
            _courseService = courseService;
            _mapper = mapper;
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var courseModels = await _courseService.GetAllWithDetailsAsync();
            var courseViewModels = _mapper.Map<IEnumerable<CourseViewModel>>(courseModels);
            return View(courseViewModels);
        }


        [HttpGet]
        public async Task<IActionResult> AppliedCourses()
        {
            var courseModels = await _courseService.GetAllAvailableForUserCoursesWithDetailsAsync(User);
            var courseViewModels = _mapper.Map<IEnumerable<CourseViewModel>>(courseModels);
            return View(courseViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> ApplyForCourse(Guid courseId)
        {
            await _courseService.AddUserForCourseAsync(User, courseId);
            return RedirectToAction("Details", new { id = courseId });
        }

        [HttpPost]
        public async Task<IActionResult> LeaveCourse(Guid courseId)
        {
            await _courseService.RemoveUserFromCourseAsync(User, courseId);
            return RedirectToAction("AppliedCourses");
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var courseModel = await _courseService.GetByIdWithDetailsAsync(id);
            var courseViewModel = _mapper.Map<CourseViewModel>(courseModel);

            // TODO: Move to Business Logic
            var currentUser = await _accountService.GetUserAsync(User);
            foreach (var exam in courseViewModel.Exams)
            {
                exam.CurrentUserAttmptNumber = exam.UserExamAttempts.Count(x => x.User.Id == currentUser.Id);
            }

            return View(courseViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var courseModel = _mapper.Map<CourseModel>(viewModel);
                var createdCourseId = await _courseService.CreateAsync(courseModel);
                return RedirectToAction("Details", new { id = createdCourseId });
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var courseModel = await _courseService.GetByIdWithDetailsAsync(id);
            var courseViewModel = _mapper.Map<CourseViewModel>(courseModel);
            return View(courseViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CourseViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var courseModel = _mapper.Map<CourseModel>(viewModel);
                var updatedCourseId = await _courseService.UpdateAsync(courseModel);
                return RedirectToAction("Details", new { id = updatedCourseId });
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _courseService.SoftDeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
