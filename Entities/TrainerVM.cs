using System;

namespace Coach.Entities
{
    public class TrainerVM
    {
        public string UserId { get; set; }
        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Tele { get; set; }
        public string Fax { get; set; }
        public int? GenderId { get; set; }
        public int? CountryId { get; set; }
        public int? TrainerPlanId { get; set; }
        public string Pic { get; set; }
        public int SectionId { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public int PaymentMethodId { get; set; }
       
       
     
    }
}
