using System.Linq.Expressions;
using Data_Access_Layer.Context;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data_Access_Layer.Repositories.Implementations
{
    public class ExamRepository : IExamRepository
    {
        private readonly SqlContext _sqlContext;

        public ExamRepository(SqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<Guid> CreateAsync(ExamEntity entity)
        {
            var entityEntry = await _sqlContext.Exams.AddAsync(entity);
            return entityEntry.Entity.Id;
        }

        public async Task<IEnumerable<ExamEntity>> GetAllAsync(bool includeDeleted = false)
        {
            var examEntities = await _sqlContext.Exams
                .Where(exam => exam.IsDeleted == false || exam.IsDeleted == includeDeleted)
                .ToListAsync();

            return examEntities;
        }

        public async Task<IEnumerable<ExamEntity>> GetAllWithDetailsAsync(bool includeDeleted = false)
        {
            var examEntities = await _sqlContext.Exams
                .Include(exam => exam.Questions)
                .Include(exam => exam.UserExamAttempts)
                .Where(exam => exam.IsDeleted == false || exam.IsDeleted == includeDeleted)
                .ToListAsync();

            return examEntities;
        }

        public async Task<ExamEntity?> GetByIdAsync(Guid id)
        {
            var examEntity = await _sqlContext.Exams.FindAsync(id);
            return examEntity;
        }

        public async Task<ExamEntity?> GetByIdWithDetailsAsync(Guid id)
        {
            var examEntity = await _sqlContext.Exams
                .Include(exam => exam.Questions)
                .Include(exam => exam.Course)
                .Include(exam => exam.UserExamAttempts)
                .ThenInclude(x=>x.User)
                .Include(exam => exam.UserExamAttempts)
                .ThenInclude(x => x.UserAnswers)
                .FirstOrDefaultAsync(exam => exam.Id == id);

            return examEntity;
        }

        public async Task<bool> HardDeleteAsync(Guid id)
        {
            var examEntity = await _sqlContext.Exams.FindAsync(id);
            if (examEntity != null)
            {
                _sqlContext.Exams.Remove(examEntity);
                return true;
            }

            return false;
        }

        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            var examEntity = await _sqlContext.Exams.FindAsync(id);
            if (examEntity != null)
            {
                examEntity.IsDeleted = true;
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<ExamEntity>> GetAllExamsByFilterWithDetailsAsync(Expression<Func<ExamEntity, bool>> filters, bool includeDeleted = false)
        {
            var examEntities = await _sqlContext.Exams
                .Where(filters)
                .Where(exam => exam.IsDeleted == false || exam.IsDeleted == includeDeleted)
                .Include(exam => exam.Questions)
                .Include(exam => exam.UserExamAttempts)
                .ThenInclude(attempt => attempt.User)
                .ToListAsync();

            return examEntities;
        }
    }
}
