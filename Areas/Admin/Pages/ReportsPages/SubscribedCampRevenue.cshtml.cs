using Coach.Data;
using Coach.Models;
using Coach.Reports;
using Coach.ReportsModel;
using DevExpress.DataAccess.Native.Filtering;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Coach.Areas.Admin.Pages.ReportsPages
{
    public class SubscribedCampRevenueModel : PageModel
    {
        private CoachContext _context;
        public double TotalCost { get; set; }
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public CamFilterModel CampFilterModel { get; set; }


        private UserManager<ApplicationUser> _userManager { get; set; }
        public rptSubscribedCamp report { get; set; }
        public SubscribedCampRevenueModel(CoachContext context, UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _context = context;
            _userManager = userManager;
            _db = db;
        }

        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task <IActionResult> OnPost()
        {
            var CampSubscription = _context.Subscriptions.Where(e => e.EntityTypeId == 2 && e.ispaid == true).ToList();
            List<CampSubscriptionRPT> ds = new List<CampSubscriptionRPT>();
            if (CampSubscription.Count != 0)
            {

                foreach (var item in CampSubscription)
                {
                    var Subscribeduser = await _userManager.FindByIdAsync(item.UserId);

                    var Camp = _context.Camps.Include(i => i.Country).Where(e => e.CampId == item.EntityId).FirstOrDefault();
                    var Addeduser = await _userManager.FindByIdAsync(Camp.UserId);

                    if (Camp != null)
                    {
                        var CampObj = new CampSubscriptionRPT()
                        {CampId= Camp.CampId,
                            Cost = Camp.Cost.Value,
                            CountryId = Camp.CountryId,
                            CampTlEn = Camp.CampTlEn,
                            StartDate = Camp.StartDate,
                            EndDate = Camp.EndDate,
                            Pic = Camp.Pic,
                            Country= Camp.Country.CountryTlEn,
                            SubscriberName= Subscribeduser.FullName,
                            UserAddedby= Addeduser.FullName
                        };
                        ds.Add(CampObj);
                    }
                }
            }
            TotalCost = ds.Sum(a => a.Cost);
            if (CampFilterModel.CountryId == null && CampFilterModel.CampId == null&& CampFilterModel.From == null && CampFilterModel.To == null)
            {
                ds = null;
                TotalCost = 0;
            }
            if (CampFilterModel.From != null && CampFilterModel.To == null)
            {
                ds = null;
            }
            if (CampFilterModel.From == null && CampFilterModel.To != null)
            {
                ds = null;
            }
            if (CampFilterModel.From != null && CampFilterModel.To != null)
            {
                ds = ds.Where(i => i.StartDate <= CampFilterModel.To && i.StartDate >= CampFilterModel.From).ToList();
                TotalCost = ds.Sum(e => e.Cost);
            }
            if (CampFilterModel.CountryId != null)
            {
                ds = ds.Where(i => i.CountryId == CampFilterModel.CountryId).ToList();
                TotalCost = ds.Sum(e => e.Cost);
            }
            if (CampFilterModel.CampId != null)
            {
                ds = ds.Where(i => i.CampId == CampFilterModel.CampId).ToList();
                TotalCost = ds.Sum(e => e.Cost);
            }
            //if (CampFilterModel.UserId != null)
            //{
            //    var campsId = _context.Camps.Where(e => e.UserId == CampFilterModel.UserId).Select(e => e.CampId).ToList();
            //    ds = ds.Where(e=>e.CampId==campsId.Any())
            //    TotalCost = ds.Sum(e => e.Cost);
            //}
            report = new rptSubscribedCamp(TotalCost);
            report.DataSource = ds;
            return Page();
        }
    }
}
