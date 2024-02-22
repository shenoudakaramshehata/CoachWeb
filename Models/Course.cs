using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace Coach.Models
{
    public partial class Course
    {
        public Course()
        {
            CourseImages = new HashSet<CourseImage>();
        }

        public int CourseId { get; set; }
        public string CourseTlAr { get; set; }
        public string CourseTlEn { get; set; }
        public int TrainerId { get; set; } //trainer
        public int CourseTargetId { get; set; }
        public string CourseDescAr { get; set; }
        public string CourseDescEn { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? PublishDate { get; set; }
        public string Pic { get; set; }
        public double? Cost { get; set; }
        [JsonIgnore]
        public virtual CourseTarget CourseTarget { get; set; }
        [JsonIgnore]
        public virtual Trainer Trainer { get; set; }
        [JsonIgnore]
        public virtual ICollection<CourseImage> CourseImages { get; set; }
    }
}
