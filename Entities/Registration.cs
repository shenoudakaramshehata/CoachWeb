﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Coach.Entities
{
    public class Registration
    {
        public string FullName{ get; set; }
        public int CountryId { get; set; }
        public string Mobile { get; set; }
       
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
      
    }
}
