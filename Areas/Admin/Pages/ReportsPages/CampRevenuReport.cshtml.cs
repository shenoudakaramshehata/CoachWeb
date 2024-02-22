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
    public class CampRevenuReportModel : PageModel
    {
        private CoachContext _context;
        public double TotalCost { get; set; }

        [BindProperty]
        public FilterModel filterModel { get; set; }


        private UserManager<ApplicationUser> _userManager { get; set; }
        public rptCampRevenue report { get; set; }
        public CampRevenuReportModel(CoachContext context, UserManager<ApplicationUser> userManager)
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
            TotalCost = _context.Camps.Sum(a => a.Cost).Value;
            List<CampPRT> ds = _context.Camps.Include(i => i.CampPlan).Include(i => i.CampTarget).Include(i => i.CampType).Include(i => i.Country).Select(i => new CampPRT
            {
                Cost = i.Cost,
                StartDate = i.StartDate,
                EndDate = i.EndDate,
                Country = i.Country.CountryTlEn,
                ispaid = i.ispaid,
                CampTlAr = i.CampTlAr,
                CampTlEn = i.CampTlEn,
                IsActive = i.IsActive,
                Pic = i.Pic,
                Remarks = i.Remarks,
                SubPrice = i.SubPrice,
                CampId = i.CampId,
                CampPlan = i.CampPlan.PlanTlEn,
                CampTarget = i.CampTarget.CampTargetTlEn,
                CampType = i.CampType.CampTypeTlEn,
                PostDate=i.PostDate,
                

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
                TotalCost = ds.Sum(e => e.Cost).Value;
            }
            report = new rptCampRevenue(TotalCost);
            report.DataSource = ds;
            return Page();
        }
    }
}
