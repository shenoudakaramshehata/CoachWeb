using Coach.Reports;
using Coach.Data;
using Coach.Models;
using Coach.ReportsModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Coach.Areas.Admin.Pages.ReportsPages
{
    public class TrainerSubscriptionReportModelModel : PageModel
    {
        private CoachContext _context;
        public double TotalCost { get; set; }

        [BindProperty]
        public FilterModel filterModel { get; set; }


        private UserManager<ApplicationUser> _userManager { get; set; }
        public rptTrainerSubscriptionRevenue report { get; set; }
        public TrainerSubscriptionReportModelModel(CoachContext context, UserManager<ApplicationUser> userManager)
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
            //TotalCost = _context.Trainers.Sum(a => a.SubscriptionCost);
            List<TrainerRPT> ds = _context.Trainers.Include(i => i.Gender).Include(i => i.Country).Include(i => i.Courses).Include(i => i.Country).Select(i => new TrainerRPT
            {
                Country = i.Country.CountryTlEn,
                AddedDate = i.AddedDate,
                DescriptionAr = i.DescriptionAr,
                DescriptionEn = i.DescriptionEn,
                Email = i.Email,
                Fax = i.Fax,
                FullNameAr = i.FullNameAr,
                FullNameEn = i.FullNameEn,
                Gender = i.Gender.GenderTlEn,
                Mobile = i.Mobile,
                Pic = i.Pic,
                Section = i.Section.SectionTlEn,
                //SubscriptionCost = i.SubscriptionCost,
                Tele = i.Tele,
                TrainerId = i.TrainerId,
                UserId = i.UserId,
                //TrainerPlan = i.TrainerPlan.PlanTlEn

            }).ToList();

            if (filterModel.From != null && filterModel.To == null)
            {
                ds = null;
                TotalCost = 0;
            }
            if (filterModel.From == null && filterModel.To != null)
            {
                ds = null;
                TotalCost = 0;
            }
            if (filterModel.From != null && filterModel.To != null)
            {
                ds = ds.Where(i => i.AddedDate <= filterModel.To && i.AddedDate >= filterModel.From).ToList();
                TotalCost = ds.Sum(e => e.SubscriptionCost);
            }
            report = new rptTrainerSubscriptionRevenue(TotalCost);
            report.DataSource = ds;
            return Page();
        }
    }
}
