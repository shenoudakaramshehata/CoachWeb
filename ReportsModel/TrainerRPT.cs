using System;

namespace Coach.ReportsModel
{
    public class TrainerRPT
    {
        public int TrainerId { get; set; }
        public string UserId { get; set; }
        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Tele { get; set; }
        public string Fax { get; set; }
        public string Gender { get; set; }
        public string Country { get; set; }
        
        public string TrainerPlan { get; set; }
        public string Pic { get; set; }
        public string Section { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public double SubscriptionCost { get; set; }
        public DateTime? AddedDate { get; set; }
       
    }
}
