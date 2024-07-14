using Business_Logic_Layer.Models;

namespace Business_Logic_Layer.Services.Interfaces
{
    public interface IExamService
    {
        Task<IEnumerable<ExamModel>> GetAllAsync();

        Task<ExamModel> GetByIdAsync(Guid id);

        Task<Guid> CreateAsync(ExamModel model);

        Task<int> DeleteAsync(Guid id);

        Task<Guid> UpdateAsync(ExamModel model);
    }
}
