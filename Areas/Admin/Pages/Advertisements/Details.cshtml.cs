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

namespace Coach.Areas.Admin.Pages.Advertisements
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


    }
}
