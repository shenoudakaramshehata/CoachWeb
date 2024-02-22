using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coach.ViewModel
{
    public class CourseVM
    {
        public string CourseTlAr { get; set; }
        public string CourseTlEn { get; set; }
        public int TrainerId { get; set; }
        public int CourseTargetId { get; set; }
        public string CourseDescAr { get; set; }
        public string CourseDescEn { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? PublishDate { get; set; }
        public double Cost { get; set; }

    }
}
