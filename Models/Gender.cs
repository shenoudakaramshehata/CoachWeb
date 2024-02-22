using System;
using System.Collections.Generic;

#nullable disable

namespace Coach.Models
{
    public partial class Gender
    {
        public Gender()
        {
            Trainers = new HashSet<Trainer>();
        }

        public int GenderId { get; set; }
        public string GenderTlAr { get; set; }
        public string GenderTlEn { get; set; }

        public virtual ICollection<Trainer> Trainers { get; set; }
    }
}
