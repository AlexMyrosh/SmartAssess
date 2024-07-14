using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Identity;

namespace Business_Logic_Layer.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IdentityResult> CreateAsync(UserEntity user, string password);
    }
}
