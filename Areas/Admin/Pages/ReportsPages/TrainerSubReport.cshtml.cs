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
    public class TrainerSubReportModel : PageModel
    {
        private CoachContext _context;
        public double TotalCost { get; set; }

        [BindProperty]
        public FilterModel filterModel { get; set; }


        private UserManager<ApplicationUser> _userManager { get; set; }
        public rptTrainerSubRevenue report { get; set; }
        public TrainerSubReportModel(CoachContext context, UserManager<ApplicationUser> userManager)
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
            TotalCost = _context.TrainerSubscriptions.Sum(a => a.Price.Value);
            List<SubTrainer> ds = _context.TrainerSubscriptions.Where(e=>e.ispaid==true).Include(i => i.Trainer).Include(i=>i.TrainerPlan).Select(i => new SubTrainer
            {
                StartDate = i.StartDate,
                EndDate = i.EndDate,
                Price = i.Price,
                Country = _context.Countries.Where(a => a.CountryId == i.Trainer.CountryId).FirstOrDefault().CountryTlEn,
                FullNameAr=i.Trainer.FullNameAr,
                FullNameEn=i.Trainer.FullNameEn,
                Mobile=i.Trainer.Mobile,
                Email=i.Trainer.Email,
                Gender= _context.Genders.Where(a => a.GenderId == i.Trainer.GenderId).FirstOrDefault().GenderTlEn,
                TrainerPlan= i.TrainerPlan.PlanTlEn,
                Pic=i.Trainer.Pic,
                TrainerSubscriptionId=i.TrainerSubscriptionId,  
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
                ds = ds.Where(i => i.StartDate <= filterModel.To && i.StartDate >= filterModel.From).ToList();
                TotalCost = ds.Sum(e => e.Price.Value);
            }
            report = new rptTrainerSubRevenue(TotalCost);
            report.DataSource = ds;
            return Page();
        }
    }
}
