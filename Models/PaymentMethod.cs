using System;
using System.Collections.Generic;

#nullable disable

namespace Coach.Models
{
    public partial class PaymentMethod
    {
        public PaymentMethod()
        {
            Subscriptions = new HashSet<Subscription>();
        }

        public int PaymentMethodId { get; set; }
        public string PaymentMethodTlar { get; set; }
        public string PaymentMethodTlEn { get; set; }

        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }
}
