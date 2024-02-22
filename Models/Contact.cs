using System;
using System.Collections.Generic;


namespace Coach.Models
{
    public partial class Contact
    {
        public int ContactId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string Msg { get; set; }
        public DateTime? TransDate { get; set; }
    }
}
