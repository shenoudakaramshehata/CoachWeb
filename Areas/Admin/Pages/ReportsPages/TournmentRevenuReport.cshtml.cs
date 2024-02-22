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
    public class TournmentRevenuReportModel : PageModel
    {
        private CoachContext _context;
        public double TotalCost { get; set; }
        
        [BindProperty]
        public FilterModel filterModel { get; set; }


        private UserManager<ApplicationUser> _userManager { get; set; }
        public rptTournmentRevenu report { get; set; }
        public TournmentRevenuReportModel(CoachContext context, UserManager<ApplicationUser> userManager)
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
            TotalCost = _context.Tournaments.Sum(a => a.Cost).Value;
            List<TournmentRPT> ds = _context.Tournaments.Include(i => i.TournamentPlan).Include(i => i.TournamentTarget).Include(i => i.TournamentType).Include(i => i.Country).Select(i => new TournmentRPT
            {
                Cost = i.Cost,
                StartDate = i.StartDate,
                EndDate = i.EndDate,
                Country = i.Country.CountryTlEn,
                ispaid = i.ispaid,
                TournamentTlAr = i.TournamentTlAr,
                TournamentTlEn = i.TournamentTlEn,
                IsActive = i.IsActive,
                Pic = i.Pic,
                Remarks = i.Remarks,
                SubPrice = i.SubPrice,
                SubEndDate = i.SubEndDate,
                SubStartDate = i.SubStartDate,
                TournamentId = i.TournamentId,
                TournamentPlan = i.TournamentPlan.PlanTlEn,
                TournamentTarget = i.TournamentTarget.TournamentTargetTlEn,
                TournamentType = i.TournamentType.TournamentTypeTlEn

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
            report = new rptTournmentRevenu(TotalCost);
            report.DataSource = ds;
            return Page();
        }
    }
}

