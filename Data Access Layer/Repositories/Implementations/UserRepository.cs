using Data_Access_Layer.Context;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repositories.Interfaces;

namespace Data_Access_Layer.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlContext _sqlContext;

        public UserRepository(SqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<UserEntity?> GetByIdAsync(string id)
        {
            var userEntity = await _sqlContext.Set<UserEntity>().FindAsync(id);
            return userEntity;
        }
    }
}