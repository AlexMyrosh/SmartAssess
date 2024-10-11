using Business_Logic_Layer.Models;
using System.Security.Claims;

namespace Business_Logic_Layer.Services.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseModel>> GetAllAsync(bool includeDeleted = false);

        Task<IEnumerable<CourseModel>> GetAllWithDetailsAsync(bool includeDeleted = false);

        Task<IEnumerable<CourseModel>> GetAllWithTakenUserExamsAsync(ClaimsPrincipal userPrincipal, bool includeDeleted = false);

        Task<CourseModel?> GetByIdAsync(Guid id);

        Task<CourseModel?> GetByIdWithDetailsAsync(Guid id, ClaimsPrincipal currentUserPrincipal);

        Task<Guid> CreateAsync(CourseModel model, string createdByTeacherId);

        Task<bool> SoftDeleteAsync(Guid id);

        Task<bool> HardDeleteAsync(Guid id);

        Task<Guid> UpdateAsync(CourseModel model);

        Task<PaginationCourseModel> GetAllAppliedByUserBySearchQueryWithPaginationAsync(ClaimsPrincipal userPrincipal, int pageSize, string searchQuery = "", int pageNumber = 1, bool includeDeleted = false);

        Task<PaginationCourseModel> GetAllBySearchQueryWithPaginationAsync(int pageSize, string searchQuery = "", int pageNumber = 1, bool includeDeleted = false);

        Task<PaginationCourseModel> GetAllAppliedByTeacherBySearchQueryWithPaginationAsync(ClaimsPrincipal userPrincipal, int pageSize, string searchQuery = "", int pageNumber = 1, bool includeDeleted = false);

        Task AddUserForCourseAsync(ClaimsPrincipal userPrincipal, Guid courseId);

        Task AddTeacherForCourseAsync(ClaimsPrincipal userPrincipal, Guid courseId);
        
        Task RemoveUserFromCourseAsync(ClaimsPrincipal userPrincipal, Guid courseId);

        Task RemoveTeacherFromCourseAsync(ClaimsPrincipal userPrincipal, Guid courseId);

        Task<Guid> UpdateLongDescriptionAsync(Guid courseId, string newLongDescription);

        Task RemoveUserFromCourseAsync(string userId, Guid courseId);
    }
}
