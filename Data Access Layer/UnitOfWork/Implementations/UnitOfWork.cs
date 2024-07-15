using Data_Access_Layer.Context;
using Data_Access_Layer.Repositories.Implementations;
using Data_Access_Layer.Repositories.Interfaces;
using Data_Access_Layer.UnitOfWork.Interfaces;

namespace Data_Access_Layer.UnitOfWork.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SqlContext _sqlContext;
        private IExamRepository? _examRepository;

        public UnitOfWork(SqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public IExamRepository ExamRepository
        {
            get
            {
                _examRepository ??= new ExamRepository(_sqlContext);
                return _examRepository;

            }
        }

        public async Task<int> SaveAsync()
        {
            return await _sqlContext.SaveChangesAsync();
        }
    }
}
