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

namespace Coach.Areas.Admin.Pages.Countries
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
        public Country country { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {
           
            try
            {
                country = await _context.Countries.FirstOrDefaultAsync(m => m.CountryId == id);
                if (country == null)
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
               var model = await _context.Countries.FindAsync(id);
                if (model != null)
                {
                    var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/Country/" + model.CountryPic);

                    if ( 
                            _context.CampPlans.Any(c => c.CountryId == id)
                        || _context.Camps.Any(c => c.CountryId == id)
                        || _context.Adzs.Any(c => c.CountryId == id)
                        || _context.PublicDevices.Any(c => c.CountryId == id)
                        || _context.PublicNotifications.Any(c => c.CountryId == id)
                        || _context.TournamentPlans.Any(c => c.CountryId == id)
                        || _context.Tournaments.Any(c => c.CountryId == id)
                        || _context.TrainerPlans.Any(c => c.CountryId == id)
                        || _context.Trainers.Any(c => c.CountryId == id)
                        )
                    {
                        _toastNotification.AddErrorToastMessage("You cannot delete this Country");
                        return Page();
                    }
           

                    _context.Countries.Remove(model);
                    await _context.SaveChangesAsync();
                    if (System.IO.File.Exists(ImagePath))
                    {
                        System.IO.File.Delete(ImagePath);
                    }
                    _toastNotification.AddSuccessToastMessage("Country Deleted successfully");

                }
            }
            catch (Exception )

            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
                return Page();

            }

            return RedirectToPage("./Index");
        }

    }
}
