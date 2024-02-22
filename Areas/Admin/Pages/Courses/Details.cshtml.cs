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

namespace Coach.Areas.Admin.Pages.Courses
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
        public Course course { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {
          
            try
            {
                course = await _context.Courses
                    .Include(c=>c.Trainer)
                    .Include(c=>c.CourseTarget)
                    .FirstOrDefaultAsync(m => m.CourseId == id);
                
                if (course == null)
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
