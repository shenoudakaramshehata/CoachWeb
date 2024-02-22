using System;
using System.Collections.Generic;

#nullable disable

namespace Coach.Models
{
    public partial class CourseTarget
    {
        public CourseTarget()
        {
            Courses = new HashSet<Course>();
        }

        public int CourseTargetId { get; set; }
        public string CourseTargetTlAr { get; set; }
        public string CourseTargetTlEn { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
