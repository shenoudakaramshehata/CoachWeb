using System;
using System.Collections.Generic;

#nullable disable

namespace Coach.Models
{
    public partial class TrainerSubscription
    {
        public int TrainerSubscriptionId { get; set; }
        public int TrainerId { get; set; }
        public int TrainerPlanId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double? Price { get; set; }
        public string Remarks { get; set; }
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
        public virtual Trainer Trainer { get; set; }
        public virtual TrainerPlan TrainerPlan { get; set; }


    }
}
