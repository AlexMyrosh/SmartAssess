﻿using Business_Logic_Layer.Models;

namespace Business_Logic_Layer.Services.Interfaces
{
    public interface IUserExamPassService
    {
        Task<Guid> CreateAsync(UserExamAttemptModel model);

        Task<IEnumerable<UserExamAttemptModel>> GetAllAsync(bool includeDeleted = false);

        Task<IEnumerable<UserExamAttemptModel>> GetAllWithDetailsAsync(bool includeDeleted = false);

        Task<UserExamAttemptModel?> GetByIdAsync(Guid id);

        Task<UserExamAttemptModel?> GetByIdWithDetailsAsync(Guid id);

        Task<Guid> UpdateAsync(UserExamAttemptModel model);
    }
}
