using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Coach.Entities
{
    public class ChangeEmailVM
    {
        [Required]
        [Display(Name = "New Email")]
        //[RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z")]
        public string NewEmail { get; set; }
        [Required]
        [Display(Name = "Confirm New Email")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{6,}$",ErrorMessage = "Should have at least one lower case , one upper case , one number,one special character and minimum length 6 characters")]
        [Compare("NewEmail", ErrorMessage = "The New NewEmail and Confirmation Email do not match")]
        public string ConfirmNewEmail { get; set; }
    }
}
