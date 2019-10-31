using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sparta_Online_Shop.Models
{
    public class PasswordConfirmationModel
    {
        [Display(Name = "Old Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your current password")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }


        [Display(Name = "New Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a new password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }


        [Display(Name = "Confirm Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please re-enter your new password")]
        [Compare("NewPassword", ErrorMessage = "The passwords you entered do not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }
}