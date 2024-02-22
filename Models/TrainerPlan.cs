using System;
using System.Collections.Generic;

#nullable disable

namespace Coach.Models
{
    public partial class TrainerPlan
    {
        public TrainerPlan()
        {
            TrainerSubscriptions = new HashSet<TrainerSubscription>();
        }

        public int TrainerPlanId { get; set; }
        public string PlanTlAr { get; set; }
        public string PlanTlEn { get; set; }
        public bool? IsActive { get; set; }
        public double? Price { get; set; }
        public int? DurationInMonth { get; set; }
        public int? CountryId { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<TrainerSubscription> TrainerSubscriptions { get; set; }


    }
}
