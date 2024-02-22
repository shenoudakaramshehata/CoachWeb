using System;

namespace Coach.ReportsModel
{
    public class FilterModel
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int TournmentId { get; set; }
    }
}
