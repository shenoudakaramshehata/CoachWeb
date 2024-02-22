using System;
using System.Collections.Generic;

#nullable disable

namespace Coach.Models
{
    public partial class Newsletter
    {
        public int NewsletterId { get; set; }
        public string Email { get; set; }
        public DateTime? Date { get; set; }
    }
}
