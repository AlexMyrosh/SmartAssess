using AutoMapper;
using Business_Logic_Layer.Models;
using Business_Logic_Layer.Services.Implementations;
using Business_Logic_Layer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.ViewModels;

namespace Presentation_Layer.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IMapper _mapper;

        public CourseController(ICourseService courseService, IMapper mapper)
        {
            _courseService = courseService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var courseModels = await _courseService.GetAllWithDetailsAsync();
            var courseViewModels = _mapper.Map<IEnumerable<CourseViewModel>>(courseModels);
            return View(courseViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var courseModel = await _courseService.GetByIdWithDetailsAsync(id);
            var courseViewModel = _mapper.Map<CourseViewModel>(courseModel);
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
