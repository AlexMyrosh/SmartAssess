using AutoMapper;
using Business_Logic_Layer.Models;
using Business_Logic_Layer.Services.Implementations;
using Business_Logic_Layer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.ViewModels.ExamAssessment;

namespace Presentation_Layer.Controllers
{
    [Authorize]
    public class ExamAssessmentController(
        ICourseService courseService,
        IUserExamPassService userExamPassService,
        IOpenAiService openAiService,
        IMapper mapper)
        : Controller
    {
        [HttpGet]
        public async Task<IActionResult> MyPassedExams()
        {
            var courseModels = await courseService.GetAllWithTakenUserExamsAsync(User);
            var viewModel = mapper.Map<MyPassedExamsViewModel>(courseModels);
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
            var examAttemptModel = await userExamPassService.GetStartedAttemptAsync(examId, User);
            var viewModel = mapper.Map<TakeExamViewModel>(examAttemptModel);
            return View(viewModel);
        }

        [HttpPost]
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
