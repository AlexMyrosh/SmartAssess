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

        public async Task<Guid> CreateAsync(ExamModel examModel)
        {
            var examEntity = _mapper.Map<ExamEntity>(examModel);
            var createdExamId = await _unitOfWork.ExamRepository.CreateAsync(examEntity);
            await _unitOfWork.SaveAsync();
            return createdExamId;
        }

        public async Task<IEnumerable<ExamModel>> GetAllAsync(bool isDeleted = false)
        {
            var examEntities = await _unitOfWork.ExamRepository.GetAllAsync(isDeleted);
            var examModels = _mapper.Map<IEnumerable<ExamModel>>(examEntities);
            return examModels;
        }

        public async Task<IEnumerable<ExamModel>> GetAllAvailableExamsWithDetailsAsync()
        {
            var currentDateTime = DateTime.Now;
            var examEntities = await _unitOfWork.ExamRepository.GetAllExamsByFilterWithDetailsAsync(exam => exam.ExamStartDateTime <= currentDateTime && exam.ExamEndDateTime > currentDateTime);
            var examModels = _mapper.Map<IEnumerable<ExamModel>>(examEntities);
            return examModels;
        }

        public async Task<IEnumerable<ExamModel>> GetAllWithDetailsAsync(bool isDeleted = false)
        {
            var examEntities = await _unitOfWork.ExamRepository.GetAllWithDetailsAsync(isDeleted);
            var examModels = _mapper.Map<IEnumerable<ExamModel>>(examEntities);
            return examModels;
        }

        public async Task<ExamModel> GetByIdAsync(Guid id)
        {
            var examEntity = await _unitOfWork.ExamRepository.GetByIdAsync(id);
            var examModel = _mapper.Map<ExamModel>(examEntity);
            return examModel;
        }

        public async Task<ExamModel?> GetByIdWithDetailsAsync(Guid id)
        {
            var examEntity = await _unitOfWork.ExamRepository.GetByIdWithDetailsAsync(id);
            var examModel = _mapper.Map<ExamModel>(examEntity);
            if (DateTime.Now + examModel.ExamDuration > examModel.ExamEndDateTime)
            {
                examModel.ExamDuration = examModel.ExamEndDateTime - DateTime.Now;
            }

            return examModel;
        }

        public async Task<bool> HardDeleteAsync(Guid id)
        {
            var result = await _unitOfWork.ExamRepository.HardDeleteAsync(id);
            await _unitOfWork.SaveAsync();
            return result;
        }

        public async Task HardDeleteAsync(ExamModel entity)
        {
            var examEntity = _mapper.Map<ExamEntity>(entity);
            await _unitOfWork.ExamRepository.HardDeleteAsync(examEntity.Id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            var result = await _unitOfWork.ExamRepository.SoftDeleteAsync(id);
            await _unitOfWork.SaveAsync();
            return result;
        }

        public async Task<Guid> UpdateAsync(ExamModel examModel)
        {
            var examEntityFromDb = await _unitOfWork.ExamRepository.GetByIdAsync(examModel.Id);
            _mapper.Map(examModel, examEntityFromDb);
            await _unitOfWork.SaveAsync();
            return examModel.Id;
        }
    }
}