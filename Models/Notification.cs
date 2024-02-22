using System;
using System.Collections.Generic;

#nullable disable

namespace Coach.Models
{
    public partial class Notification
    {
        public int NotificationId { get; set; }
        public int TrainerId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
    }
}
