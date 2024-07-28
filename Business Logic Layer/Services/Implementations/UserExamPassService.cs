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

        public async Task<Guid> CreateAsync(UserExamAttemptModel model)
        {
            var userExamAttemptEntity = _mapper.Map<UserExamAttemptEntity>(model);
            var createdEntityId = await _unitOfWork.UserExamPassRepository.CreateAsync(userExamAttemptEntity);
            await _unitOfWork.SaveAsync();
            return createdEntityId;
        }

        public async Task<IEnumerable<UserExamAttemptModel>> GetAllAsync(bool includeDeleted = false)
        {
            var userExamAttemptEntities = await _unitOfWork.UserExamPassRepository.GetAllAsync(includeDeleted);
            var userExamAttemptModels = _mapper.Map<IEnumerable<UserExamAttemptModel>>(userExamAttemptEntities);
            return userExamAttemptModels;
        }

        public async Task<IEnumerable<UserExamAttemptModel>> GetAllWithDetailsAsync(bool includeDeleted = false)
        {
            var userExamAttemptEntities = await _unitOfWork.UserExamPassRepository.GetAllWithDetailsAsync(includeDeleted);
            var userExamAttemptModels = _mapper.Map<IEnumerable<UserExamAttemptModel>>(userExamAttemptEntities);
            return userExamAttemptModels;
        }

        public async Task<UserExamAttemptModel?> GetByIdAsync(Guid id)
        {
            var userExamAttemptEntity = await _unitOfWork.UserExamPassRepository.GetByIdAsync(id);
            var userExamAttemptModel = _mapper.Map<UserExamAttemptModel>(userExamAttemptEntity);
            return userExamAttemptModel;
        }

        public async Task<UserExamAttemptModel?> GetByIdWithDetailsAsync(Guid id)
        {
            var entity = await _unitOfWork.UserExamPassRepository.GetByIdWithDetailsAsync(id);
            var model = _mapper.Map<UserExamAttemptModel>(entity);
            return model;
        }

        public async Task<Guid> UpdateAsync(UserExamAttemptModel model)
        {
            if (model.Id is null)
            {
                throw new ArgumentException("UserExamAttemptModel id is null", nameof(model.Id));
            }

            if (model.UserAnswers is null)
            {
                throw new ArgumentException("UserExamAttemptModel UserAnswers is null", nameof(model.UserAnswers));
            }

            var userAttemptEntityFromDb = await _unitOfWork.UserExamPassRepository.GetByIdWithDetailsAsync(model.Id.Value);
            if (userAttemptEntityFromDb is null)
            {
                throw new ArgumentException("Unable to get UserExamAttemptEntity by id", nameof(model.Id));
            }

            // TODO: doesn't work with automapper, try again
            userAttemptEntityFromDb.Feedback = model.Feedback;
            for (var i = 0; i < model.UserAnswers.Count; i++)
            {
                userAttemptEntityFromDb.UserAnswers[i].Grade = model.UserAnswers[i].Grade;
                userAttemptEntityFromDb.UserAnswers[i].Feedback = model.UserAnswers[i].Feedback;
            }

            await _unitOfWork.SaveAsync();
            return model.Id.Value;
        }
    }
}