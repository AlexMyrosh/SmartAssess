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

        public async Task<IdentityResult> CreateAsync(UserEntity user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Student");
            }

            return result;
        }

        public async Task<UserEntity?> GetCurrentUserAsync(ClaimsPrincipal userPrincipal)
        {
            var currentUser = await _userManager.GetUserAsync(userPrincipal);
            return currentUser;
        }

        public async Task<IdentityResult> UpdateAsync(UserEntity? user)
        {
            var currentUserBeforeUpdate = await _unitOfWork.UserRepository.GetByIdAsync(user!.Id);
            _mapper.Map(user, currentUserBeforeUpdate);
            return await _userManager.UpdateAsync(currentUserBeforeUpdate);
        }
    }
}
