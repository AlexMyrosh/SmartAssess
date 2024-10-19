using Data_Access_Layer.Context;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data_Access_Layer.Repositories.Implementations
{
    public class CourseRepository(SqlContext sqlContext) : ICourseRepository
    {
        public async Task<Guid> CreateAsync(CourseEntity entity)
        {
            var entityEntry = await sqlContext.Courses.AddAsync(entity);
            return entityEntry.Entity.Id;
        }

        public async Task<List<CourseEntity>> GetAllAsync(bool includeDeleted = false)
        {
            var courseEntities = await sqlContext.Courses
                .Where(course => course.IsDeleted == false || course.IsDeleted == includeDeleted)
                .ToListAsync();

            return courseEntities;
        }

        public async Task<List<CourseEntity>> GetAllByFilterAsync(Expression<Func<CourseEntity, bool>> filter, string userId, bool includeDeleted = false)
        {
            var courseEntities = await sqlContext
                .Courses
                .Where(course => course.IsDeleted == false || course.IsDeleted == includeDeleted)
                .Where(filter)
                .Include(course => course.Exams)
                .ThenInclude(exam => exam.UserExamAttempts.Where(x=>x.UserId == userId))
                .ThenInclude(attempt => attempt.User)
                .Include(course => course.Exams)
                .ThenInclude(exam => exam.Questions)
                .Include(course => course.Exams)
                .ThenInclude(exam => exam.UserExamAttempts.Where(x => x.UserId == userId))
                .ThenInclude(attempt => attempt.UserAnswers)
                .ToListAsync();

            return courseEntities;
        }

        public async Task<PaginationCourseEntity> GetAllByFilterWithPaginationAsync(Expression<Func<CourseEntity, bool>> filter, int pageSize, int pageNumber = 1, bool includeDeleted = false)
        {
            var query = sqlContext.Courses
                .Where(course => course.IsDeleted == false || course.IsDeleted == includeDeleted)
                .Where(filter);

            var result = new PaginationCourseEntity
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

        public async Task<PaginationCourseEntity> GetAllDeletedByFilterWithPaginationAsync(Expression<Func<CourseEntity, bool>> filter, int pageSize, int pageNumber = 1)
        {
            var query = sqlContext.Courses
                .Include(course => course.DeletedBy)
                .Where(course => course.IsDeleted)
                .Where(filter);

            var result = new PaginationCourseEntity
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

        public async Task<List<CourseEntity>> GetAllRemovedAsync()
        {
            var courseEntities = await sqlContext.Courses
                .Where(course => course.IsDeleted)
                .Include(course => course.DeletedBy)
                .ToListAsync();

            return courseEntities;
        }

        public async Task<List<CourseEntity>> GetAllWithDetailsAsync(bool includeDeleted = false)
        {
            var courseEntities = await sqlContext.Courses
                .Include(course => course.Exams)
                .Include(course => course.Users)
                .Where(course => course.IsDeleted == false || course.IsDeleted == includeDeleted)
                .ToListAsync();

            return courseEntities;
        }

        public async Task<List<CourseEntity>> GetAllWithDetailsByFilterAsync(Expression<Func<CourseEntity, bool>> filter, bool includeDeleted = false)
        {
            var courseEntities = await sqlContext.Courses
                .Include(course => course.Exams)
                .Include(course => course.Users)
                .Where(course => course.IsDeleted == false || course.IsDeleted == includeDeleted)
                .Where(filter)
                .ToListAsync();

            return courseEntities;
        }

        public async Task<CourseEntity?> GetByIdAsync(Guid id)
        {
            var courseEntity = await sqlContext.Courses
                .Where(x=>!x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();

            return courseEntity;
        }

        public async Task<CourseEntity?> GetByIdWithDetailsAsync(Guid id)
        {
            var courseEntity = await sqlContext.Courses
                .Where(x => !x.IsDeleted && x.Id == id)
                .Include(course => course.Exams.Where(exam => !exam.IsDeleted))
                .ThenInclude(exam => exam.UserExamAttempts)
                .ThenInclude(attempt => attempt.User)
                .Include(course => course.Exams)
                .ThenInclude(exam => exam.Questions)
                .Include(course => course.Users)
                .Include(course => course.Teachers)
                .FirstOrDefaultAsync();

            return courseEntity;
        }

        public async Task<bool> HardDeleteAsync(Guid id)
        {
            var courseEntity = await sqlContext.Courses.FindAsync(id);
            if (courseEntity != null)
            {
                sqlContext.Courses.Remove(courseEntity);
                return true;
            }

            return false;
        }

        public async Task<bool> SoftDeleteAsync(Guid id, string deletedByUserId)
        {
            var courseEntity = await sqlContext.Courses.FindAsync(id);
            if (courseEntity != null)
            {
                courseEntity.IsDeleted = true;
                courseEntity.DeletedById = deletedByUserId;
                courseEntity.DeletedOn = DateTimeOffset.Now;
                return true;
            }

            return false;
        }
    }
}