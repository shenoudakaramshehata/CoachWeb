using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Coach.Entities
{
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        public int CustomerId { get; set; }
        
    }
    
}
