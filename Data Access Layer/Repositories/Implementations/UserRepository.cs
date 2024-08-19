using Data_Access_Layer.Context;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<UserEntity?> GetByIdWithDetailsAsync(string id)
        {
            var userEntity = await _sqlContext.Set<UserEntity>()
                .Include(exam => exam.Courses)
                .Include(exam => exam.UserExamAttempts)
                .FirstOrDefaultAsync();

            return userEntity;
        }
    }
}