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
        private IUserExamPassRepository? _userExamPassRepository;
        private IUserRepository? _userRepository;

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

        public IUserExamPassRepository UserExamPassRepository
        {
            get
            {
                _userExamPassRepository ??= new UserExamAttemptRepository(_sqlContext);
                return _userExamPassRepository;
            }
        }

        public IUserRepository UserRepository
        {
            get
            {
                _userRepository ??= new UserRepository(_sqlContext);
                return _userRepository;
            }
        }

        public async Task SaveAsync()
        {
            await _sqlContext.SaveChangesAsync();
        }
    }
}
