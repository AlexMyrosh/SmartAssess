﻿using Data_Access_Layer.Models;

namespace Data_Access_Layer.Repositories.Interfaces
{
    public interface IUserExamPassRepository
    {
        Task<Guid> CreateAsync(UserExamAttemptEntity entity);

        void Update(UserExamAttemptEntity entity);

        Task<List<UserExamAttemptEntity>> GetAllAsync();

        Task<List<UserExamAttemptEntity>> GetAllWithDetailsAsync();

        Task<UserExamAttemptEntity?> GetByIdAsync(Guid id);

        Task<UserExamAttemptEntity?> GetByIdWithDetailsAsync(Guid id);

        Task<UserExamAttemptEntity?> GetStartedAttemptAsync(Guid examId, string userId);
    }
}