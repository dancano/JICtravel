using JICtravel.Web.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JICtravel.Web.Helpers
{
    public interface IUserHelper
    {
        Task<SlaveEntity> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(SlaveEntity user, string password);

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(SlaveEntity user, string roleName);

        Task<bool> IsUserInRoleAsync(SlaveEntity user, string roleName);
    }

}
