using System;
using System.Collections.Generic;

#nullable disable

namespace Coach.Models
{
    public partial class PublicDevice
    {
        public PublicDevice()
        {
            PublicNotificationDevice = new HashSet<PublicNotificationDevice>();
        }
        public int PublicDeviceId { get; set; }

        public int CountryId { get; set; }

        public string DeviceId { get; set; }

        public bool IsAndroiodDevice { get; set; }


        public virtual Country Country { get; set; }
        public virtual ICollection<PublicNotificationDevice> PublicNotificationDevice { get; set; }

    }
}
