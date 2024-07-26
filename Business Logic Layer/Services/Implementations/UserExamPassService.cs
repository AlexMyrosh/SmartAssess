using AutoMapper;
using Business_Logic_Layer.Models;
using Business_Logic_Layer.Services.Interfaces;
using Data_Access_Layer.Models;
using Data_Access_Layer.UnitOfWork.Interfaces;

namespace Business_Logic_Layer.Services.Implementations
{
    public class UserExamPassService : IUserExamPassService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UserExamPassService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> CreateAsync(UserExamPassModel model)
        {
            var entity = _mapper.Map<UserExamAttemptEntity>(model);
            var createdItemId = await _unitOfWork.UserExamPassRepository.CreateAsync(entity);
            await _unitOfWork.SaveAsync();
            return createdItemId;
        }

        public async Task<IEnumerable<UserExamPassModel>> GetAllAsync(bool isDeleted = false)
        {
            var entities = await _unitOfWork.UserExamPassRepository.GetAllAsync(isDeleted);
            var models = _mapper.Map<IEnumerable<UserExamPassModel>>(entities);
            return models;
        }

        public async Task<IEnumerable<UserExamPassModel>> GetAllWithDetailsAsync(bool isDeleted = false)
        {
            var entities = await _unitOfWork.UserExamPassRepository.GetAllWithDetailsAsync(isDeleted);
            var models = _mapper.Map<IEnumerable<UserExamPassModel>>(entities);
            return models;
        }

        public async Task<UserExamPassModel?> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.UserExamPassRepository.GetByIdAsync(id);
            var model = _mapper.Map<UserExamPassModel>(entity);
            return model;
        }

        public async Task<UserExamPassModel?> GetByIdWithDetailsAsync(Guid id)
        {
            var entity = await _unitOfWork.UserExamPassRepository.GetByIdWithDetailsAsync(id);
            var model = _mapper.Map<UserExamPassModel>(entity);
            return model;
        }

        public async Task<Guid> UpdateAsync(UserExamPassModel model)
        {
            var examEntityFromDb = await _unitOfWork.UserExamPassRepository.GetByIdWithDetailsAsync(model.Id);
            examEntityFromDb!.Feedback = model.Feedback;
            for (var i = 0; i < model.UserAnswers.Count; i++)
            {
                examEntityFromDb.UserAnswers[i].Grade = model.UserAnswers[i].Grade;
            }

            await _unitOfWork.SaveAsync();
            return model.Id;
        }
    }
}