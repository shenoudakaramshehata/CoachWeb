using System;

namespace Coach.ReportsModel
{
    public class TournmentRPT
    {
        public int TournamentId { get; set; }
        public string User { get; set; }//uesr
        public string TournamentTlAr { get; set; }
        public string TournamentTlEn { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Pic { get; set; }
        public string TournamentType { get; set; }
        public string Country{ get; set; }
        public string TournamentTarget { get; set; }
        public string TournamentPlan { get; set; }
        public DateTime? SubStartDate { get; set; }
        public DateTime? SubEndDate { get; set; }
        public double? SubPrice { get; set; }
        public string Remarks { get; set; }
        public double? Cost { get; set; }

        //Payment
        public bool ispaid { get; set; }
    }
}
