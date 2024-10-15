using Data_Access_Layer.Models;

namespace Data_Access_Layer.Repositories.Interfaces
{
    public interface IUserExamPassRepository
    {
        Task<Guid> CreateAsync(UserExamAttemptEntity entity);

        void Update(UserExamAttemptEntity entity);

        Task<IEnumerable<UserExamAttemptEntity>> GetAllAsync();

        Task<IEnumerable<UserExamAttemptEntity>> GetAllWithDetailsAsync();

        Task<UserExamAttemptEntity?> GetByIdAsync(Guid id);

        Task<UserExamAttemptEntity?> GetByIdWithDetailsAsync(Guid id);

        Task<bool> HardDeleteAsync(Guid id);

        Task<UserExamAttemptEntity?> GetStartedAttemptAsync(Guid examId, string userId);
    }
}