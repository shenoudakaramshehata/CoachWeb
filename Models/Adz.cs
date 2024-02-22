using System;
using System.Collections.Generic;

#nullable disable

namespace Coach.Models
{
    public partial class Adz
    {
       
        public int AdzId { get; set; }
        public string AdzPic { get; set; }
        public int? EntityTypeId { get; set; }
        public string EntityId { get; set; }
        public bool? AdzIsActive { get; set; }
        public int? AdzOrderIndex { get; set; }
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }
        public virtual EntityType EntityType { get; set; }
    }
}
