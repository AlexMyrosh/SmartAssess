using Data_Access_Layer.Models;

namespace Data_Access_Layer.Repositories.Interfaces
{
    public interface IUserExamPassRepository
    {
        Task<Guid> CreateAsync(UserExamPassEntity entity);

        Task<IEnumerable<UserExamPassEntity>> GetAllAsync(bool isDeleted = false);

        Task<IEnumerable<UserExamPassEntity>> GetAllWithDetailsAsync(bool isDeleted = false);

        Task<UserExamPassEntity?> GetByIdAsync(Guid id);

        Task<UserExamPassEntity?> GetByIdWithDetailsAsync(Guid id);
    }
}