using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Coach.Data;
using Coach.Models;
using NToastNotify;

namespace Coach.Areas.Admin.Pages.Banners
{
    public class DetailsModel : PageModel
    {
        private CoachContext _context;
        private readonly IToastNotification _toastNotification;

        public DetailsModel(CoachContext context,  IToastNotification toastNotification)
        {
            _context = context;
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



    }
}
