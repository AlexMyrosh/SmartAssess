using AutoMapper;
using Business_Logic_Layer.Models;
using Business_Logic_Layer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.ViewModels.Enums;
using Presentation_Layer.ViewModels.Old;

namespace Presentation_Layer.Controllers
{
    [Authorize]
    public class ExamAssessmentController : Controller
    {
        private readonly IExamService _examService;
        private readonly ICourseService _courseService;
        private readonly IUserExamPassService _userExamPassService;
        private readonly IOpenAiService _openAiService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public ExamAssessmentController(IExamService examService, ICourseService courseService, IUserExamPassService userExamPassService, IOpenAiService openAiService, IAccountService accountService, IMapper mapper)
        {
            _examService = examService;
            _userExamPassService = userExamPassService;
            _openAiService = openAiService;
            _accountService = accountService;
            _mapper = mapper;
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> PassedByCurrentUserExams()
        {
            var courseModels = await _courseService.GetAllWithTakenUserExamsAsync(User);
            var courseViewModels = _mapper.Map<List<CourseViewModel>>(courseModels);

            var currentUser = await _accountService.GetUserAsync(User);
            foreach (var courseViewModel in courseViewModels)
            {
                courseViewModel.Exams = courseViewModel.Exams.Where(x => x.UserExamAttempts.Count(x => x.User.Id == currentUser.Id) > 0).ToList();
                foreach (var exam in courseViewModel.Exams)
                {
                    exam.UserExamAttempts = exam.UserExamAttempts.Where(s => s.User.Id == currentUser.Id).ToList();

                    if (exam.UserExamAttempts.All(x => x.IsExamAssessed))
                    {
                        if (exam.FinalGradeCalculationMethod == FinalGradeCalculationMethodViewModel.Average)
                        {
                            exam.FinalExamGradeForCurrentStudent = exam.UserExamAttempts.Average(x => x.TotalGrade);
                        }
                        else if (exam.FinalGradeCalculationMethod == FinalGradeCalculationMethodViewModel.LastAttempt)
                        {
                            exam.FinalExamGradeForCurrentStudent = exam.UserExamAttempts.FirstOrDefault(x =>
                                x.AttemptStarterAt == exam.UserExamAttempts.Max(y => y.AttemptStarterAt)).TotalGrade;
                        }
                        else
                        {
                            exam.FinalExamGradeForCurrentStudent = exam.UserExamAttempts.Max(x => x.TotalGrade);
                        }
                    }
                }
            }

            return View(courseViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var examPassModel = await _userExamPassService.GetByIdWithDetailsAsync(id);
            var viewModel = _mapper.Map<UserExamAttemptViewModel>(examPassModel);
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Evaluation(Guid examId)
        {
            var examPassModel = await _userExamPassService.GetByIdWithDetailsAsync(examId);
            var viewModel = _mapper.Map<UserExamAttemptViewModel>(examPassModel);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Evaluation(UserExamAttemptViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = _mapper.Map<UserExamAttemptModel>(viewModel);
                model.IsExamAssessed = true;
                await _userExamPassService.UpdateAsync(model);
                return RedirectToAction("UsersAnswers", "Exam", new { id = viewModel.Exam.Id });
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Assessment(Guid examId)
        {
            var examModel = await _examService.GetByIdWithDetailsAsync(examId);
            var viewModel = _mapper.Map<UserExamAttemptViewModel>(examModel);
            var currentUser = await _accountService.GetUserAsync(User);
            var examAttemptModel = _mapper.Map<UserExamAttemptModel>(viewModel);
            examAttemptModel.User = currentUser;
            var createdAttemptId = await _userExamPassService.CreateAsync(examAttemptModel);
            viewModel.Id = createdAttemptId;

            if (viewModel.Exam.ExamEndDateTime.Value.UtcDateTime - DateTime.UtcNow < viewModel.Exam.ExamDuration)
            {
                viewModel.Exam.ExamDuration = viewModel.Exam.ExamEndDateTime.Value.UtcDateTime - DateTime.UtcNow;
            }

            viewModel.Exam.CurrentUserAttmptNumber = viewModel.Exam.UserExamAttempts.Count(x => x.User.Id == currentUser.Id);
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ContinueAssessments(Guid startedAttemptId)
        {
            var model = await _userExamPassService.GetByIdWithDetailsAsync(startedAttemptId);
            var viewModel = _mapper.Map<UserExamAttemptViewModel>(model);

            viewModel.TakenTimeToComplete = DateTimeOffset.Now - viewModel.AttemptStarterAt;

            return View("Assessment", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Assessment(UserExamAttemptViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = _mapper.Map<UserExamAttemptModel>(viewModel);
                model.User = await _accountService.GetUserAsync(User);
                await _userExamPassService.CompleteAttemptAsync(model);
                return RedirectToAction("Details", new { id = viewModel.Id });
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task SaveAssessmentProgress(UserExamAttemptViewModel viewModel)
        {
            var model = _mapper.Map<UserExamAttemptModel>(viewModel);
            await _userExamPassService.SaveIntermediateResultAsync(model);
        }

        [HttpPost]
        public async Task AiEvaluation(Guid userExamAttemptId)
        {
            await _openAiService.ExamEvaluationAsync(userExamAttemptId);
        }
    }
}
