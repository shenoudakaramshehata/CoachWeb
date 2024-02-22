using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Coach.Entities
{
    public class RegistrationModel
    {
        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }  
        public string Mobile { get; set; }
        public string Tele { get; set; }
        public string Fax { get; set; } 
        public int? GenderId { get; set; }
        public int? CountryId { get; set; }
        public string Pic { get; set; }
        public int SectionId { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
    }
}
