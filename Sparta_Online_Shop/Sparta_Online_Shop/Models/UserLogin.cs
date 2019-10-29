using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sparta_Online_Shop.Models
{
    public class UserLogin
    {
        [Display(Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your email address")]
        public string UserEmail { get; set; }

        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}