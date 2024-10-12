using System.Security.Claims;
using AutoMapper;
using Business_Logic_Layer.Models;
using Business_Logic_Layer.Models.Enums;
using Business_Logic_Layer.Services.Interfaces;
using Data_Access_Layer.Models;
using Data_Access_Layer.Models.Enums;
using Data_Access_Layer.UnitOfWork.Interfaces;

namespace Business_Logic_Layer.Services.Implementations
{
    public class UserExamPassService : IUserExamPassService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;

        public UserExamPassService(IUnitOfWork unitOfWork, IMapper mapper, IAccountService accountService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accountService = accountService;
        }

        public async Task<Guid> CreateAsync(UserExamAttemptModel model)
        {
            var userExamAttemptEntity = _mapper.Map<UserExamAttemptEntity>(model);
            var createdEntityId = await _unitOfWork.UserExamPassRepository.CreateAsync(userExamAttemptEntity);
            await _unitOfWork.SaveAsync();
            return createdEntityId;
        }

        public async Task CompleteAttemptAsync(UserExamAttemptModel model)
        {
            var entityFromDb = await _unitOfWork.UserExamPassRepository.GetByIdWithDetailsAsync(model.Id.Value);
            entityFromDb.Status = ExamAttemptStatusEntity.Completed;
            entityFromDb.TakenTimeToComplete = DateTime.UtcNow - entityFromDb.AttemptStarterAt.UtcDateTime;
            foreach (var userAnswer in entityFromDb.UserAnswers)
            {
                userAnswer.AnswerText = model.UserAnswers.FirstOrDefault(x => x.Id == userAnswer.Id).AnswerText;
            }

            await _unitOfWork.SaveAsync();
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

        public async Task SetStatusAsync(Guid id, ExamAttemptStatusModel statusToSet)
        {
            var entity = await _unitOfWork.UserExamPassRepository.GetByIdAsync(id);
            entity.Status = (ExamAttemptStatusEntity)statusToSet;
            await _unitOfWork.SaveAsync();
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

            userAttemptEntityFromDb.IsExamAssessed = model.IsExamAssessed;
            userAttemptEntityFromDb.IsAssessedByAi = model.IsAssessedByAi;
            userAttemptEntityFromDb.Status = (ExamAttemptStatusEntity)model.Status;

            await _unitOfWork.SaveAsync();
            return model.Id.Value;
        }

        public async Task SaveIntermediateResultAsync(UserExamAttemptModel model)
        {
            if (model.Id is null || model.Id == Guid.Empty)
            {
                return;
            }

            var entityFromDb = await _unitOfWork.UserExamPassRepository.GetByIdWithDetailsAsync(model.Id.Value);
            foreach (var userAnswer in entityFromDb.UserAnswers)
            {
                userAnswer.AnswerText = model.UserAnswers.FirstOrDefault(x => x.Id == userAnswer.Id).AnswerText;
            }
            
            await _unitOfWork.SaveAsync();
        }

        public async Task<UserExamAttemptModel> GetStartedAttemptAsync(Guid examId, ClaimsPrincipal userClaimsPrincipal)
        {
            var userId = _accountService.GetUserId(userClaimsPrincipal);
            var userAttempt = await _unitOfWork.UserExamPassRepository.GetStartedAttemptAsync(examId, userId);
            UserExamAttemptModel model;
            if (userAttempt is null)
            {
                var examEntity = await _unitOfWork.ExamRepository.GetByIdWithDetailsAsync(examId);
                model = new UserExamAttemptModel
                {
                    AttemptStarterAt = DateTimeOffset.Now,
                    Status = ExamAttemptStatusModel.InProgress,
                    User = new UserModel
                    {
                        Id = userId
                    },
                    Exam = new ExamModel
                    {
                        Id = examId
                    },
                    UserAnswers = new List<UserAnswerModel>(examEntity.Questions.Count)
                };

                foreach (var question in examEntity.Questions)
                {
                    model.UserAnswers.Add(new UserAnswerModel
                    {
                        QuestionId = question.Id
                    });
                }

                var createdAttemptEntityId = await CreateAsync(model);
                userAttempt = await _unitOfWork.UserExamPassRepository.GetByIdWithDetailsAsync(createdAttemptEntityId);
            }

            model = _mapper.Map<UserExamAttemptModel>(userAttempt);
            model.Exam.UserAttemptCount = model.Exam.UserExamAttempts.Count(x => x.User.Id == userId);
            return model;
        }

        public async Task<Guid> UpdateAfterEvaluationAsync(UserExamAttemptModel model)
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

            userAttemptEntityFromDb.IsExamAssessed = true;
            userAttemptEntityFromDb.Status = ExamAttemptStatusEntity.Completed;

            await _unitOfWork.SaveAsync();
            return model.Id.Value;
        }
    }
}