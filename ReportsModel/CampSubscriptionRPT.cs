using System;

namespace Coach.ReportsModel
{
    public class CampSubscriptionRPT
    {
        public int CampId { get; set; }
        public string SubscriberName { get; set; }
        public string UserAddedby { get; set; }
        public string CampTlEn { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Pic { get; set; }
        public string Country { get; set; }
        public int CountryId { get; set; }
        public double Cost { get; set; }
    }
}
