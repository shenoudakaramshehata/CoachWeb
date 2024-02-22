using System;
using System.Collections.Generic;

#nullable disable

namespace Coach.Models
{
    public partial class CampTarget
    {
        public CampTarget()
        {
            Camps = new HashSet<Camp>();
        }

        public int CampTargetId { get; set; }
        public string CampTargetTlAr { get; set; }
        public string CampTargetTlEn { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Camp> Camps { get; set; }
    }
}
