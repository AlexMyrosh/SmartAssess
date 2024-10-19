using Data_Access_Layer.Context;
using Data_Access_Layer.Models;
using Data_Access_Layer.Models.Enums;
using Data_Access_Layer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data_Access_Layer.Repositories.Implementations
{
    public class UserExamAttemptRepository(SqlContext sqlContext) : IUserExamPassRepository
    {
        public async Task<Guid> CreateAsync(UserExamAttemptEntity entity)
        {
            var entityEntry = await sqlContext.UserExamAttempts.AddAsync(entity);
            return entityEntry.Entity.Id;
        }

        public async Task<List<UserExamAttemptEntity>> GetAllAsync()
        {
            var userExamAttemptEntities = await sqlContext.UserExamAttempts.ToListAsync();
            return userExamAttemptEntities;
        }

        public async Task<List<UserExamAttemptEntity>> GetAllWithDetailsAsync()
        {
            var userExamAttemptEntities = await sqlContext.Set<UserExamAttemptEntity>()
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
            var userExamAttemptEntity = await sqlContext.UserExamAttempts.FindAsync(id);
            return userExamAttemptEntity;
        }

        public async Task<UserExamAttemptEntity?> GetByIdWithDetailsAsync(Guid id)
        {
            var userExamAttemptEntity = await sqlContext.Set<UserExamAttemptEntity>()
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
            var entity = sqlContext.UserExamAttempts
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

        public void Update(UserExamAttemptEntity entity)
        {
            sqlContext.UserExamAttempts.Update(entity);
        }
    }
}
