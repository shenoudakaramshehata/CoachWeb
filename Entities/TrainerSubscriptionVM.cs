using System;

namespace Coach.Entities
{
    public class TrainerSubscriptionVM
    {
        public int TrainerId { get; set; }
        public int TrainerPlanId { get; set; }
        public string Remarks { get; set; }
        public int PaymentMethodId { get; set; }
    }
}
