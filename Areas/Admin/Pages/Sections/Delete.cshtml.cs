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
using Microsoft.AspNetCore.Localization;

namespace Coach.Areas.Admin.Pages.Sections
{
    public class DeleteModel : PageModel
    {


        private CoachContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;
        public DeleteModel(CoachContext context, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
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




        public async Task<IActionResult> OnPostAsync(int id)
        {
            try
            {

                
                section = await _context.Sections.FindAsync(id);
                if (section != null)
                {
                    if (_context.Trainers.Any(c => c.SectionId == id) )
                    {
                        _toastNotification.AddErrorToastMessage("You cannot delete this Section");
                        return Page();
                    }
                    var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/Section/" + section.SectionPic);

                    _context.Sections.Remove(section);
                    await _context.SaveChangesAsync();
                    if (System.IO.File.Exists(ImagePath))
                    {
                        System.IO.File.Delete(ImagePath);
                    }
                }
                _toastNotification.AddSuccessToastMessage("Section Deleted successfully");

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
