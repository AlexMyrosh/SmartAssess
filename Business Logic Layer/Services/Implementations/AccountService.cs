using System.Security.Claims;
using AutoMapper;
using Business_Logic_Layer.Services.Interfaces;
using Data_Access_Layer.Models;
using Data_Access_Layer.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Business_Logic_Layer.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountService(UserManager<UserEntity> userManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IdentityResult?> CreateAsync(UserEntity user, string password)
        {
            var identityResult = await _userManager.CreateAsync(user, password);
            if(identityResult.Succeeded)
            {
                // TODO: Replace "Student" by enum or const value
                await _userManager.AddToRoleAsync(user, "Student");
            }

            return identityResult;
        }

        public async Task<UserEntity?> GetUserAsync(ClaimsPrincipal userPrincipal)
        {
            var user = await _userManager.GetUserAsync(userPrincipal);
            return user;
        }

        public async Task<IdentityResult?> UpdateAsync(UserEntity? user)
        {
            var userFromDb = await _unitOfWork.UserRepository.GetByIdAsync(user!.Id);
            if (userFromDb is null)
            {
                throw new ArgumentException("Unable to get user by id", nameof(user.Id));
            }

            _mapper.Map(user, userFromDb);
            var identityResult = await _userManager.UpdateAsync(userFromDb);
            return identityResult;
        }
    }
}
