using JICtravel.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JICtravel.Common.Helpers
{
    public static class CombosHelper
    {
        public static List<Role> GetRoles()
        {
            return new List<Role>
            {
                new Role { Id = 1, Name = "Slave" }
            };
        }
    }

}
