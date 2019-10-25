using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Buffers;


namespace Sparta_Online_Shop
{
    public class RestrictedDate : System.ComponentModel.DataAnnotations.ValidationAttribute
        {
            protected override ValidationResult IsValid(object date, ValidationContext validationContext)
            {
                if (date != null)
                {
                    DateTime d = (DateTime)date;
                    if (d < DateTime.Now || d == DateTime.MinValue)
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        return new ValidationResult(ErrorMessage);
                    }
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
    }

        [MetadataType(typeof(UserMetadata))]
        public partial class User
        {
            [NotMapped]
            public string ConfirmPassword { get; set; }
        }

        public class UserMetadata
        {
            [Display(Name = "First Name")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a first name")]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a last name")]
            public string LastName { get; set; }

            [Display(Name = "Email")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter an email address")]
            [EmailAddress(ErrorMessage = "The email address you have entered is not valid")]
            [DataType(DataType.EmailAddress)]
            public string UserEmail { get; set; }

            [Display(Name = "Password")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a password")]
            [DataType(DataType.Password)]
            [MinLength(6, ErrorMessage = "Your password needs to be at least 6 characters long")]
            public string UserPassword { get; set; }

            [Display(Name = "Confirm Password")]
            [DataType(DataType.Password)]
            [Compare("UserPassword", ErrorMessage = "The passwords you entered do not match")]
            public string ConfirmPassword { get; set; }

        }
}
