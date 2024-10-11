using Data_Access_Layer.Models;

namespace Data_Access_Layer.Repositories.Interfaces
{
    public interface IUserExamPassRepository
    {
        Task<Guid> CreateAsync(UserExamAttemptEntity entity);

        void Update(UserExamAttemptEntity entity);

        Task<IEnumerable<UserExamAttemptEntity>> GetAllAsync(bool includeDeleted = false);

        Task<IEnumerable<UserExamAttemptEntity>> GetAllWithDetailsAsync(bool includeDeleted = false);

        Task<UserExamAttemptEntity?> GetByIdAsync(Guid id);

        Task<UserExamAttemptEntity?> GetByIdWithDetailsAsync(Guid id);

        Task<bool> SoftDeleteAsync(Guid id);

        Task<bool> HardDeleteAsync(Guid id);

        Task<UserExamAttemptEntity?> GetStartedAttemptAsync(Guid examId, string userId);
    }
}