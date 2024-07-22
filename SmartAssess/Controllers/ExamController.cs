using AutoMapper;
using Business_Logic_Layer.Models;
using Business_Logic_Layer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.ViewModels;

namespace Presentation_Layer.Controllers
{
    public class ExamController : Controller
    {
        private readonly IExamService _examService;
        private readonly IUserExamPassService _userExamPassService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public ExamController(IExamService examService, IUserExamPassService userExamPassService, IAccountService accountService, IMapper mapper)
        {
            _examService = examService;
            _userExamPassService = userExamPassService;
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var examModels = await _examService.GetAllAvailableExamsWithDetailsAsync();
            var examViewModels = _mapper.Map<IEnumerable<ExamViewModel>>(examModels);
            return View(examViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            var examModel = await _examService.GetByIdWithDetailsAsync(id);
            var examViewModel = _mapper.Map<ExamViewModel>(examModel);
            return View(examViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExamViewModel examViewModel)
        {
            if (ModelState.IsValid)
            {
                var examModel = _mapper.Map<ExamModel>(examViewModel);
                var createdExamId = await _examService.CreateAsync(examModel);
                return RedirectToAction("GetById", new { id = createdExamId });
            }

            return View(examViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _examService.SoftDeleteAsync(id);
            return RedirectToAction("GetAll");
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
                return RedirectToAction("GetById", new { id = updatedExamId });
            }

            return View(examViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Pass(Guid examId)
        {
            var examModel = await _examService.GetByIdWithDetailsAsync(examId);
            var viewModel = _mapper.Map<UserExamPassViewModel>(examModel);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Pass(UserExamPassViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = _mapper.Map<UserExamPassModel>(viewModel);
                model.User = await _accountService.GetCurrentUserAsync(User);
                await _userExamPassService.CreateAsync(model);
                return RedirectToAction("GetAll");
            }


            return View(viewModel);
        }
    }
}