using AutoMapper;
using Business_Logic_Layer.Models;
using Business_Logic_Layer.Services.Interfaces;
using Data_Access_Layer.Models;
using Data_Access_Layer.Models.Enums;
using Data_Access_Layer.UnitOfWork.Interfaces;

namespace Business_Logic_Layer.Services.Implementations
{
    public class ExamService : IExamService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ExamService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> CreateAsync(ExamModel model)
        {
            var examEntity = _mapper.Map<ExamEntity>(model);
            var createdExamId = await _unitOfWork.ExamRepository.CreateAsync(examEntity);
            await _unitOfWork.SaveAsync();
            return createdExamId;
        }

        public async Task<IEnumerable<ExamModel>> GetAllAsync(bool includeDeleted = false)
        {
            var examEntities = await _unitOfWork.ExamRepository.GetAllAsync(includeDeleted);
            var examModels = _mapper.Map<IEnumerable<ExamModel>>(examEntities);
            return examModels;
        }

        public async Task<IEnumerable<ExamModel>> GetAllAvailableExamsWithDetailsAsync(bool includeDeleted = false)
        {
            var currentDateTimeOffset = DateTimeOffset.UtcNow;
            var examEntities = await _unitOfWork.ExamRepository.GetAllExamsByFilterWithDetailsAsync(exam => exam.ExamStartDateTime <= currentDateTimeOffset && exam.ExamEndDateTime > currentDateTimeOffset, includeDeleted);
            var examModels = _mapper.Map<IEnumerable<ExamModel>>(examEntities);
            return examModels;
        }

        public async Task<IEnumerable<ExamModel>> GetAllWithDetailsAsync(bool includeDeleted = false)
        {
            var examEntities = await _unitOfWork.ExamRepository.GetAllWithDetailsAsync(includeDeleted);
            var examModels = _mapper.Map<IEnumerable<ExamModel>>(examEntities);
            return examModels;
        }

        public async Task<ExamModel?> GetByIdAsync(Guid id)
        {
            var examEntity = await _unitOfWork.ExamRepository.GetByIdAsync(id);
            var examModel = _mapper.Map<ExamModel>(examEntity);
            return examModel;
        }

        public async Task<ExamModel?> GetByIdWithDetailsAsync(Guid id)
        {
            var examEntity = await _unitOfWork.ExamRepository.GetByIdWithDetailsAsync(id);
            var examModel = _mapper.Map<ExamModel>(examEntity);
            return examModel;
        }

        public async Task<bool> HardDeleteAsync(Guid id)
        {
            var result = await _unitOfWork.ExamRepository.HardDeleteAsync(id);
            await _unitOfWork.SaveAsync();
            return result;
        }

        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            var result = await _unitOfWork.ExamRepository.SoftDeleteAsync(id);
            await _unitOfWork.SaveAsync();
            return result;
        }

        public async Task<Guid> UpdateAsync(ExamModel model)
        {
            if (model.Id is null)
            {
                throw new ArgumentException("ExamModel id is null", nameof(model.Id));
            }

            var examEntityFromDb = await _unitOfWork.ExamRepository.GetByIdWithDetailsAsync(model.Id.Value);
            examEntityFromDb.Name = model.Name;
            examEntityFromDb.Description = model.Description;
            examEntityFromDb.ExamStartDateTime = model.ExamStartDateTime.Value;
            examEntityFromDb.ExamEndDateTime = model.ExamEndDateTime.Value;
            examEntityFromDb.ExamDuration = model.ExamDuration.Value;
            examEntityFromDb.MinimumPassGrade = model.MinimumPassGrade.Value;
            examEntityFromDb.MaxAttemptsAllowed = model.MaxAttemptsAllowed.Value;
            examEntityFromDb.FinalGradeCalculationMethod = (FinalGradeCalculationMethodEntity)model.FinalGradeCalculationMethod;

            for (int i = 0; i < model.Questions.Count; i++)
            {
                if (examEntityFromDb.Questions.Exists(q => q.Id == model.Questions[i].Id))
                {
                    examEntityFromDb.Questions.First(q=>q.Id == model.Questions[i].Id).QuestionText = model.Questions[i].QuestionText;
                    examEntityFromDb.Questions.First(q => q.Id == model.Questions[i].Id).MaxGrade = model.Questions[i].MaxGrade.Value;
                }
                else
                {
                    examEntityFromDb.Questions.Add(new ExamQuestionEntity
                    {
                        QuestionText = model.Questions[i].QuestionText,
                        MaxGrade = model.Questions[i].MaxGrade.Value,
                    });
                }
            }

            await _unitOfWork.SaveAsync();
            return model.Id.Value;
        }
    }
}