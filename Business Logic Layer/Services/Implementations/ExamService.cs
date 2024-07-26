using AutoMapper;
using Business_Logic_Layer.Models;
using Business_Logic_Layer.Services.Interfaces;
using Data_Access_Layer.Models;
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
            var currentDateTime = DateTime.Now;
            var examEntities = await _unitOfWork.ExamRepository.GetAllExamsByFilterWithDetailsAsync(exam => exam.ExamStartDateTime <= currentDateTime && exam.ExamEndDateTime > currentDateTime, includeDeleted);
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
            var examEntityFromDb = await _unitOfWork.ExamRepository.GetByIdAsync(model.Id);
            _mapper.Map(model, examEntityFromDb);
            await _unitOfWork.SaveAsync();
            return model.Id;
        }
    }
}