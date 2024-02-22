using System;

namespace Coach.ViewModel
{
    public class EditCourseVm
    {
        public int CourseId { get; set; }
        public string CourseTlAr { get; set; }
        public string CourseTlEn { get; set; }
        public int CourseTargetId { get; set; }
        public string CourseDescAr { get; set; }
        public string CourseDescEn { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? PublishDate { get; set; }
        public double Cost { get; set; }
    }
}
