using Data_Access_Layer.Context;
using Data_Access_Layer.Models;
using Data_Access_Layer.Models.Enums;
using Data_Access_Layer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data_Access_Layer.Repositories.Implementations
{
    public class UserExamAttemptRepository : IUserExamPassRepository
    {
        private readonly SqlContext _sqlContext;

        public UserExamAttemptRepository(SqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<Guid> CreateAsync(UserExamAttemptEntity entity)
        {
            var entityEntry = await _sqlContext.UserExamAttempts.AddAsync(entity);
            return entityEntry.Entity.Id;
        }

        public async Task<IEnumerable<UserExamAttemptEntity>> GetAllAsync(bool includeDeleted = false)
        {
            var userExamAttemptEntities = await _sqlContext.UserExamAttempts
                .Where(userExamAttempt => userExamAttempt.IsDeleted == false || userExamAttempt.IsDeleted == includeDeleted)
                .ToListAsync();

            return userExamAttemptEntities;
        }

        public async Task<IEnumerable<UserExamAttemptEntity>> GetAllWithDetailsAsync(bool includeDeleted = false)
        {
            var userExamAttemptEntities = await _sqlContext.Set<UserExamAttemptEntity>()
                .Where(userExamAttempt => userExamAttempt.IsDeleted == false || userExamAttempt.IsDeleted == includeDeleted)
                .Include(entity => entity.Exam)
                .ThenInclude(exam => exam.Course)
                .Include(entity => entity.User)
                .Include(entity => entity.UserAnswers)
                .ThenInclude(sub => sub.Question)
                .ToListAsync();

            return userExamAttemptEntities;
        }

        public async Task<UserExamAttemptEntity?> GetByIdAsync(Guid id)
        {
            var userExamAttemptEntity = await _sqlContext.Set<UserExamAttemptEntity>().FindAsync(id);
            return userExamAttemptEntity;
        }

        public async Task<UserExamAttemptEntity?> GetByIdWithDetailsAsync(Guid id)
        {
            var userExamAttemptEntity = await _sqlContext.Set<UserExamAttemptEntity>()
                .Include(entity => entity.Exam)
                .ThenInclude(exam => exam.Course)
                .Include(entity => entity.User)
                .Include(entity => entity.UserAnswers)
                .ThenInclude(sub => sub.Question)
                .Include(entity => entity.Exam)
                .ThenInclude(exam => exam.UserExamAttempts)
                .FirstOrDefaultAsync(exam => exam.Id == id);

            return userExamAttemptEntity;
        }

        public Task<UserExamAttemptEntity?> GetStartedAttemptAsync(Guid examId, string userId)
        {
            var entity = _sqlContext.UserExamAttempts
                .Include(entity => entity.Exam)
                .ThenInclude(exam => exam.Course)
                .Include(entity => entity.Exam)
                .ThenInclude(exam => exam.Questions)
                .Include(entity => entity.Exam)
                .ThenInclude(exam => exam.UserExamAttempts)
                .Include(x => x.UserAnswers)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x =>
                x.UserId == userId && x.ExamId == examId && x.Status == ExamAttemptStatusEntity.InProgress);

            return entity;
        }

        public async Task<bool> HardDeleteAsync(Guid id)
        {
            var userExamAttemptEntity = await _sqlContext.UserExamAttempts.FindAsync(id);
            if (userExamAttemptEntity != null)
            {
                _sqlContext.UserExamAttempts.Remove(userExamAttemptEntity);
                return true;
            }

            return false;
        }

        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            var userExamAttemptEntity = await _sqlContext.UserExamAttempts.FindAsync(id);
            if (userExamAttemptEntity != null)
            {
                userExamAttemptEntity.IsDeleted = true;
                return true;
            }

            return false;
        }

        public void Update(UserExamAttemptEntity entity)
        {
            _sqlContext.UserExamAttempts.Update(entity);
        }
    }
}
