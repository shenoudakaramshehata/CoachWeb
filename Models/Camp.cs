using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace Coach.Models
{
    public partial class Camp
    {
        public Camp()
        {
            CampImages = new HashSet<CampImage>();
        }

        public int CampId { get; set; }
        public string UserId { get; set; } 
        public string CampTlAr { get; set; }
        public string CampTlEn { get; set; }
        public string CampDescAr { get; set; }
        public string CampDescEn { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? PlanStartDate { get; set; }
        public DateTime? PlanEndDate { get; set; }
        public string Pic { get; set; }
        public int CampTypeId { get; set; }
        public int CountryId { get; set; }
        public int CampTargetId { get; set; }
        public int CampPlanId { get; set; }
        public DateTime? JoinStart { get; set; }
        public DateTime? JoinEnd { get; set; }
        public double? SubPrice { get; set; }
        public string Remarks { get; set; }
        public double? Cost { get; set; }
        public bool ispaid { get; set; }
        public int PaymentMethodId { get; set; }
        public string payment_type { get; set; }
        public string PaymentID { get; set; }
        public string Result { get; set; }
        public DateTime? PostDate { get; set; }
        public string TranID { get; set; }
        public string Ref { get; set; }
        public string TrackID { get; set; }
        public string Auth { get; set; }
        //end Payment
        [JsonIgnore]
        public virtual CampTarget CampTarget { get; set; }
        [JsonIgnore]
        public virtual CampType CampType { get; set; }
        [JsonIgnore]
        public virtual Country Country { get; set; }
        [JsonIgnore]
        public virtual CampPlan CampPlan { get; set; }
        [JsonIgnore]
        public virtual ICollection<CampImage> CampImages { get; set; }
    }
}
