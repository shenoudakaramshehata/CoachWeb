using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Coach.Models
{
    public partial class Subscription
    {
        public int SubscriptionId { get; set; }
        public DateTime? SubDate { get; set; }
        public string UserId { get; set; }
        [NotMapped]
        public string EntityName { get; set; }
        public int EntityTypeId { get; set; }
        public int? EntityId { get; set; }
        public double? Cost { get; set; }
        public int? PaymentMethodId { get; set; }
        public bool ispaid { get; set; }
        public string payment_type { get; set; }
        public string PaymentID { get; set; }
        public string Result { get; set; }
        public DateTime? PostDate { get; set; }
        public string TranID { get; set; }
        public string Ref { get; set; }
        public string TrackID { get; set; }
        public string Auth { get; set; }


        public virtual EntityType EntityType { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
       
    }
}
