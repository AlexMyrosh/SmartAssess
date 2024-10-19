using System.Linq.Expressions;
using Data_Access_Layer.Context;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data_Access_Layer.Repositories.Implementations
{
    public class UserRepository(SqlContext sqlContext) : IUserRepository
    {
        public async Task<UserEntity?> GetByIdAsync(string id)
        {
            var userEntity = await sqlContext.Users
                .Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();

            return userEntity;
        }

        public async Task<UserEntity?> GetByEmailAsync(string email)
        {
            var userEntity = await sqlContext.Users
                .Where(x => !x.IsDeleted && x.Email == email)
                .FirstOrDefaultAsync();

            return userEntity;
        }

        public async Task<UserEntity?> GetByIdWithoutTrackingAsync(string id)
        {
            var userEntity = await sqlContext.Users
                .Where(x => !x.IsDeleted && x.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return userEntity;
        }

        public async Task<UserEntity?> GetByIdWithDetailsAsync(string id, bool canBeDeleted = false)
        {
            var userEntity = await sqlContext.Users
                .Where(x => (!x.IsDeleted || x.IsDeleted == canBeDeleted) && x.Id == id)
                .Include(exam => exam.Courses.Where(course => course.IsDeleted == false))
                .Include(exam => exam.UserExamAttempts)
                .Include(course => course.TeachingCourses.Where(teachingCourse => teachingCourse.IsDeleted == false))
                .FirstOrDefaultAsync();

            return userEntity;
        }

        public async Task SoftDeleteAsync(string id, string deletedByUserId)
        {
            var userEntity = await sqlContext.Users.FindAsync(id);
            if (userEntity != null)
            {
                userEntity.IsDeleted = true;
                userEntity.DeletedById = deletedByUserId;
                userEntity.DeletedOn = DateTimeOffset.Now;
            }
        }

        public async Task<List<UserEntity>> GetAllDeletedAsync()
        {
            var examEntities = await sqlContext.Users
                .Where(exam => exam.IsDeleted)
                .Include(exam => exam.DeletedBy)
                .ToListAsync();

            return examEntities;
        }

        public async Task<PaginationUserEntity> GetAllDeletedByFilterWithPaginationAsync(Expression<Func<UserEntity, bool>> filter, int pageSize, int pageNumber = 1)
        {
            var query = sqlContext.Users
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

        public async Task<PaginationUserEntity> GetAllByFilterWithPaginationAsync(Expression<Func<UserEntity, bool>> filter, int pageSize, int pageNumber = 1)
        {
            var query = sqlContext.Users
                .Where(course => !course.IsDeleted)
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

        public async Task<List<UserEntity>> GetAllByFilterAsync(Expression<Func<UserEntity, bool>> filter, bool includeDeleted)
        {
            var examEntities = await sqlContext.Users
                .Where(exam => exam.IsDeleted == false || exam.IsDeleted == includeDeleted)
                .Where(filter)
                .ToListAsync();

            return examEntities;
        }
    }
}