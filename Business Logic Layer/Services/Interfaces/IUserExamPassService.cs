using Business_Logic_Layer.Models;
using Business_Logic_Layer.Models.Enums;
using System.Security.Claims;

namespace Business_Logic_Layer.Services.Interfaces
{
    public interface IUserExamPassService
    {
        Task<Guid> CreateAsync(UserExamAttemptModel model);

        Task CompleteAttemptAsync(UserExamAttemptModel model);

        Task<IEnumerable<UserExamAttemptModel>> GetAllAsync();

        Task<IEnumerable<UserExamAttemptModel>> GetAllWithDetailsAsync();

        Task<UserExamAttemptModel?> GetByIdAsync(Guid id);

        Task<UserExamAttemptModel?> GetByIdWithDetailsAsync(Guid id);

        Task<Guid> UpdateAsync(UserExamAttemptModel model);

        Task<Guid> UpdateAfterEvaluationAsync(UserExamAttemptModel model);

        Task SetStatusAsync(Guid id, ExamAttemptStatusModel statusToSet);

        Task SaveIntermediateResultAsync(UserExamAttemptModel model);

        Task<UserExamAttemptModel> GetStartedAttemptAsync(Guid examId, ClaimsPrincipal userClaimsPrincipal);
    }
}
