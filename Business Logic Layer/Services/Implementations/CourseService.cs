﻿using AutoMapper;
using Business_Logic_Layer.Models;
using Business_Logic_Layer.Services.Interfaces;
using Data_Access_Layer.Models;
using Data_Access_Layer.UnitOfWork.Interfaces;
using System.Security.Claims;

namespace Business_Logic_Layer.Services.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;

        public CourseService(IUnitOfWork unitOfWork, IMapper mapper, IAccountService accountService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accountService = accountService;
        }

        public async Task AddUserForCourseAsync(ClaimsPrincipal userPrincipal, Guid courseId)
        {
            var userId = _accountService.GetUserId(userPrincipal);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return;
            }

            var courseModel = await _unitOfWork.CourseRepository.GetByIdWithDetailsAsync(courseId);
            if(courseModel is null)
            {
                return;
            }

            var userEntity = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            courseModel.Users.Add(userEntity);
            await _unitOfWork.SaveAsync();
        }

        public async Task<Guid> CreateAsync(CourseModel model, string createdByTeacherId)
        {
            var courseEntity = _mapper.Map<CourseEntity>(model);
            var createdByUser = await _unitOfWork.UserRepository.GetByIdAsync(createdByTeacherId);
            courseEntity.Teachers.Add(createdByUser);
            var createdCourseId = await _unitOfWork.CourseRepository.CreateAsync(courseEntity);
            await _unitOfWork.SaveAsync();
            return createdCourseId;
        }

        public async Task<IEnumerable<CourseModel>> GetAllAsync(bool includeDeleted = false)
        {
            var courseEntities = await _unitOfWork.CourseRepository.GetAllAsync(includeDeleted);
            var courseModels = _mapper.Map<IEnumerable<CourseModel>>(courseEntities);
            return courseModels;
        }

        public async Task<PaginationCourseModel> GetAllAppliedByUserBySearchQueryWithPaginationAsync(ClaimsPrincipal userPrincipal, int pageSize, string searchQuery = "", int pageNumber = 1, bool includeDeleted = false)
        {
            var userModel = await _accountService.GetUserAsync(userPrincipal);
            if (userModel is null)
            {
                throw new ArgumentException("Unable to get user by ClaimsPrincipal", nameof(userPrincipal));
            }

            var paginationCourseEntity = await _unitOfWork.CourseRepository.GetAllByFilterWithPaginationAsync(
                course => course.Name.Contains(searchQuery) && course.Users.Any(user => user.Id == userModel.Id), 
                pageSize, 
                pageNumber, 
                includeDeleted);

            var paginationCourseModel = _mapper.Map<PaginationCourseModel>(paginationCourseEntity);
            return paginationCourseModel;
        }

        public async Task<PaginationCourseModel> GetAllBySearchQueryWithPaginationAsync(int pageSize, string searchQuery = "", int pageNumber = 1, bool includeDeleted = false)
        {
            var paginationCourseEntity = await _unitOfWork.CourseRepository.GetAllByFilterWithPaginationAsync(course => course.Name.Contains(searchQuery), pageSize, pageNumber, includeDeleted);
            var paginationCourseModel = _mapper.Map<PaginationCourseModel>(paginationCourseEntity);
            return paginationCourseModel;
        }

        public async Task<IEnumerable<CourseModel>> GetAllWithDetailsAsync(bool includeDeleted = false)
        {
            var courseEntities = await _unitOfWork.CourseRepository.GetAllWithDetailsAsync(includeDeleted);
            var courseModels = _mapper.Map<IEnumerable<CourseModel>>(courseEntities);
            return courseModels;
        }

        public async Task<CourseModel?> GetByIdAsync(Guid id)
        {
            var courseEntity = await _unitOfWork.CourseRepository.GetByIdAsync(id);
            var courseModel = _mapper.Map<CourseModel>(courseEntity);
            return courseModel;
        }

        public async Task<CourseModel?> GetByIdWithDetailsAsync(Guid id)
        {
            var courseEntity = await _unitOfWork.CourseRepository.GetByIdWithDetailsAsync(id);
            var courseModel = _mapper.Map<CourseModel>(courseEntity);
            return courseModel;
        }

        public async Task<bool> HardDeleteAsync(Guid id)
        {
            var result = await _unitOfWork.CourseRepository.HardDeleteAsync(id);
            await _unitOfWork.SaveAsync();
            return result;
        }

        public async Task RemoveUserFromCourseAsync(ClaimsPrincipal userPrincipal, Guid courseId)
        {
            var userId = _accountService.GetUserId(userPrincipal);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return;
            }

            var courseModel = await _unitOfWork.CourseRepository.GetByIdWithDetailsAsync(courseId);
            if (courseModel is null)
            {
                return;
            }

            var userEntity = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            courseModel.Users.Remove(userEntity);
            await _unitOfWork.SaveAsync();
        }

        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            var result = await _unitOfWork.CourseRepository.SoftDeleteAsync(id);
            await _unitOfWork.SaveAsync();
            return result;
        }

        public async Task<Guid> UpdateAsync(CourseModel model)
        {
            var courseEntityFromDb = await _unitOfWork.CourseRepository.GetByIdWithDetailsAsync(model.Id);
            _mapper.Map(model, courseEntityFromDb);
            await _unitOfWork.SaveAsync();
            return model.Id;
        }

        public async Task<Guid> UpdateLongDescriptionAsync(Guid courseId, string newLongDescription)
        {
            var courseEntityFromDb = await _unitOfWork.CourseRepository.GetByIdWithDetailsAsync(courseId);
            courseEntityFromDb.LongDescription = newLongDescription;
            await _unitOfWork.SaveAsync();
            return courseId;
        }

        public async Task<IEnumerable<CourseModel>> GetAllWithTakenUserExamsAsync(ClaimsPrincipal userPrincipal, bool includeDeleted = false)
        {
            var userId = _accountService.GetUserId(userPrincipal);
            var courseEntities = await _unitOfWork.CourseRepository.GetAllByFilterAsync(course => course.Exams.Any(exam => exam.UserExamAttempts.Any(userAttempt => userAttempt.UserId == userId)));
            var courseModels = _mapper.Map<IEnumerable<CourseModel>>(courseEntities);
            return courseModels;
        }

        public async Task<PaginationCourseModel> GetAllAppliedByTeacherBySearchQueryWithPaginationAsync(ClaimsPrincipal userPrincipal, int pageSize, string searchQuery = "", int pageNumber = 1, bool includeDeleted = false)
        {
            var userModel = await _accountService.GetUserAsync(userPrincipal);
            if (userModel is null)
            {
                throw new ArgumentException("Unable to get user by ClaimsPrincipal", nameof(userPrincipal));
            }

            var paginationCourseEntity = await _unitOfWork.CourseRepository.GetAllByFilterWithPaginationAsync(
                course => course.Name.Contains(searchQuery) && course.Teachers.Any(user => user.Id == userModel.Id),
                pageSize,
                pageNumber,
                includeDeleted);

            var paginationCourseModel = _mapper.Map<PaginationCourseModel>(paginationCourseEntity);
            return paginationCourseModel;
        }

        public async Task AddTeacherForCourseAsync(ClaimsPrincipal userPrincipal, Guid courseId)
        {
            var userId = _accountService.GetUserId(userPrincipal);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return;
            }

            var courseModel = await _unitOfWork.CourseRepository.GetByIdWithDetailsAsync(courseId);
            if (courseModel is null)
            {
                return;
            }

            var userEntity = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            courseModel.Teachers.Add(userEntity);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveTeacherFromCourseAsync(ClaimsPrincipal userPrincipal, Guid courseId)
        {
            var userId = _accountService.GetUserId(userPrincipal);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return;
            }

            var courseModel = await _unitOfWork.CourseRepository.GetByIdWithDetailsAsync(courseId);
            if (courseModel is null)
            {
                return;
            }

            var userEntity = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            courseModel.Teachers.Remove(userEntity);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveUserFromCourseAsync(string userId, Guid courseId)
        {
            var courseModel = await _unitOfWork.CourseRepository.GetByIdWithDetailsAsync(courseId);
            if (courseModel is null)
            {
                return;
            }

            var userEntity = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            courseModel.Users.Remove(userEntity);
            await _unitOfWork.SaveAsync();
        }
    }
}