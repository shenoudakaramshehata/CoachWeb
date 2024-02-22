using System;

namespace Coach.ReportsModel
{
    public class CamFilterModel
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int? CountryId { get; set; }
        public string UserId { get; set; }
        public int? CampId { get; set; }


    }
}
