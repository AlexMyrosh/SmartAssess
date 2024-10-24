using Data_Access_Layer.Models;
using System.Linq.Expressions;

namespace Data_Access_Layer.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<UserEntity?> GetByIdAsync(string id);

        Task<UserEntity?> GetByEmailAsync(string email);

        Task<UserEntity?> GetByUsernameAsync(string username);

        Task<UserEntity?> GetByIdWithDetailsAsync(string id, bool canBeDeleted = false);

        Task<UserEntity?> GetByIdWithoutTrackingAsync(string id);

        Task SoftDeleteAsync(string id, string deletedByUserId);

        Task<List<UserEntity>> GetAllDeletedAsync();

        Task<PaginationUserEntity> GetAllDeletedByFilterWithPaginationAsync(Expression<Func<UserEntity, bool>> filter, int pageSize, int pageNumber = 1);

        Task<PaginationUserEntity> GetAllByFilterWithPaginationAsync(Expression<Func<UserEntity, bool>> filter, int pageSize, int pageNumber = 1);

        Task<List<UserEntity>> GetAllByFilterAsync(Expression<Func<UserEntity, bool>> filter, bool includeDeleted);
    }
}