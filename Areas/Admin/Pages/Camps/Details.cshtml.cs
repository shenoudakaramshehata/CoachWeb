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

namespace Coach.Areas.Admin.Pages.Camps
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
        public Camp camp { get; set; }
        [BindProperty]
        public ApplicationUser user { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {
           
            try
            {
                camp = await _context.Camps
                    .Include(c=>c.CampType)
                    .Include(c=>c.CampTarget)
                    .Include(c=>c.CampImages)
                    .Include(c=>c.Country)
                    .Include(c=>c.CampPlan)
                    .FirstOrDefaultAsync(m => m.CampId == id);


                    
                if (camp == null)
                {
                    return Redirect("../Error");
                }
                user = await _userManager.FindByIdAsync(camp.UserId);

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
                camp = await _context.Camps
                     .Include(c => c.CampType)
                     .Include(c => c.CampTarget)
                     .Include(c => c.CampImages)
                     .Include(c => c.Country)
                     .Include(c => c.CampPlan)
                     .FirstOrDefaultAsync(m => m.CampId == id);
                if (camp == null)
                {
                    _toastNotification.AddErrorToastMessage("Something went wrong");
                }
                user = await _userManager.FindByIdAsync(camp.UserId);
                camp.ispaid = true;
                _context.Attach(camp).State = EntityState.Modified;
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
