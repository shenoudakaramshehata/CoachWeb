using System;

namespace Coach.ReportsModel
{
    public class TournamentFilterModel
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int? CountryId { get; set; }
        public string UserId { get; set; }
        public int? TournamentId { get; set; }
    }
}
