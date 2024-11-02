using AutoMapper;
using Business_Logic_Layer.Models;
using Business_Logic_Layer.Services.Interfaces;
using Data_Access_Layer.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.ViewModels.ExamAssessment;

namespace Presentation_Layer.Controllers
{
    public class ExamAssessmentController(
        ICourseService courseService,
        IUserExamPassService userExamPassService,
        IOpenAiService openAiService,
        IMapper mapper)
        : Controller
    {
        [HttpGet]
        [Authorize(Roles = $"{RoleNames.Student}")]
        public async Task<IActionResult> MyPassedExams()
        {
            var courseModels = await courseService.GetAllWithTakenUserExamsAsync(User);
            var viewModel = mapper.Map<MyPassedExamsViewModel>(courseModels);
            return View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> TakenExamDetails(Guid id)
        {
            var examPassModel = await userExamPassService.GetByIdWithDetailsAsync(id);
            var viewModel = mapper.Map<TakenExamDetailsViewModel>(examPassModel);
            viewModel.GeneralFeedback = viewModel.GeneralFeedback?.Trim();
            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleNames.Teacher},{RoleNames.Admin}")]
        public async Task<IActionResult> ManualEvaluation(Guid examId)
        {
            var examPassModel = await userExamPassService.GetByIdWithDetailsAsync(examId);
            var viewModel = mapper.Map<ExamManualEvaluationViewModel>(examPassModel);
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleNames.Teacher},{RoleNames.Admin}")]
        public async Task<IActionResult> ManualEvaluation(ExamManualEvaluationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = mapper.Map<UserExamAttemptModel>(viewModel);
                model.IsExamAssessed = true;
                await userExamPassService.UpdateAfterEvaluationAsync(model);
                return RedirectToAction("Result", "Exam", new { id = viewModel.ExamId });
            }

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleNames.Student}")]
        public async Task<IActionResult> TakeExam(Guid examId)
        {
            var examAttemptModel = await userExamPassService.GetStartedAttemptAsync(examId, User);
            var viewModel = mapper.Map<TakeExamViewModel>(examAttemptModel);
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleNames.Student}")]
        public async Task<IActionResult> TakeExam(TakeExamViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = mapper.Map<UserExamAttemptModel>(viewModel);
                await userExamPassService.CompleteAttemptAsync(model);
                return RedirectToAction("TakenExamDetails", new { id = viewModel.AttemptId });
            }

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleNames.Student}")]
        public async Task SaveAssessmentProgress(TakeExamViewModel viewModel)
        {
            var model = mapper.Map<UserExamAttemptModel>(viewModel);
            await userExamPassService.SaveIntermediateResultAsync(model);
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleNames.Teacher},{RoleNames.Admin}")]
        public async Task AiEvaluation(Guid userExamAttemptId)
        { 
            await openAiService.ExamEvaluationAsync(userExamAttemptId);
        }
    }
}
