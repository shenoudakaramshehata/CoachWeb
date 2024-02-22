using System;
using System.Collections.Generic;

#nullable disable

namespace Coach.Models
{
    public partial class CampType
    {
        public CampType()
        {
            Camps = new HashSet<Camp>();
        }

        public int CampTypeId { get; set; }
        public string CampTypeTlAr { get; set; }
        public string CampTypeTlEn { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Camp> Camps { get; set; }
    }
}
