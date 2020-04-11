using JICtravel.Web.Data.Entities;
using JICtravel.Web.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace JICtravel.Web.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<SlaveEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<SlaveEntity> _signInManager;

        public UserHelper(
            UserManager<SlaveEntity> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<SlaveEntity> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.RememberMe,
                false);
        }
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }


        public async Task<IdentityResult> AddUserAsync(SlaveEntity user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task AddUserToRoleAsync(SlaveEntity user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            bool roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }

        public async Task<SlaveEntity> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> IsUserInRoleAsync(SlaveEntity user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }
    }

}
