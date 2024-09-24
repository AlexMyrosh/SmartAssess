using AutoMapper;
using Business_Logic_Layer.Models;
using Business_Logic_Layer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.ViewModels;

namespace Presentation_Layer.Controllers
{
    [Authorize]
    public class ExamController : Controller
    {
        private readonly IExamService _examService;
        private readonly ICourseService _courseService;
        private readonly IMapper _mapper;

        public ExamController(IExamService examService, ICourseService courseService, IMapper mapper)
        {
            _examService = examService;
            _courseService = courseService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var examModels = await _examService.GetAllAvailableExamsWithDetailsAsync();
            var examViewModels = _mapper.Map<IEnumerable<ExamViewModel>>(examModels);
            return View(examViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var examModel = await _examService.GetByIdWithDetailsAsync(id);
            var examViewModel = _mapper.Map<ExamViewModel>(examModel);
            return View(examViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid courseId)
        {
            var courseModel = await _courseService.GetByIdAsync(courseId);
            var courseViewModel = _mapper.Map<CourseViewModel>(courseModel);
            var examViewModel = new ExamViewModel
            {
                Questions = new List<ExamQuestionViewModel>
                {
                    new()
                },
                Course = courseViewModel
            };

            return View(examViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExamViewModel examViewModel)
        {
            if (ModelState.IsValid)
            {
                var examModel = _mapper.Map<ExamModel>(examViewModel);
                await _examService.CreateAsync(examModel);
                return RedirectToAction("Details", "Course", new { id = examViewModel.Course.Id });
            }

            return View(examViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid examId, Guid courseId)
        {
            await _examService.SoftDeleteAsync(examId);
            return RedirectToAction("Details", "Course", new { id = courseId });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var examModel = await _examService.GetByIdWithDetailsAsync(id);
            var examViewModel = _mapper.Map<ExamViewModel>(examModel);
            return View(examViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ExamViewModel examViewModel)
        {
            if (ModelState.IsValid)
            {
                var examModel = _mapper.Map<ExamModel>(examViewModel);
                await _examService.UpdateAsync(examModel);
                return RedirectToAction("Details", "Course", new { id = examViewModel.Course.Id });
            }

            return View(examViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> UsersAnswers(Guid id)
        {
            var exam = await _examService.GetByIdWithDetailsAsync(id);
            var viewModel = _mapper.Map<List<UserExamAttemptViewModel>>(exam.UserExamAttempts);
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ExamStatistic(Guid id)
        {
            var exam = await _examService.GetByIdWithDetailsAsync(id);
            var viewModel = _mapper.Map<List<UserExamAttemptViewModel>>(exam.UserExamAttempts);
            return View(viewModel);
        }
    }
}