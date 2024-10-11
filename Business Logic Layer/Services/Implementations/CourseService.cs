using AutoMapper;
using Business_Logic_Layer.Models;
using Business_Logic_Layer.Services.Interfaces;
using Data_Access_Layer.Models;
using Data_Access_Layer.UnitOfWork.Interfaces;
using System.Security.Claims;
using Business_Logic_Layer.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Data_Access_Layer.Roles;

namespace Business_Logic_Layer.Services.Implementations
{
    public class CourseService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IAccountService accountService,
        IUserExamPassService userExamPassService,
        UserManager<UserEntity> userManager)
        : ICourseService
    {
        public async Task AddUserForCourseAsync(ClaimsPrincipal userPrincipal, Guid courseId)
        {
            var userId = accountService.GetUserId(userPrincipal);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return;
            }

            var courseModel = await unitOfWork.CourseRepository.GetByIdWithDetailsAsync(courseId);
            if(courseModel is null)
            {
                return;
            }

            var userEntity = await unitOfWork.UserRepository.GetByIdAsync(userId);
            courseModel.Users.Add(userEntity);
            await unitOfWork.SaveAsync();
        }

        public async Task<Guid> CreateAsync(CourseModel model, string createdByTeacherId)
        {
            var courseEntity = mapper.Map<CourseEntity>(model);
            var createdByUser = await unitOfWork.UserRepository.GetByIdAsync(createdByTeacherId);
            courseEntity.Teachers.Add(createdByUser);
            var createdCourseId = await unitOfWork.CourseRepository.CreateAsync(courseEntity);
            await unitOfWork.SaveAsync();
            return createdCourseId;
        }

        public async Task<IEnumerable<CourseModel>> GetAllAsync(bool includeDeleted = false)
        {
            var courseEntities = await unitOfWork.CourseRepository.GetAllAsync(includeDeleted);
            var courseModels = mapper.Map<IEnumerable<CourseModel>>(courseEntities);
            return courseModels;
        }

        public async Task<PaginationCourseModel> GetAllAppliedByUserBySearchQueryWithPaginationAsync(ClaimsPrincipal userPrincipal, int pageSize, string searchQuery = "", int pageNumber = 1, bool includeDeleted = false)
        {
            var userModel = await accountService.GetUserAsync(userPrincipal);
            if (userModel is null)
            {
                throw new ArgumentException("Unable to get user by ClaimsPrincipal", nameof(userPrincipal));
            }

            var paginationCourseEntity = await unitOfWork.CourseRepository.GetAllByFilterWithPaginationAsync(
                course => course.Name.Contains(searchQuery) && course.Users.Any(user => user.Id == userModel.Id), 
                pageSize, 
                pageNumber, 
                includeDeleted);

            var paginationCourseModel = mapper.Map<PaginationCourseModel>(paginationCourseEntity);
            paginationCourseModel.CurrentUserRole = RoleNames.Student;
            return paginationCourseModel;
        }

        public async Task<PaginationCourseModel> GetAllBySearchQueryWithPaginationAsync(int pageSize, string searchQuery = "", int pageNumber = 1, bool includeDeleted = false)
        {
            var paginationCourseEntity = await unitOfWork.CourseRepository.GetAllByFilterWithPaginationAsync(course => course.Name.Contains(searchQuery), pageSize, pageNumber, includeDeleted);
            var paginationCourseModel = mapper.Map<PaginationCourseModel>(paginationCourseEntity);
            return paginationCourseModel;
        }

        public async Task<IEnumerable<CourseModel>> GetAllWithDetailsAsync(bool includeDeleted = false)
        {
            var courseEntities = await unitOfWork.CourseRepository.GetAllWithDetailsAsync(includeDeleted);
            var courseModels = mapper.Map<IEnumerable<CourseModel>>(courseEntities);
            return courseModels;
        }

        public async Task<CourseModel?> GetByIdAsync(Guid id)
        {
            var courseEntity = await unitOfWork.CourseRepository.GetByIdAsync(id);
            var courseModel = mapper.Map<CourseModel>(courseEntity);
            return courseModel;
        }

        public async Task<CourseModel?> GetByIdWithDetailsAsync(Guid id, ClaimsPrincipal currentUserPrincipal)
        {
            var courseEntity = await unitOfWork.CourseRepository.GetByIdWithDetailsAsync(id);
            var courseModel = mapper.Map<CourseModel>(courseEntity);
            var currentUser = await userManager.GetUserAsync(currentUserPrincipal);

            if (await userManager.IsInRoleAsync(currentUser, RoleNames.Student))
            {
                courseModel.IsCurrentUserApplied = courseModel.Users.Exists(user => user.Id == currentUser.Id);
            }
            else if (await userManager.IsInRoleAsync(currentUser, RoleNames.Teacher))
            {
                courseModel.IsCurrentUserApplied = courseModel.Teachers.Exists(user => user.Id == currentUser.Id);
            }

            foreach (var exam in courseModel.Exams)
            {
                exam.UserAttemptCount = exam.UserExamAttempts.Count(x => x.User.Id == currentUser.Id);
                var startedExam = exam.UserExamAttempts.FirstOrDefault(x => x.User.Id == currentUser.Id && x.Status == ExamAttemptStatusModel.InProgress);
                if (startedExam != null && DateTimeOffset.Now - startedExam.AttemptStarterAt > startedExam.Exam?.ExamDuration)
                {
                    await userExamPassService.SetStatusAsync(startedExam.Id.Value, ExamAttemptStatusModel.Completed);
                }
                else if (startedExam != null)
                {
                    exam.StartedAttemptId = startedExam.Id;
                }
            }

            return courseModel;
        }

        public async Task<bool> HardDeleteAsync(Guid id)
        {
            var result = await unitOfWork.CourseRepository.HardDeleteAsync(id);
            await unitOfWork.SaveAsync();
            return result;
        }

        public async Task RemoveUserFromCourseAsync(ClaimsPrincipal userPrincipal, Guid courseId)
        {
            var userId = accountService.GetUserId(userPrincipal);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return;
            }

            var courseModel = await unitOfWork.CourseRepository.GetByIdWithDetailsAsync(courseId);
            if (courseModel is null)
            {
                return;
            }

            var userEntity = await unitOfWork.UserRepository.GetByIdAsync(userId);
            courseModel.Users.Remove(userEntity);
            await unitOfWork.SaveAsync();
        }

        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            var result = await unitOfWork.CourseRepository.SoftDeleteAsync(id);
            await unitOfWork.SaveAsync();
            return result;
        }

        public async Task<Guid> UpdateAsync(CourseModel model)
        {
            var courseEntityFromDb = await unitOfWork.CourseRepository.GetByIdWithDetailsAsync(model.Id);
            mapper.Map(model, courseEntityFromDb);
            await unitOfWork.SaveAsync();
            return model.Id;
        }

        public async Task<Guid> UpdateLongDescriptionAsync(Guid courseId, string newLongDescription)
        {
            var courseEntityFromDb = await unitOfWork.CourseRepository.GetByIdWithDetailsAsync(courseId);
            courseEntityFromDb.LongDescription = newLongDescription;
            await unitOfWork.SaveAsync();
            return courseId;
        }

        public async Task<IEnumerable<CourseModel>> GetAllWithTakenUserExamsAsync(ClaimsPrincipal userPrincipal, bool includeDeleted = false)
        {
            var userId = accountService.GetUserId(userPrincipal);
            var courseEntities = await unitOfWork.CourseRepository.GetAllByFilterAsync(course => course.Exams.Any(exam => exam.UserExamAttempts.Any(userAttempt => userAttempt.UserId == userId)));
            var courseModels = mapper.Map<IEnumerable<CourseModel>>(courseEntities);
            return courseModels;
        }

        public async Task<PaginationCourseModel> GetAllAppliedByTeacherBySearchQueryWithPaginationAsync(ClaimsPrincipal userPrincipal, int pageSize, string searchQuery = "", int pageNumber = 1, bool includeDeleted = false)
        {
            var userModel = await accountService.GetUserAsync(userPrincipal);
            if (userModel is null)
            {
                throw new ArgumentException("Unable to get user by ClaimsPrincipal", nameof(userPrincipal));
            }

            var paginationCourseEntity = await unitOfWork.CourseRepository.GetAllByFilterWithPaginationAsync(
                course => course.Name.Contains(searchQuery) && course.Teachers.Any(user => user.Id == userModel.Id),
                pageSize,
                pageNumber,
                includeDeleted);

            var paginationCourseModel = mapper.Map<PaginationCourseModel>(paginationCourseEntity);
            paginationCourseModel.CurrentUserRole = RoleNames.Teacher;
            return paginationCourseModel;
        }

        public async Task AddTeacherForCourseAsync(ClaimsPrincipal userPrincipal, Guid courseId)
        {
            var userId = accountService.GetUserId(userPrincipal);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return;
            }

            var courseModel = await unitOfWork.CourseRepository.GetByIdWithDetailsAsync(courseId);
            if (courseModel is null)
            {
                return;
            }

            var userEntity = await unitOfWork.UserRepository.GetByIdAsync(userId);
            courseModel.Teachers.Add(userEntity);
            await unitOfWork.SaveAsync();
        }

        public async Task RemoveTeacherFromCourseAsync(ClaimsPrincipal userPrincipal, Guid courseId)
        {
            var userId = accountService.GetUserId(userPrincipal);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return;
            }

            var courseModel = await unitOfWork.CourseRepository.GetByIdWithDetailsAsync(courseId);
            if (courseModel is null)
            {
                return;
            }

            var userEntity = await unitOfWork.UserRepository.GetByIdAsync(userId);
            courseModel.Teachers.Remove(userEntity);
            await unitOfWork.SaveAsync();
        }

        public async Task RemoveUserFromCourseAsync(string userId, Guid courseId)
        {
            var courseModel = await unitOfWork.CourseRepository.GetByIdWithDetailsAsync(courseId);
            if (courseModel is null)
            {
                return;
            }

            var userEntity = await unitOfWork.UserRepository.GetByIdAsync(userId);
            courseModel.Users.Remove(userEntity);
            await unitOfWork.SaveAsync();
        }
    }
}