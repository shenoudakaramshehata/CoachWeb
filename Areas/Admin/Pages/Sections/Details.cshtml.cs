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

namespace Coach.Areas.Admin.Pages.Sections
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
        public Section section { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {
            
            try
            {
                section = await _context.Sections.FirstOrDefaultAsync(m => m.SectionId == id);
                
                if (section == null)
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
