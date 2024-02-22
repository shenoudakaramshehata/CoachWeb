using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace Coach.Models
{
    public partial class Section
    {
        public Section()
        {
            Trainers = new HashSet<Trainer>();
        }

        public int SectionId { get; set; }
        public string SectionPic { get; set; }
        public string SectionOrderIndex { get; set; }
        public bool? IsActive { get; set; }
        public string SectionTlAr { get; set; }
        public string SectionTlEn { get; set; }
        [JsonIgnore]

        public virtual ICollection<Trainer> Trainers { get; set; }
    }
}
