using System;
using System.Collections.Generic;

#nullable disable

namespace Coach.Models
{
    public partial class EntityType
    {
        public EntityType()
        {
            Adzs = new HashSet<Adz>();
            Subscriptions = new HashSet<Subscription>();
        }

        public int EntityTypeId { get; set; }
        public string EntityTypeTlar { get; set; }
        public string EntityTypeTlen { get; set; }

        public virtual ICollection<Adz> Adzs { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }
}
