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

        public async Task<Guid> CreateAsync(ExamEntity model)
        {
            var result = await _sqlContext.Set<ExamEntity>().AddAsync(model);
            return result.Entity.Id;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var examEntity = await _sqlContext.Set<ExamEntity>().FindAsync(id);
            if(examEntity != null)
            {
                examEntity.IsDeleted = true;
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<ExamEntity>> GetAllAsync(bool isDeleted = false)
        {
            var examEntities = await _sqlContext.Set<ExamEntity>()
                .Where(genre => genre.IsDeleted == false || genre.IsDeleted == isDeleted)
                .ToListAsync();
            return examEntities;
        }

        public async Task<ExamEntity?> GetByIdAsync(Guid id)
        {
            var examEntity = await _sqlContext.Set<ExamEntity>().FindAsync(id);
            return examEntity;
        }

        public Guid Update(ExamEntity model)
        {
            var result = _sqlContext.Set<ExamEntity>().Update(model);
            return result.Entity.Id;
        }
    }
}
