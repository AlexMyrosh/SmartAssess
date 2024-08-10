using Business_Logic_Layer.Models;
using System.Security.Claims;

namespace Business_Logic_Layer.Services.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseModel>> GetAllAsync(bool includeDeleted = false);

        Task<IEnumerable<CourseModel>> GetAllWithDetailsAsync(bool includeDeleted = false);

        Task<CourseModel?> GetByIdAsync(Guid id);

        Task<CourseModel?> GetByIdWithDetailsAsync(Guid id);

        Task<Guid> CreateAsync(CourseModel model);

        Task<bool> SoftDeleteAsync(Guid id);

        Task<bool> HardDeleteAsync(Guid id);

        Task<Guid> UpdateAsync(CourseModel model);

        Task<IEnumerable<CourseModel>> GetAllAvailableForUserCoursesWithDetailsAsync(ClaimsPrincipal userPrincipal, bool includeDeleted = false);
    }
}
