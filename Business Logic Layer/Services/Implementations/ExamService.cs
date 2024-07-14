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

        public async Task<int> DeleteAsync(Guid id)
        {
            await _unitOfWork.ExamRepository.DeleteAsync(id);
            return await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ExamModel>> GetAllAsync()
        {
            var examEntities = await _unitOfWork.ExamRepository.GetAllAsync();
            var examModels = _mapper.Map<IEnumerable<ExamModel>>(examEntities);
            return examModels;
        }

        public async Task<ExamModel> GetByIdAsync(Guid id)
        {
            var examEntity = await _unitOfWork.ExamRepository.GetByIdAsync(id);
            var examModel = _mapper.Map<ExamModel>(examEntity);
            return examModel;
        }

        public async Task<Guid> UpdateAsync(ExamModel examModel)
        {
            var examEntity = _mapper.Map<ExamEntity>(examModel);
            var updatedEntityId = _unitOfWork.ExamRepository.Update(examEntity);
            await _unitOfWork.SaveAsync();
            return updatedEntityId;
        }
    }
}
