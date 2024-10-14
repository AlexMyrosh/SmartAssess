using Data_Access_Layer.Models;
using System.Linq.Expressions;

namespace Data_Access_Layer.Repositories.Interfaces
{
    public interface IExamRepository
    {
        Task<IEnumerable<ExamEntity>> GetAllAsync(bool includeDeleted = false);

        Task<IEnumerable<ExamEntity>> GetAllWithDetailsAsync(bool includeDeleted = false);

        Task<ExamEntity?> GetByIdAsync(Guid id);

        Task<ExamEntity?> GetByIdWithDetailsAsync(Guid id);

        Task<Guid> CreateAsync(ExamEntity entity);

        Task<bool> SoftDeleteAsync(Guid id);

        Task<bool> HardDeleteAsync(Guid id);

        public Task<IEnumerable<ExamEntity>> GetAllExamsByFilterWithDetailsAsync(Expression<Func<ExamEntity, bool>> filters, bool includeDeleted = false);

        Task<List<ExamEntity>> GetAllRemovedAsync();
    }
}