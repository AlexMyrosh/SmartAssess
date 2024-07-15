using Data_Access_Layer.Models;

namespace Data_Access_Layer.Repositories.Interfaces
{
    public interface IExamRepository
    {
        Task<IEnumerable<ExamEntity>> GetAllAsync(bool isDeleted = false);

        Task<IEnumerable<ExamEntity>> GetAllWithDetailsAsync(bool isDeleted = false);

        Task<ExamEntity?> GetByIdAsync(Guid id);

        Task<ExamEntity?> GetByIdWithDetailsAsync(Guid id);

        Task<Guid> CreateAsync(ExamEntity model);

        Task<bool> SoftDeleteAsync(Guid id);

        Task<bool> HardDeleteAsync(Guid id);

        void HardDelete(ExamEntity entity);

        Guid Update(ExamEntity model);
    }
}