using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Coach.Data;
using Coach.Models;

namespace Coach.Areas.Admin.Pages.TournamentPlans
{
    public class EditModel : PageModel
    {
        private CoachContext _context;
        private readonly IToastNotification _toastNotification;
        public EditModel(CoachContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }


        [BindProperty]
        public TournamentPlan plan { get; set; }


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



            return Page();
        }




        public async Task<IActionResult> OnPostAsync(int id)
        {
           
            
            try
            {
                var model = _context.TournamentPlans.Where(c => c.TournamentPlanId == id).FirstOrDefault();
                if (model == null)
                {
                    return Page();
                }
              

                model.IsActive = plan.IsActive;
                model.PlanTlAr = plan.PlanTlAr;
                model.PlanTlEn = plan.PlanTlEn;
                model.DurationInMonth = plan.DurationInMonth;
                model.CountryId = plan.CountryId;
                model.Price = plan.Price;

                _context.Attach(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("TournamentPlan Edited successfully");

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
