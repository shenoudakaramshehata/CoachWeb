using System;
using System.Collections.Generic;

#nullable disable

namespace Coach.Models
{
    public partial class Country
    {
        public Country()
        {
            Adzs = new HashSet<Adz>();
            CampPlans = new HashSet<CampPlan>();
            Camps = new HashSet<Camp>();
            PublicDevices = new HashSet<PublicDevice>();
            PublicNotifications = new HashSet<PublicNotification>();
            TournamentPlans = new HashSet<TournamentPlan>();
            Tournaments = new HashSet<Tournament>();
            TrainerPlans = new HashSet<TrainerPlan>();
            Trainers = new HashSet<Trainer>();
        }

        public int CountryId { get; set; }
        public string CountryTlAr { get; set; }
        public string CountryTlEn { get; set; }
        public string CountryPic { get; set; }
        public bool? CountryIsActive { get; set; }
        public int? CountryOrderIndex { get; set; }

        public virtual ICollection<Adz> Adzs { get; set; }
        public virtual ICollection<CampPlan> CampPlans { get; set; }
        public virtual ICollection<Camp> Camps { get; set; }
        public virtual ICollection<PublicDevice> PublicDevices { get; set; }
        public virtual ICollection<PublicNotification> PublicNotifications { get; set; }
        public virtual ICollection<TournamentPlan> TournamentPlans { get; set; }
        public virtual ICollection<Tournament> Tournaments { get; set; }
        public virtual ICollection<TrainerPlan> TrainerPlans { get; set; }
        public virtual ICollection<Trainer> Trainers { get; set; }
    }
}
