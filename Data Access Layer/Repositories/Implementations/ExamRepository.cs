using System.Linq.Expressions;
using Data_Access_Layer.Context;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data_Access_Layer.Repositories.Implementations
{
    public class ExamRepository(SqlContext sqlContext) : IExamRepository
    {
        public async Task<Guid> CreateAsync(ExamEntity entity)
        {
            var entityEntry = await sqlContext.Exams.AddAsync(entity);
            return entityEntry.Entity.Id;
        }

        public async Task<List<ExamEntity>> GetAllAsync(bool includeDeleted = false)
        {
            var examEntities = await sqlContext.Exams
                .Where(exam => exam.IsDeleted == false || exam.IsDeleted == includeDeleted)
                .ToListAsync();

            return examEntities;
        }

        public async Task<List<ExamEntity>> GetAllWithDetailsAsync(bool includeDeleted = false)
        {
            var examEntities = await sqlContext.Exams
                .Include(exam => exam.Questions)
                .Include(exam => exam.UserExamAttempts)
                .Where(exam => exam.IsDeleted == false || exam.IsDeleted == includeDeleted)
                .ToListAsync();

            return examEntities;
        }

        public async Task<ExamEntity?> GetByIdAsync(Guid id)
        {
            var examEntity = await sqlContext.Exams
                .Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();

            return examEntity;
        }

        public async Task<ExamEntity?> GetByIdWithDetailsAsync(Guid id)
        {
            var examEntity = await sqlContext.Exams
                .Where(x => !x.IsDeleted && x.Id == id)
                .Include(exam => exam.Questions)
                .Include(exam => exam.Course)
                .Include(exam => exam.UserExamAttempts)
                .ThenInclude(x=>x.User)
                .Include(exam => exam.UserExamAttempts)
                .ThenInclude(x => x.UserAnswers)
                .FirstOrDefaultAsync();

            return examEntity;
        }

        public async Task<bool> HardDeleteAsync(Guid id)
        {
            var examEntity = await sqlContext.Exams.FindAsync(id);
            if (examEntity != null)
            {
                sqlContext.Exams.Remove(examEntity);
                return true;
            }

            return false;
        }

        public async Task<bool> SoftDeleteAsync(Guid id, string deletedByUserId)
        {
            var examEntity = await sqlContext.Exams.FindAsync(id);
            if (examEntity != null)
            {
                examEntity.IsDeleted = true;
                examEntity.DeletedById = deletedByUserId;
                examEntity.DeletedOn = DateTimeOffset.Now;
                return true;
            }

            return false;
        }

        public async Task<List<ExamEntity>> GetAllExamsByFilterWithDetailsAsync(Expression<Func<ExamEntity, bool>> filters, bool includeDeleted = false)
        {
            var examEntities = await sqlContext.Exams
                .Where(filters)
                .Where(exam => exam.IsDeleted == false || exam.IsDeleted == includeDeleted)
                .Include(exam => exam.Questions)
                .Include(exam => exam.UserExamAttempts)
                .ThenInclude(attempt => attempt.User)
                .ToListAsync();

            return examEntities;
        }

        public async Task<List<ExamEntity>> GetAllRemovedAsync()
        {
            var examEntities = await sqlContext.Exams
                .Where(exam => exam.IsDeleted)
                .Include(exam => exam.DeletedBy)
                .Include(exam => exam.Course)
                .ToListAsync();

            return examEntities;
        }

        public async Task<PaginationExamEntity> GetAllDeletedByFilterWithPaginationAsync(Expression<Func<ExamEntity, bool>> filter, int pageSize, int pageNumber = 1)
        {
            var query = sqlContext.Exams
                .Include(exam => exam.Course)
                .Include(exam => exam.DeletedBy)
                .Where(course => course.IsDeleted)
                .Where(filter);

            var result = new PaginationExamEntity
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
