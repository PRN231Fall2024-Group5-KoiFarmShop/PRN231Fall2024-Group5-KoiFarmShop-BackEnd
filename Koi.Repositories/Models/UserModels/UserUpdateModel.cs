﻿using Koi.DTOs.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Repositories.Models.UserModels
{
    public class UserUpdateModel
    {
        public string? FullName { get; set; }
        public DateTime? Dob { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ImageUrl { get; set; }
        public string? Address { get; set; }
        public bool? IsActive { get; set; }
        public RoleEnums? RoleName { get; set; }
    }
}