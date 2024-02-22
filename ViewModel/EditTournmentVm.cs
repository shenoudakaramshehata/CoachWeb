using System;

namespace Coach.ViewModel
{
    public class EditTournmentVm
    {
        public int TournmentId { get; set; }//uesr
        public string TournamentTlAr { get; set; }
        public string TournamentTlEn { get; set; }
        public string TournamentDescAr { get; set; }
        public string TournamentDescEn { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime JoinStart { get; set; }
        public DateTime JoinEnd { get; set; }
        public int TournamentTypeId { get; set; }
        public int CountryId { get; set; }
        public int TournamentTargetId { get; set; }
        public string Remarks { get; set; }
        public double Cost { get; set; }
    }
}
