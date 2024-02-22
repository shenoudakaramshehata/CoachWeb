using System;
using System.Collections.Generic;

#nullable disable

namespace Coach.Models
{
    public partial class CourseImage
    {
        public int CourseImageId { get; set; }
        public int CourseId { get; set; }
        public string Pic { get; set; }

        public virtual Course Course { get; set; }
    }
}
