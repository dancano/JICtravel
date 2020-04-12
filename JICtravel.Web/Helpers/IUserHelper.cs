using JICtravel.Web.Data.Entities;
using JICtravel.Web.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace JICtravel.Web.Helpers
{
    public interface IUserHelper
    {
        Task<SlaveEntity> GetUserAsync(string email);

        Task<SlaveEntity> GetUserAsync(Guid userId);

        Task<IdentityResult> AddUserAsync(SlaveEntity user, string password);

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(SlaveEntity user, string roleName);

        Task<bool> IsUserInRoleAsync(SlaveEntity user, string roleName);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();
        
        Task<SlaveEntity> AddUserAsync(AddUserViewModel model, string path);

        Task<IdentityResult> ChangePasswordAsync(SlaveEntity user, string oldPassword, string newPassword);

        Task<IdentityResult> UpdateUserAsync(SlaveEntity user);

        Task<SignInResult> ValidatePasswordAsync(SlaveEntity user, string password);

    }

}
