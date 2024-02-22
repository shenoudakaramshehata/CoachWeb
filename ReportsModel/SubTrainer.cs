using System;

namespace Coach.ReportsModel
{
    public class SubTrainer
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double? Price { get; set; }
        public string Remarks { get; set; }
        public int TrainerSubscriptionId { get; set; }
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
        
    }
}
