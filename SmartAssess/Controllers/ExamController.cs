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
                var createdExamId = await _examService.CreateAsync(examModel);
                return RedirectToAction("Index", new { id = createdExamId });
            }

            return View(examViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _examService.SoftDeleteAsync(id);
            return RedirectToAction("Index");
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
                var updatedExamId = await _examService.UpdateAsync(examModel);
                return RedirectToAction("Details", new { id = updatedExamId });
            }

            return View(examViewModel);
        }
    }
}