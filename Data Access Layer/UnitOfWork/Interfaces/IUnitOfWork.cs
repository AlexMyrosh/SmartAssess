using Data_Access_Layer.Repositories.Interfaces;

namespace Data_Access_Layer.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        IExamRepository ExamRepository { get; }

        Task<int> SaveAsync();
    }
}