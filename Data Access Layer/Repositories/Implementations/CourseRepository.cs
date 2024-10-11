using Data_Access_Layer.Context;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data_Access_Layer.Repositories.Implementations
{
    public class CourseRepository : ICourseRepository
    {
        private readonly SqlContext _sqlContext;

        public CourseRepository(SqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<Guid> CreateAsync(CourseEntity entity)
        {
            var entityEntry = await _sqlContext.Courses.AddAsync(entity);
            return entityEntry.Entity.Id;
        }

        public async Task<IEnumerable<CourseEntity>> GetAllAsync(bool includeDeleted = false)
        {
            var courseEntities = await _sqlContext.Courses
                .Where(course => course.IsDeleted == false || course.IsDeleted == includeDeleted)
                .ToListAsync();

            return courseEntities;
        }

        public async Task<IEnumerable<CourseEntity>> GetAllByFilterAsync(Expression<Func<CourseEntity, bool>> filter, string userId, bool includeDeleted = false)
        {
            var courseEntities = await _sqlContext
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
            var query = _sqlContext.Courses
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

        public async Task<IEnumerable<CourseEntity>> GetAllWithDetailsAsync(bool includeDeleted = false)
        {
            var courseEntities = await _sqlContext.Courses
                .Include(course => course.Exams)
                .Include(course => course.Users)
                .Where(course => course.IsDeleted == false || course.IsDeleted == includeDeleted)
                .ToListAsync();

            return courseEntities;
        }

        public async Task<IEnumerable<CourseEntity>> GetAllWithDetailsByFilterAsync(Expression<Func<CourseEntity, bool>> filter, bool includeDeleted = false)
        {
            var courseEntities = await _sqlContext.Courses
                .Include(course => course.Exams)
                .Include(course => course.Users)
                .Where(course => course.IsDeleted == false || course.IsDeleted == includeDeleted)
                .Where(filter)
                .ToListAsync();

            return courseEntities;
        }

        public async Task<CourseEntity?> GetByIdAsync(Guid id)
        {
            var courseEntity = await _sqlContext.Courses.FindAsync(id);
            return courseEntity;
        }

        public async Task<CourseEntity?> GetByIdWithDetailsAsync(Guid id)
        {
            var courseEntity = await _sqlContext.Courses
                .Include(course => course.Exams.Where(exam => !exam.IsDeleted))
                .ThenInclude(exam => exam.UserExamAttempts)
                .ThenInclude(attempt => attempt.User)
                .Include(course => course.Exams)
                .ThenInclude(exam => exam.Questions)
                .Include(course => course.Users)
                .Include(course => course.Teachers)
                .FirstOrDefaultAsync(course => course.Id == id);

            return courseEntity;
        }

        public async Task<bool> HardDeleteAsync(Guid id)
        {
            var courseEntity = await _sqlContext.Courses.FindAsync(id);
            if (courseEntity != null)
            {
                _sqlContext.Courses.Remove(courseEntity);
                return true;
            }

            return false;
        }

        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            var courseEntity = await _sqlContext.Courses.FindAsync(id);
            if (courseEntity != null)
            {
                courseEntity.IsDeleted = true;
                return true;
            }

            return false;
        }
    }
}
