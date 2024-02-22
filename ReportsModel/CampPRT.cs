using System;

namespace Coach.ReportsModel
{
    public class CampPRT
    {
        public int CampId { get; set; }
        public string User { get; set; } 
        public string CampTlAr { get; set; }
        public string CampTlEn { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Pic { get; set; }
        public string CampType { get; set; }
        public string Country { get; set; }
        public string CampTarget { get; set; }
        public string CampPlan { get; set; }
        public double? SubPrice { get; set; }
        public string Remarks { get; set; }
        public double? Cost { get; set; }
        public bool ispaid { get; set; }
        public string PaymentMethod { get; set; }
        public string payment_type { get; set; }
        public DateTime? PostDate { get; set; }
     

        
    }
}
