using Data_Access_Layer.Context;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data_Access_Layer.Repositories.Implementations
{
    public class UserExamPassRepository : IUserExamPassRepository
    {
        private readonly SqlContext _sqlContext;

        public UserExamPassRepository(SqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<Guid> CreateAsync(UserExamPassEntity entity)
        {
            var result = await _sqlContext.Set<UserExamPassEntity>().AddAsync(entity);
            return result.Entity.Id;
        }

        public async Task<IEnumerable<UserExamPassEntity>> GetAllAsync(bool isDeleted = false)
        {
            var entities = await _sqlContext.Set<UserExamPassEntity>().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<UserExamPassEntity>> GetAllWithDetailsAsync(bool isDeleted = false)
        {
            var entities = await _sqlContext.Set<UserExamPassEntity>()
                .Include(entity => entity.Exam)
                .Include(entity => entity.User)
                .Include(entity => entity.UserAnswers)
                .ThenInclude(sub => sub.Question)
                .ToListAsync();

            return entities;
        }

        public async Task<UserExamPassEntity?> GetByIdAsync(Guid id)
        {
            var entity = await _sqlContext.Set<UserExamPassEntity>().FindAsync(id);
            return entity;
        }

        public async Task<UserExamPassEntity?> GetByIdWithDetailsAsync(Guid id)
        {
            var examEntity = await _sqlContext.Set<UserExamPassEntity>()
                .Include(entity => entity.Exam)
                .Include(entity => entity.User)
                .Include(entity => entity.UserAnswers)
                .ThenInclude(sub => sub.Question)
                .FirstOrDefaultAsync(exam => exam.Id == id);

            return examEntity;
        }
    }
}
