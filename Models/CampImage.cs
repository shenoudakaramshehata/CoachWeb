using System;
using System.Collections.Generic;

#nullable disable

namespace Coach.Models
{
    public partial class CampImage
    {
        public int CampImageId { get; set; }
        public int CampId { get; set; }
        public string Pic { get; set; }

        public virtual Camp Camp { get; set; }
    }
}
