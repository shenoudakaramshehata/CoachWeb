using Coach.Data;
using Coach.Models;
using Coach.Reports;
using Coach.ReportsModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Coach.Areas.Admin.Pages.ReportsPages
{
    public class SubscribedCoursesRevenuesModel : PageModel
    {
        private CoachContext _context;
        public double TotalCost { get; set; }

        [BindProperty]
        public CourseFilterModel CourseFilterModel { get; set; }


        private UserManager<ApplicationUser> _userManager { get; set; }
        public rptSubscribedCourse report { get; set; }
        public SubscribedCoursesRevenuesModel(CoachContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        public IActionResult OnGet()
        {

            //if (filterModel.From != null && filterModel.To != null)
            //{
            //    ds = ds.Where(i => i.ScheduleDate <= filterModel.ToDate && i.ScheduleDate >= filterModel.FromDate).ToList();
            //}
            //if (filterModel.ShowAll == false && filterModel.AssetTagId == null && filterModel.FromDate == null && filterModel.ToDate == null && filterModel.TechnicianId == null && filterModel.Cost == null)
            //{
            //    ds = null;
            //}
            // var ds = _context.Genders.ToList();



            return Page();

        }
        public IActionResult OnPost()
        {
            var courseSubscription = _context.Subscriptions.Where(e => e.EntityTypeId == 4&&e.ispaid==true).Select(e=>e.EntityId).ToList();
            List<CourseRPT> ds = new List<CourseRPT>();
            if (courseSubscription.Count != 0)
            {

                foreach (var item in courseSubscription)
                {
                    var course = _context.Courses.Include(i => i.CourseTarget).Include(i => i.Trainer).Where(e => e.CourseId == item).FirstOrDefault();
                    if (course != null)
                    {
                        var CourseObj = new CourseRPT()
                        {
                            Cost = course.Cost.Value,
                            CourseTargetId = course.CourseTargetId,
                            CourseTargetTitle = course.CourseTarget.CourseTargetTlEn,
                            CourseTlEn = course.CourseTlEn,
                            PublishDate = course.PublishDate,
                            TrainerId = course.TrainerId,
                            TrainerName = course.Trainer.FullNameEn
                        };
                        ds.Add(CourseObj);
                    }

                }

            }
            TotalCost = ds.Sum(a => a.Cost);
            if (CourseFilterModel.CourseTargetId == null&& CourseFilterModel.TrainerId == null)
            {
                ds = null;
                TotalCost = 0;
            }
            if (CourseFilterModel.CourseTargetId != null)
            {
                ds = ds.Where(i => i.CourseTargetId== CourseFilterModel.CourseTargetId).ToList();
                TotalCost = ds.Sum(e => e.Cost);
            }
            if (CourseFilterModel.TrainerId != null)
            {
                ds = ds.Where(i => i.TrainerId == CourseFilterModel.TrainerId).ToList();
                TotalCost = ds.Sum(e => e.Cost);
            }
            report = new rptSubscribedCourse(TotalCost);
            report.DataSource = ds;
            return Page();
        }
    }
}
