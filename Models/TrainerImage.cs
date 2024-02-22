using System;
using System.Collections.Generic;

#nullable disable

namespace Coach.Models
{
    public partial class TrainerImage
    {
        public int TrainerImageId { get; set; }
        public int TrainerId { get; set; }
        public string Pic { get; set; }

        public virtual Trainer Trainer { get; set; }
    }
}
