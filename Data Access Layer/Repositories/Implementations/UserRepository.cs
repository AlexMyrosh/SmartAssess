using System.Linq.Expressions;
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

        public async Task<UserEntity?> GetByIdWithoutTrackingAsync(string id)
        {
            var userEntity = await _sqlContext.Set<UserEntity>().AsNoTracking().FirstOrDefaultAsync(user => user.Id == id);
            return userEntity;
        }

        public async Task<UserEntity?> GetByIdWithDetailsAsync(string id)
        {
            var userEntity = await _sqlContext.Set<UserEntity>()
                .Include(exam => exam.Courses.Where(course => course.IsDeleted == false))
                .Include(exam => exam.UserExamAttempts)
                .Include(course => course.TeachingCourses.Where(teachingCourse => teachingCourse.IsDeleted == false))
                .FirstOrDefaultAsync(user => user.Id == id);

            return userEntity;
        }

        public async Task SoftDeleteAsync(string id, string deletedByUserId)
        {
            var userEntity = await _sqlContext.Users.FindAsync(id);
            if (userEntity != null)
            {
                userEntity.IsDeleted = true;
                userEntity.DeletedById = deletedByUserId;
                userEntity.DeletedOn = DateTimeOffset.Now;
            }
        }

        public async Task<List<UserEntity>> GetAllRemovedAsync()
        {
            var examEntities = await _sqlContext.Users
                .Where(exam => exam.IsDeleted)
                .Include(exam => exam.DeletedBy)
                .ToListAsync();

            return examEntities;
        }

        public async Task<PaginationUserEntity> GetAllDeletedByFilterWithPaginationAsync(Expression<Func<UserEntity, bool>> filter, int pageSize, int pageNumber = 1)
        {
            var query = _sqlContext.Users
                .Where(course => course.IsDeleted)
                .Where(filter);

            var result = new PaginationUserEntity
            {
                TotalItems = await query.CountAsync()
            };

            var courseEntities = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            result.Items = courseEntities;
            return result;
        }
    }
}