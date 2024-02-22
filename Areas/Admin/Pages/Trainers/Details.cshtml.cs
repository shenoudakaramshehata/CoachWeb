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

namespace Coach.Areas.Admin.Pages.Trainers
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
        public Trainer trainer { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {
          
            try
            {
                trainer = await _context.Trainers
                    .Include(c=>c.Gender)
                    .Include(c=>c.Section)
                    .Include(c=>c.Country)
                    .FirstOrDefaultAsync(m => m.TrainerId == id);
                
                if (trainer == null)
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

    }
}
