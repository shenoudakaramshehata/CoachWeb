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
using Microsoft.Extensions.Localization;
using NToastNotify;
using Coach.Data;
using Coach.Models;

namespace Coach.Areas.Admin.Pages.Advertisements
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
        public Adz adz { get; set; }
        [BindProperty]
        public string EntityName { get; set; }

        public IActionResult OnGet(int id)
        {
            try
            {
                adz = _context.Adzs.Include(c => c.EntityType).Include(c => c.Country).Where(c => c.AdzId == id).FirstOrDefault();

                if (adz == null)
                {
                    return Redirect("../Error");
                }

                if (adz.EntityTypeId == 1)
                {

                    EntityName = _context.Trainers.FirstOrDefault(c => c.TrainerId == Convert.ToInt32(adz.EntityId))?.FullNameAr;
                }

                if (adz.EntityTypeId == 2)
                {
                    EntityName = _context.Camps.FirstOrDefault(c => c.CampId == Convert.ToInt32(adz.EntityId))?.CampTlAr;
                }
                if (adz.EntityTypeId == 3)
                {
                    EntityName = _context.Tournaments.FirstOrDefault(c => c.TournamentId == Convert.ToInt32(adz.EntityId))?.TournamentTlAr;
                }
                if (adz.EntityTypeId == 4)
                {
                    EntityName = _context.Courses.FirstOrDefault(c => c.CourseId == Convert.ToInt32(adz.EntityId))?.CourseTlAr;
                }
                if (adz.EntityTypeId == 5)
                {
                    EntityName = adz.EntityId;
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
                var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/Adz/" + adz.AdzPic);

                
                adz = await _context.Adzs.FindAsync(id);
                if (adz != null)
                {
                    _context.Adzs.Remove(adz);
                    await _context.SaveChangesAsync();
                    if (System.IO.File.Exists(ImagePath))
                    {
                        System.IO.File.Delete(ImagePath);
                    }
                    _context.SaveChanges();
                    _toastNotification.AddSuccessToastMessage("Adz Deleted successfully");

                }
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
