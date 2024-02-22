using System;
using System.Collections.Generic;

#nullable disable

namespace Coach.Models
{
    public partial class Banner
    {
        public int BannerId { get; set; }
        public string BannerPic { get; set; }
        public int? EntityTypeId { get; set; }
        public string EntityId { get; set; }
        public bool? BannerIsActive { get; set; }
        public int? BannerOrderIndex { get; set; }
    }
}
