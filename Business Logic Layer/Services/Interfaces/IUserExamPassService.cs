using Business_Logic_Layer.Models;

namespace Business_Logic_Layer.Services.Interfaces
{
    public interface IUserExamPassService
    {
        Task<Guid> CreateAsync(UserExamPassModel model);

        Task<IEnumerable<UserExamPassModel>> GetAllAsync(bool isDeleted = false);

        Task<IEnumerable<UserExamPassModel>> GetAllWithDetailsAsync(bool isDeleted = false);

        Task<UserExamPassModel?> GetByIdAsync(Guid id);

        Task<UserExamPassModel?> GetByIdWithDetailsAsync(Guid id);

        Task<Guid> UpdateAsync(UserExamPassModel model);
    }
}
