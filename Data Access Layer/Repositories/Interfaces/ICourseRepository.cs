using Data_Access_Layer.Models;
using System.Linq.Expressions;

namespace Data_Access_Layer.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        Task<List<CourseEntity>> GetAllAsync(bool includeDeleted = false);

        Task<List<CourseEntity>> GetAllWithDetailsAsync(bool includeDeleted = false);

        Task<CourseEntity?> GetByIdAsync(Guid id);

        Task<CourseEntity?> GetByIdWithDetailsAsync(Guid id);

        Task<Guid> CreateAsync(CourseEntity entity);

        Task<bool> SoftDeleteAsync(Guid id, string deletedByUserId);

        Task<bool> HardDeleteAsync(Guid id);

        Task<List<CourseEntity>> GetAllWithDetailsByFilterAsync(Expression<Func<CourseEntity, bool>> filter, bool includeDeleted = false);

        Task<PaginationCourseEntity> GetAllDeletedByFilterWithPaginationAsync(Expression<Func<CourseEntity, bool>> filter, int pageSize, int pageNumber = 1);

        Task<PaginationCourseEntity> GetAllByFilterWithPaginationAsync(Expression<Func<CourseEntity, bool>> filter, int pageSize, int pageNumber = 1, bool includeDeleted = false);

        Task<List<CourseEntity>> GetAllByFilterAsync(Expression<Func<CourseEntity, bool>> filter, string userId, bool includeDeleted = false);

        Task<List<CourseEntity>> GetAllRemovedAsync();
    }
}
