using Data_Access_Layer.Repositories.Interfaces;

namespace Data_Access_Layer.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        IExamRepository ExamRepository { get; }

        IUserExamPassRepository UserExamPassRepository { get; }

        IUserRepository UserRepository { get; }

        ICourseRepository CourseRepository { get; }

        Task SaveAsync();

        void ClearChangeTracker();
    }
}