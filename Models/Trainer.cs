using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

#nullable disable

namespace Coach.Models
{
    public partial class Trainer
    {
        public Trainer()
        {
            Courses = new HashSet<Course>();
            TrainerImages = new HashSet<TrainerImage>();
            TrainerSubscriptions = new HashSet<TrainerSubscription>();
        }

        public int TrainerId { get; set; }
        public string UserId { get; set; }
        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Tele { get; set; }
        public string Fax { get; set; }
        public int? GenderId { get; set; }
        public int? CountryId { get; set; }
        public string Pic { get; set; }
        public int SectionId { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public virtual Country Country { get; set; }
        public virtual Gender Gender { get; set; }
        public DateTime? AddedDate { get; set; }
        [JsonIgnore]
        public virtual Section Section { get; set; }
        [JsonIgnore]
        public virtual ICollection<Course> Courses { get; set; }
        [JsonIgnore]
        public virtual ICollection<TrainerSubscription> TrainerSubscriptions { get; set; }
        [JsonIgnore]
        public virtual ICollection<TrainerImage> TrainerImages { get; set; }
      
    }
}
