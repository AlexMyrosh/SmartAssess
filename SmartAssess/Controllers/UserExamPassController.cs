using AutoMapper;
using Business_Logic_Layer.Models;
using Business_Logic_Layer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.ViewModels;

namespace Presentation_Layer.Controllers
{
    public class UserExamPassController : Controller
    {
        private readonly IExamService _examService;
        private readonly IUserExamPassService _userExamPassService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public UserExamPassController(IExamService examService, IUserExamPassService userExamPassService, IAccountService accountService, IMapper mapper)
        {
            _examService = examService;
            _userExamPassService = userExamPassService;
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var examPassModels = await _userExamPassService.GetAllWithDetailsAsync();
            var viewModel = _mapper.Map<IEnumerable<UserExamAttemptViewModel>>(examPassModels);
            return View(viewModel);
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
                var updateEntityId = await _userExamPassService.UpdateAsync(model);
                return RedirectToAction("Details", new { id = updateEntityId });
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Assessment(Guid examId)
        {
            var examModel = await _examService.GetByIdWithDetailsAsync(examId);
            var viewModel = _mapper.Map<UserExamAttemptViewModel>(examModel);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Assessment(UserExamAttemptViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = _mapper.Map<UserExamAttemptModel>(viewModel);
                model.User = await _accountService.GetUserAsync(User);
                var createdItemId = await _userExamPassService.CreateAsync(model);
                return RedirectToAction("Details", new { id = createdItemId });
            }

            return View(viewModel);
        }
    }
}
