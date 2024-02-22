using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Coach.Entities
{
    public class ResetPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
      
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        public string Email { get; set; }


    }

}
