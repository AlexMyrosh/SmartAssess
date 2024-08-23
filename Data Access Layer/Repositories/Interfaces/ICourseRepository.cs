using Data_Access_Layer.Models;
using System.Linq.Expressions;

namespace Data_Access_Layer.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        Task<IEnumerable<CourseEntity>> GetAllAsync(bool includeDeleted = false);

        Task<IEnumerable<CourseEntity>> GetAllWithDetailsAsync(bool includeDeleted = false);

        Task<CourseEntity?> GetByIdAsync(Guid id);

        Task<CourseEntity?> GetByIdWithDetailsAsync(Guid id);

        Task<Guid> CreateAsync(CourseEntity entity);

        Task<bool> SoftDeleteAsync(Guid id);

        Task<bool> HardDeleteAsync(Guid id);

        public Task<IEnumerable<CourseEntity>> GetAllWithDetailsByFilterAsync(Expression<Func<CourseEntity, bool>> filter, bool includeDeleted = false);

        public Task<PaginationCourseEntity> GetAllByFilterWithPaginationAsync(Expression<Func<CourseEntity, bool>> filter, int pageSize, int pageNumber = 1, bool includeDeleted = false);
    }
}
