using Data_Access_Layer.Models;

namespace Data_Access_Layer.Repositories.Interfaces
{
    public interface IExamRepository
    {
        Task<IEnumerable<ExamEntity>> GetAllAsync(bool isDeleted = false);

        Task<ExamEntity?> GetByIdAsync(Guid id);

        Task<Guid> CreateAsync(ExamEntity model);

        Task<bool> DeleteAsync(Guid id);

        Guid Update(ExamEntity model);
    }
}