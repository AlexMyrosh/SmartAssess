using AutoMapper;
using Business_Logic_Layer.Models;
using Business_Logic_Layer.Services.Interfaces;
using Data_Access_Layer.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.ViewModels.Exam;
using Presentation_Layer.ViewModels.Exam.Shared;

namespace Presentation_Layer.Controllers
{
    [Authorize]
    public class ExamController(IExamService examService, ICourseService courseService, IMapper mapper)
        : Controller
    {
        [HttpGet]
        [Authorize(Roles = $"{RoleNames.Teacher},{RoleNames.Admin}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var examModel = await examService.GetByIdWithDetailsAsync(id);
            var examViewModel = mapper.Map<ExamDetailsViewModel>(examModel);
            return View(examViewModel);
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleNames.Teacher},{RoleNames.Admin}")]
        public async Task<IActionResult> Create(Guid courseId)
        {
            var courseModel = await courseService.GetByIdAsync(courseId);
            var examViewModel = new ExamViewModel
            {
                Questions = new List<QuestionViewModel>
                {
                    new()
                },
                CourseId = courseModel.Id,
                CourseName = courseModel.Name,
                ExamStartDateTime = DateTimeOffset.Now.Date,
                ExamEndDateTime = DateTimeOffset.Now.Date
            };

            return View(examViewModel);
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleNames.Teacher},{RoleNames.Admin}")]
        public async Task<IActionResult> Create(ExamViewModel examViewModel)
        {
            if (ModelState.IsValid)
            {
                var examModel = mapper.Map<ExamModel>(examViewModel);
                await examService.CreateAsync(examModel);
                return RedirectToAction("Details", "Course", new { id = examViewModel.CourseId });
            }

            return View(examViewModel);
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleNames.Teacher},{RoleNames.Admin}")]
        public async Task<IActionResult> Delete(Guid examId, Guid courseId)
        {
            await examService.SoftDeleteAsync(examId, User);
            return RedirectToAction("Details", "Course", new { id = courseId });
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleNames.Teacher},{RoleNames.Admin}")]
        public async Task<IActionResult> Update(Guid id)
        {
            var examModel = await examService.GetByIdWithDetailsAsync(id);
            var examViewModel = mapper.Map<ExamViewModel>(examModel);
            return View(examViewModel);
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleNames.Teacher},{RoleNames.Admin}")]
        public async Task<IActionResult> Update(ExamViewModel examViewModel)
        {
            if (ModelState.IsValid)
            {
                var examModel = mapper.Map<ExamModel>(examViewModel);
                await examService.UpdateAsync(examModel);
                return RedirectToAction("Details", "Course", new { id = examViewModel.CourseId });
            }

            return View(examViewModel);
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleNames.Teacher},{RoleNames.Admin}")]
        public async Task<IActionResult> Result(Guid id)
        {
            var exam = await examService.GetByIdWithDetailsAsync(id);
            var viewModel = mapper.Map<ExamResultViewModel>(exam);
            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleNames.Teacher},{RoleNames.Admin}")]
        public async Task<IActionResult> Statistic(Guid id)
        {
            var exam = await examService.GetByIdWithDetailsAsync(id);
            var viewModel = mapper.Map<ExamStatisticViewModel>(exam);
            return View(viewModel);
        }
    }
}