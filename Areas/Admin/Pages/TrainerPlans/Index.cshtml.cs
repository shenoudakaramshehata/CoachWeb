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

namespace Coach.Areas.Admin.Pages.TrainerPlans
{
    public class IndexModel : PageModel
    {

        private CoachContext _context;
        private readonly IToastNotification _toastNotification;
        public IndexModel(CoachContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }
        [BindProperty(SupportsGet = true)]
        public List<TrainerPlan> planList { get; set; }
        public async Task<IActionResult> OnGet()
        {
            try
            {
                planList = await _context.TrainerPlans.ToListAsync();
            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");

            }
            return Page();
            
        }
    }
}
