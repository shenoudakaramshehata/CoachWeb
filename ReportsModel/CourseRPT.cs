using System;

namespace Coach.ReportsModel
{
    public class CourseRPT
    {
        public string CourseTlEn { get; set; }
        public int TrainerId { get; set; }
        public int CourseTargetId { get; set; }
        public string CourseTargetTitle { get; set; }
        public string TrainerName { get; set; }

        public DateTime? PublishDate { get; set; }
        public double Cost { get; set; }
    }
}
