using Data_Access_Layer.Models;
using System.Linq.Expressions;

namespace Data_Access_Layer.Repositories.Interfaces
{
    public interface IExamRepository
    {
        Task<List<ExamEntity>> GetAllAsync(bool includeDeleted = false);

        Task<List<ExamEntity>> GetAllWithDetailsAsync(bool includeDeleted = false);

        Task<ExamEntity?> GetByIdAsync(Guid id, bool canBeDeleted = false);

        Task<ExamEntity?> GetByIdWithDetailsAsync(Guid id);

        Task<Guid> CreateAsync(ExamEntity entity);

        Task<bool> SoftDeleteAsync(Guid id, string deletedByUserId);

        Task<bool> HardDeleteAsync(Guid id);

        Task<List<ExamEntity>> GetAllExamsByFilterWithDetailsAsync(Expression<Func<ExamEntity, bool>> filters, bool includeDeleted = false);

        Task<List<ExamEntity>> GetAllRemovedAsync();

        Task<PaginationExamEntity> GetAllDeletedByFilterWithPaginationAsync(Expression<Func<ExamEntity, bool>> filter, int pageSize, int pageNumber = 1);
    }
}