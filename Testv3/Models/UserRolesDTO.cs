﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Testv3.Models
{
        public class UserRolesDTO     
        {
            [Key]
            [Display(Name = "Role Name")]
            public string RoleName { get; set; }
        }

        public class ExpandedUserDTO
        {
            [Key]
            [Display(Name = "User Name")]
            public string UserName { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long. Must contain atleast 1 number and symbol.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Lockout End Date Utc")]
            public DateTime? LockoutEndDateUtc { get; set; }
            public int AccessFailedCount { get; set; }
            public string PhoneNumber { get; set; }
            public IEnumerable<UserRolesDTO> Roles { get; set; }

        }


        public class UserRoleDTO
        {
            [Key]
            [Display(Name = "User Name")]
            public string UserName { get; set; }
            [Display(Name = "Role Name")]
            public string RoleName { get; set; }
        }


        public class RoleDTO
        {
            [Key]
            public string Id { get; set; }
            [Display(Name = "Role Name")]
            public string RoleName { get; set; }

        }

        public class UserAndRolesDTO
        {
            [Key]
            [Display(Name = "User Name")]
            public string UserName { get; set; }
            public List<UserRoleDTO> colUserRoleDTO { get; set; }

        }

    
}