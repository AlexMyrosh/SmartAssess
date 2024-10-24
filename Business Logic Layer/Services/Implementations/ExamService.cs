using System.Security.Claims;
using AutoMapper;
using Business_Logic_Layer.Models;
using Business_Logic_Layer.Services.Interfaces;
using Data_Access_Layer.Models;
using Data_Access_Layer.Models.Enums;
using Data_Access_Layer.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Business_Logic_Layer.Services.Implementations
{
    public class ExamService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<UserEntity> userManager) : IExamService
    {
        public async Task<Guid> CreateAsync(ExamModel model)
        {
            var examEntity = mapper.Map<ExamEntity>(model);
            var createdExamId = await unitOfWork.ExamRepository.CreateAsync(examEntity);
            await unitOfWork.SaveAsync();
            return createdExamId;
        }

        public async Task<IEnumerable<ExamModel>> GetAllAsync(bool includeDeleted = false)
        {
            var examEntities = await unitOfWork.ExamRepository.GetAllAsync(includeDeleted);
            var examModels = mapper.Map<IEnumerable<ExamModel>>(examEntities);
            return examModels;
        }

        public async Task<IEnumerable<ExamModel>> GetAllAvailableExamsWithDetailsAsync(bool includeDeleted = false)
        {
            var currentDateTimeOffset = DateTimeOffset.UtcNow;
            var examEntities = await unitOfWork.ExamRepository.GetAllExamsByFilterWithDetailsAsync(exam => exam.ExamStartDateTime <= currentDateTimeOffset && exam.ExamEndDateTime > currentDateTimeOffset, includeDeleted);
            var examModels = mapper.Map<IEnumerable<ExamModel>>(examEntities);
            return examModels;
        }

        public async Task<List<ExamModel>> GetAllRemovedExamsAsync()
        {
            var examEntities = await unitOfWork.ExamRepository.GetAllRemovedAsync();
            var examModels = mapper.Map<List<ExamModel>>(examEntities);
            return examModels;
        }

        public async Task RestoreAsync(Guid examId)
        {
            var examEntity = await unitOfWork.ExamRepository.GetByIdAsync(examId, true);
            if (examEntity is not null)
            {
                examEntity.IsDeleted = false;
                examEntity.DeletedById = null;
                examEntity.DeletedOn = null;
                await unitOfWork.SaveAsync();
            }
        }

        public async Task<PaginationExamModel> GetAllDeletedBySearchQueryWithPaginationAsync(int pageSize, string searchQuery = "", int pageNumber = 1)
        {
            var paginationExamEntity = await unitOfWork.ExamRepository.GetAllDeletedByFilterWithPaginationAsync(exam => exam.Name.Contains(searchQuery), pageSize, pageNumber);
            var paginationExamModel = mapper.Map<PaginationExamModel>(paginationExamEntity);
            return paginationExamModel;
        }

        public async Task<IEnumerable<ExamModel>> GetAllWithDetailsAsync(bool includeDeleted = false)
        {
            var examEntities = await unitOfWork.ExamRepository.GetAllWithDetailsAsync(includeDeleted);
            var examModels = mapper.Map<IEnumerable<ExamModel>>(examEntities);
            return examModels;
        }

        public async Task<ExamModel?> GetByIdAsync(Guid id)
        {
            var examEntity = await unitOfWork.ExamRepository.GetByIdAsync(id);
            var examModel = mapper.Map<ExamModel>(examEntity);
            return examModel;
        }

        public async Task<ExamModel?> GetByIdWithDetailsAsync(Guid id)
        {
            var examEntity = await unitOfWork.ExamRepository.GetByIdWithDetailsAsync(id);
            var examModel = mapper.Map<ExamModel>(examEntity);
            return examModel;
        }

        public async Task<bool> HardDeleteAsync(Guid id)
        {
            var result = await unitOfWork.ExamRepository.HardDeleteAsync(id);
            await unitOfWork.SaveAsync();
            return result;
        }

        public async Task<bool> SoftDeleteAsync(Guid id, ClaimsPrincipal deletedByUserClaimsPrincipal)
        {
            var deletedByUserId = userManager.GetUserId(deletedByUserClaimsPrincipal);
            if (!string.IsNullOrWhiteSpace(deletedByUserId))
            {
                var result = await unitOfWork.ExamRepository.SoftDeleteAsync(id, deletedByUserId);
                await unitOfWork.SaveAsync();
                return result;
            }

            return false;
        }

        public async Task<Guid> UpdateAsync(ExamModel model)
        {
            if (model.Id is null)
            {
                throw new ArgumentException("ExamModel id is null", nameof(model.Id));
            }

            var examEntityFromDb = await unitOfWork.ExamRepository.GetByIdWithDetailsAsync(model.Id.Value);
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

            await unitOfWork.SaveAsync();
            return model.Id.Value;
        }
    }
}