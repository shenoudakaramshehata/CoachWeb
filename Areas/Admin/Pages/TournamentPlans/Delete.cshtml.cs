using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NToastNotify;
using Coach.Data;
using Coach.Models;

namespace Coach.Areas.Admin.Pages.TournamentPlans
{
    public class DeleteModel : PageModel
    {
        private CoachContext _context;
        private readonly IToastNotification _toastNotification;

        public DeleteModel(CoachContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }
        [BindProperty]
        public TournamentPlan plan { get; set; }

        [BindProperty]
        public string countryName { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {
            
            try
            {
                plan = await _context.TournamentPlans.FirstOrDefaultAsync(m => m.TournamentPlanId == id);
                if (plan == null)
                {
                    return Redirect("../Error");
                }
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong");

            }


            countryName = _context.Countries.FirstOrDefault(c => c.CountryId == plan.CountryId)?.CountryTlAr;
            return Page();
        }




        public async Task<IActionResult> OnPostAsync(int id)
        {
          

            try
            {

                
                plan = await _context.TournamentPlans.FindAsync(id);
                if (plan != null)
                {

                    if (_context.Tournaments.Any(c => c.TournamentPlanId == id))
                    {
                        _toastNotification.AddErrorToastMessage("You cannot delete this TournamentPlan");
                        return Page();
                    }
                    _context.TournamentPlans.Remove(plan);
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("TournamentPlan Deleted successfully");


                }
            }
            catch (Exception)

            {
                _toastNotification.AddErrorToastMessage("Something went wrong");

                return Page();

            }

            return RedirectToPage("./Index");
        }

    }
}
