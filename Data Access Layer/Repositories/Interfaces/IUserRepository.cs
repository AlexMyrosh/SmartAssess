using Data_Access_Layer.Models;

namespace Data_Access_Layer.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<UserEntity?> GetByIdAsync(string id);

        Task<UserEntity?> GetByIdWithDetailsAsync(string id);

        Task<UserEntity?> GetByIdWithoutTrackingAsync(string id);

        Task SoftDeleteAsync(string id);
    }
}