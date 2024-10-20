﻿using System.Security.Claims;
using Business_Logic_Layer.Models;

namespace Business_Logic_Layer.Services.Interfaces
{
    public interface IExamService
    {
        Task<IEnumerable<ExamModel>> GetAllAsync(bool includeDeleted = false);

        Task<IEnumerable<ExamModel>> GetAllWithDetailsAsync(bool includeDeleted = false);

        Task<ExamModel?> GetByIdAsync(Guid id);

        Task<ExamModel?> GetByIdWithDetailsAsync(Guid id);

        Task<Guid> CreateAsync(ExamModel model);

        Task<bool> SoftDeleteAsync(Guid id, ClaimsPrincipal deletedByUserClaimsPrincipal);

        Task<bool> HardDeleteAsync(Guid id);

        Task<Guid> UpdateAsync(ExamModel model);

        Task<IEnumerable<ExamModel>> GetAllAvailableExamsWithDetailsAsync(bool includeDeleted = false);

        Task<List<ExamModel>> GetAllRemovedExamsAsync();

        Task RestoreAsync(Guid examId);

        Task<PaginationExamModel> GetAllDeletedBySearchQueryWithPaginationAsync(int pageSize, string searchQuery = "", int pageNumber = 1);
    }
}
