using Data_Access_Layer.Models;
using System.Linq.Expressions;

namespace Data_Access_Layer.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<UserEntity?> GetByIdAsync(string id);

        Task<UserEntity?> GetByIdWithDetailsAsync(string id);

        Task<UserEntity?> GetByIdWithoutTrackingAsync(string id);

        Task SoftDeleteAsync(string id, string deletedByUserId);

        Task<List<UserEntity>> GetAllRemovedAsync();

        Task<PaginationUserEntity> GetAllDeletedByFilterWithPaginationAsync(Expression<Func<UserEntity, bool>> filter, int pageSize, int pageNumber = 1);

        Task<PaginationUserEntity> GetAllByFilterWithPaginationAsync(Expression<Func<UserEntity, bool>> filter, int pageSize, int pageNumber = 1);
    }
}