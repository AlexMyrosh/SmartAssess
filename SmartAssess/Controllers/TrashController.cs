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
        public async Task<IActionResult> Index()
        {
            var deletedCourses = await _courseService.GetAllRemovedCoursesAsync();
            var deletedExams = await _examService.GetAllRemovedExamsAsync();
            var deletedUsers = await _accountService.GetAllRemovedUsersAsync();
            var viewModel = new TrashViewModel
            {
                DeletedCourses = _mapper.Map<List<CourseViewModel>>(deletedCourses),
                DeletedExams = _mapper.Map<List<ExamViewModel>>(deletedExams),
                DeletedUsers = _mapper.Map<List<UserViewModel>>(deletedUsers)
            };

            return View(viewModel);
        }
    }
}
