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

namespace Coach.Areas.Admin.Pages.Banners
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
        public Banner banner { get; set; }
        [BindProperty]
        public string EntityName { get; set; }

        public IActionResult OnGet(int id)
        {
            try
            {
                banner = _context.Banners.Where(c => c.BannerId == id).FirstOrDefault();
                if (banner == null)
                {
                    return Redirect("../Error");
                }

                if (banner.EntityTypeId == 1)
                {

                    EntityName = _context.Trainers.FirstOrDefault(c => c.TrainerId == Convert.ToInt32(banner.EntityId))?.FullNameAr;
                }

                if (banner.EntityTypeId == 2)
                {
                    EntityName = _context.Camps.FirstOrDefault(c => c.CampId == Convert.ToInt32(banner.EntityId))?.CampTlAr;
                }
                if (banner.EntityTypeId == 3)
                {
                    EntityName = _context.Tournaments.FirstOrDefault(c => c.TournamentId == Convert.ToInt32(banner.EntityId))?.TournamentTlAr;
                }
                if (banner.EntityTypeId == 4)
                {
                    EntityName = _context.Courses.FirstOrDefault(c => c.CourseId == Convert.ToInt32(banner.EntityId))?.CourseTlAr;
                }
                if (banner.EntityTypeId == 5)
                {
                    EntityName = banner.EntityId;
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
                var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/Banner/" + banner.BannerPic);

                
                banner = await _context.Banners.FindAsync(id);
                if (banner != null)
                {
                    _context.Banners.Remove(banner);
                    await _context.SaveChangesAsync();
                    if (System.IO.File.Exists(ImagePath))
                    {
                        System.IO.File.Delete(ImagePath);
                    }
                    _context.SaveChanges();
                    _toastNotification.AddSuccessToastMessage("Banner Deleted successfully");

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
