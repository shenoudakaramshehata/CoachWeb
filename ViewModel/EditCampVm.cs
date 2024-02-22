using System;

namespace Coach.ViewModel
{
    public class EditCampVm
    {
        public int CampId { get; set; }
        public string CampTlAr { get; set; }
        public string CampTlEn { get; set; }
        public string CampDescAr { get; set; }
        public string CampDescEn { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime JoinStart { get; set; }
        public DateTime JoinEnd { get; set; }
        public int CampTypeId { get; set; }
        public int CountryId { get; set; }
        public int CampTargetId { get; set; }
        public double Cost { get; set; }
    }
}
