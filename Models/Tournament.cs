using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace Coach.Models
{
    public partial class Tournament
    {
        public Tournament()
        {
            TournamentImages = new HashSet<TournamentImage>();
        }

        public int TournamentId { get; set; }
        public string UserId { get; set; }//uesr
        public string TournamentTlAr { get; set; }
        public string TournamentTlEn { get; set; }
        public string TournamentDescAr { get; set; }
        public string TournamentDescEn { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Pic { get; set; }
        public int TournamentTypeId { get; set; }
        public int CountryId { get; set; }
        public int TournamentTargetId { get; set; }
        public int TournamentPlanId { get; set; }
        public DateTime? SubStartDate { get; set; }
        public DateTime? SubEndDate { get; set; }
        public DateTime? PlanStartDate { get; set; }
        public DateTime? PlanEndDate { get; set; }
        public double? SubPrice { get; set; }
        public string Remarks { get; set; }
        public double? Cost { get; set; }

        //Payment
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
        public virtual Country Country { get; set; }
        [JsonIgnore]
        public virtual TournamentPlan TournamentPlan { get; set; }
        [JsonIgnore]
        public virtual TournamentTarget TournamentTarget { get; set; }
        [JsonIgnore]
        public virtual TournamentType TournamentType { get; set; }
        [JsonIgnore]
        public virtual ICollection<TournamentImage> TournamentImages { get; set; }
    }
}
