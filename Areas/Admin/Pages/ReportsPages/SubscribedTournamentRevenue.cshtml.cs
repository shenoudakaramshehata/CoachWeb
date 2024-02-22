using Coach.Data;
using Coach.Reports;
using Coach.ReportsModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coach.Areas.Admin.Pages.ReportsPages
{
    public class SubscribedTournamentRevenueModel : PageModel
    {
        private CoachContext _context;
        public double TotalCost { get; set; }
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public TournamentFilterModel tournamentFilterModel { get; set; }


        private UserManager<ApplicationUser> _userManager { get; set; }
        public rptSubscribedTournament report { get; set; }
        public SubscribedTournamentRevenueModel(CoachContext context, UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _context = context;
            _userManager = userManager;
            _db = db;
        }

        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var TournamentSubscription = _context.Subscriptions.Where(e => e.EntityTypeId == 3 && e.ispaid == true).ToList();
            List<TournamentSubscriptionRPT> ds = new List<TournamentSubscriptionRPT>();
            if (TournamentSubscription.Count != 0)
            {

                foreach (var item in TournamentSubscription)
                {
                    var Subscribeduser = await _userManager.FindByIdAsync(item.UserId);

                    var Tournament = _context.Tournaments.Include(i => i.Country).Where(e => e.TournamentId == item.EntityId).FirstOrDefault();
                    var Addeduser = await _userManager.FindByIdAsync(Tournament.UserId);

                    if (Tournament != null)
                    {
                        var TournamentObj = new TournamentSubscriptionRPT()
                        {TournamentId= Tournament.TournamentId,
                            Cost = Tournament.Cost.Value,
                            CountryId = Tournament.CountryId,
                            TournamentTlEn = Tournament.TournamentTlEn,
                            StartDate = Tournament.StartDate,
                            EndDate = Tournament.EndDate,
                            Pic = Tournament.Pic,
                            Country = Tournament.Country.CountryTlEn,
                            SubscriberName = Subscribeduser.FullName,
                            UserAddedby = Addeduser.FullName
                        };
                        ds.Add(TournamentObj);
                    }
                }
            }
            TotalCost = ds.Sum(a => a.Cost);
            if (tournamentFilterModel.CountryId == null && tournamentFilterModel.TournamentId == null && tournamentFilterModel.From == null && tournamentFilterModel.To == null)
            {
                ds = null;
                TotalCost = 0;
            }
            if (tournamentFilterModel.From != null && tournamentFilterModel.To == null)
            {
                ds = null;
            }
            if (tournamentFilterModel.From == null && tournamentFilterModel.To != null)
            {
                ds = null;
            }
            if (tournamentFilterModel.From != null && tournamentFilterModel.To != null)
            {
                ds = ds.Where(i => i.StartDate <= tournamentFilterModel.To && i.StartDate >= tournamentFilterModel.From).ToList();
                TotalCost = ds.Sum(e => e.Cost);
            }
            if (tournamentFilterModel.CountryId != null)
            {
                ds = ds.Where(i => i.CountryId == tournamentFilterModel.CountryId).ToList();
                TotalCost = ds.Sum(e => e.Cost);
            }
            if (tournamentFilterModel.TournamentId != null)
            {
                ds = ds.Where(i => i.TournamentId == tournamentFilterModel.TournamentId).ToList();
                TotalCost = ds.Sum(e => e.Cost);
            }
            //if (CampFilterModel.UserId != null)
            //{
            //    var campsId = _context.Camps.Where(e => e.UserId == CampFilterModel.UserId).Select(e => e.CampId).ToList();
            //    ds = ds.Where(e=>e.CampId==campsId.Any())
            //    TotalCost = ds.Sum(e => e.Cost);
            //}
            report = new rptSubscribedTournament(TotalCost);
            report.DataSource = ds;
            return Page();
        }
    }
}
