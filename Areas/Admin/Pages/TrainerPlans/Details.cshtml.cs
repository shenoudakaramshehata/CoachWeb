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
using Coach.Data;
using Coach.Models;
using NToastNotify;

namespace Coach.Areas.Admin.Pages.TrainerPlans
{
    public class DetailsModel : PageModel
    {

        private CoachContext _context;
        private readonly IToastNotification _toastNotification;
        public DetailsModel(CoachContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }

        [BindProperty]
        public TrainerPlan plan { get; set; }

        [BindProperty]
        public string countryName { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            
            try
            {
                plan = await _context.TrainerPlans.FirstOrDefaultAsync(m => m.TrainerPlanId == id);
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





    }
}
