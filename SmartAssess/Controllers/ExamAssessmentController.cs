using AutoMapper;
using Business_Logic_Layer.Models;
using Business_Logic_Layer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.ViewModels.ExamAssessment;

namespace Presentation_Layer.Controllers
{
    [Authorize]
    public class ExamAssessmentController(
        IExamService examService,
        ICourseService courseService,
        IUserExamPassService userExamPassService,
        IOpenAiService openAiService,
        IAccountService accountService,
        IMapper mapper)
        : Controller
    {
        [HttpGet]
        public async Task<IActionResult> MyPassedExams()
        {
            var courseModels = await courseService.GetAllWithTakenUserExamsAsync(User);
            var viewModel = mapper.Map<MyPassedExamsViewModel>(courseModels);

            //var currentUser = await _accountService.GetUserAsync(User);
            //foreach (var courseViewModel in viewModel.TakenExamCourses)
            //{
            //    courseViewModel.Exams = courseViewModel.Exams.Where(x => x.UserExamAttempts.Count(x => x.User.Id == currentUser.Id) > 0).ToList();
            //    foreach (var exam in courseViewModel.Exams)
            //    {
            //        exam.UserExamAttempts = exam.UserExamAttempts.Where(s => s.User.Id == currentUser.Id).ToList();

            //        if (exam.UserExamAttempts.All(x => x.IsExamAssessed))
            //        {
            //            if (exam.FinalGradeCalculationMethod == FinalGradeCalculationMethodViewModel.Average)
            //            {
            //                exam.FinalExamGradeForCurrentStudent = exam.UserExamAttempts.Average(x => x.TotalGrade);
            //            }
            //            else if (exam.FinalGradeCalculationMethod == FinalGradeCalculationMethodViewModel.LastAttempt)
            //            {
            //                exam.FinalExamGradeForCurrentStudent = exam.UserExamAttempts.FirstOrDefault(x =>
            //                    x.AttemptStarterAt == exam.UserExamAttempts.Max(y => y.AttemptStarterAt)).TotalGrade;
            //            }
            //            else
            //            {
            //                exam.FinalExamGradeForCurrentStudent = exam.UserExamAttempts.Max(x => x.TotalGrade);
            //            }
            //        }
            //    }
            //}

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> TakenExamDetails(Guid id)
        {
            var examPassModel = await userExamPassService.GetByIdWithDetailsAsync(id);
            var viewModel = mapper.Map<TakenExamDetailsViewModel>(examPassModel);
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ManualEvaluation(Guid examId)
        {
            var examPassModel = await userExamPassService.GetByIdWithDetailsAsync(examId);
            var viewModel = mapper.Map<ExamManualEvaluationViewModel>(examPassModel);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ManualEvaluation(ExamManualEvaluationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = mapper.Map<UserExamAttemptModel>(viewModel);
                model.IsExamAssessed = true;
                await userExamPassService.UpdateAsync(model);
                return RedirectToAction("Result", "Exam", new { id = viewModel.ExamId });
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> TakeExam(Guid examId)
        {
            var examModel = await examService.GetByIdWithDetailsAsync(examId);
            var viewModel = mapper.Map<TakeExamViewModel>(examModel);

            //var currentUser = await _accountService.GetUserAsync(User);
            //var examAttemptModel = _mapper.Map<UserExamAttemptModel>(viewModel);
            //examAttemptModel.User = currentUser;
            //var createdAttemptId = await _userExamPassService.CreateAsync(examAttemptModel);
            //viewModel.Id = createdAttemptId;

            //if (viewModel.Exam.ExamEndDateTime.Value.UtcDateTime - DateTime.UtcNow < viewModel.Exam.ExamDuration)
            //{
            //    viewModel.Exam.ExamDuration = viewModel.Exam.ExamEndDateTime.Value.UtcDateTime - DateTime.UtcNow;
            //}

            //viewModel.Exam.CurrentUserAttemptNumber = viewModel.Exam.UserExamAttempts.Count(x => x.User.Id == currentUser.Id);
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ContinueAssessments(Guid startedAttemptId)
        {
            var model = await userExamPassService.GetByIdWithDetailsAsync(startedAttemptId);
            var viewModel = mapper.Map<TakeExamViewModel>(model);

            //viewModel.TakenTimeToComplete = DateTimeOffset.Now - viewModel.AttemptStarterAt;

            return View("TakeExam", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> TakeExam(TakeExamViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = mapper.Map<UserExamAttemptModel>(viewModel);
                model.User = await accountService.GetUserAsync(User);
                await userExamPassService.CompleteAttemptAsync(model);
                return RedirectToAction("TakenExamDetails", new { id = viewModel.AttemptId });
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task SaveAssessmentProgress(TakeExamViewModel viewModel)
        {
            var model = mapper.Map<UserExamAttemptModel>(viewModel);
            await userExamPassService.SaveIntermediateResultAsync(model);
        }

        [HttpPost]
        public async Task AiEvaluation(Guid userExamAttemptId)
        {
            await openAiService.ExamEvaluationAsync(userExamAttemptId);
        }
    }
}
