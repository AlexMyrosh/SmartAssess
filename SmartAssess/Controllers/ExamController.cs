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
        public IActionResult Create()
        {
            var examViewModel = new ExamViewModel
            {
                Questions = new List<ExamQuestionViewModel>
                {
                    new()
                }
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