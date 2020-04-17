﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JICtravel.Common.Models
{
    public class UserRequest
    {
        [Required]
        public string Document { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        public int UserTypeId { get; set; } // 0: Admin, 1:Slave

        public byte[] PictureArray { get; set; }

        public string PasswordConfirm { get; set; }
    }
}
