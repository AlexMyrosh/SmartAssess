using AutoMapper;
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
            var user = await _accountService.GetUserAsync(userPrincipal);
            if(user is null)
            {
                return;
            }

            var courseModel = await _unitOfWork.CourseRepository.GetByIdWithDetailsAsync(courseId);
            if(courseModel is null)
            {
                return;
            }

            var userEntity = _mapper.Map<UserEntity>(user);
            courseModel.Users.Add(userEntity);
            await _unitOfWork.SaveAsync();
        }

        public async Task<Guid> CreateAsync(CourseModel model)
        {
            var courseEntity = _mapper.Map<CourseEntity>(model);
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
            var user = await _accountService.GetUserAsync(userPrincipal);
            if (user is null)
            {
                return;
            }

            var courseModel = await _unitOfWork.CourseRepository.GetByIdWithDetailsAsync(courseId);
            if (courseModel is null)
            {
                return;
            }

            var userEntity = _mapper.Map<UserEntity>(user);
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
    }
}
