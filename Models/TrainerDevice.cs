using System;
using System.Collections.Generic;

#nullable disable

namespace Coach.Models
{
    public partial class TrainerDevice
    {
        public int TrainerDeviceId { get; set; }
        public int TrainerId { get; set; }
        public string DeviceId { get; set; }
        public bool IsAndroiodDevice { get; set; }

        public virtual Trainer Trainer { get; set; }

    }
}
