using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Coach.Data;
using Coach.Models;
using NToastNotify;
using Microsoft.AspNetCore.Identity;

namespace Coach.Areas.Admin.Pages.Tournaments
{
    public class DetailsModel : PageModel
    {


        private CoachContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IToastNotification _toastNotification;
        public DetailsModel(CoachContext context, UserManager<ApplicationUser> userManager, IToastNotification toastNotification)
        {
            _context = context;
            _userManager = userManager;

            _toastNotification = toastNotification;

        }
        [BindProperty]
        public Tournament tournament { get; set; }
        [BindProperty]
        public ApplicationUser user { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {
           
            try
            {
                tournament = await _context.Tournaments
                    .Include(c=>c.TournamentType)
                    .Include(c=>c.TournamentTarget)
                    .Include(c=>c.TournamentImages)
                    .Include(c=>c.Country)
                    .Include(c=>c.TournamentPlan)
                    .FirstOrDefaultAsync(m => m.TournamentId == id);


                    
                if (tournament == null)
                {
                    return Redirect("../Error");
                }
                user = await _userManager.FindByIdAsync(tournament.UserId);

            }
            catch (Exception)
            {

               _toastNotification.AddErrorToastMessage("Something went wrong");
            }



            return Page();
        }

        public async Task<IActionResult> OnPost(int id)
        {
            try
            {
                tournament = await _context.Tournaments
                    .Include(c => c.TournamentType)
                    .Include(c => c.TournamentTarget)
                    .Include(c => c.TournamentImages)
                    .Include(c => c.Country)
                    .Include(c => c.TournamentPlan)
                    .FirstOrDefaultAsync(m => m.TournamentId == id);



                if (tournament == null)
                {
                    return Redirect("../Error");
                }
                user = await _userManager.FindByIdAsync(tournament.UserId);
                tournament.ispaid = true;
                _context.Attach(tournament).State = EntityState.Modified;
                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");
            }
            return Page();
        }

    }
}
