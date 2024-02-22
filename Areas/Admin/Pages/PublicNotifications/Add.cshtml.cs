using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Coach.Data;
using Coach.Models;

namespace Coach.Areas.Admin.Pages.PublicNotifications
{
    public class AddModel : PageModel
    {
        private CoachContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;

        public AddModel(CoachContext context, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;

        }


        public void OnGet()
        {

        }
        public IActionResult OnGetFillCampList(string Values)
        {

            int countryId = 0;
            bool checkTrue = int.TryParse(Values, out countryId);
            var lookup = from i in _context.Camps
                         orderby i.CampId
                         where i.CountryId == countryId && i.IsActive == true 
                         select new
                         {
                             Value = i.CampId,
                             Text = i.CampTlEn
                         };
            return new JsonResult(lookup);
        }
        public IActionResult OnGetFillTournamentList(string Values)
        {

            int countryId = 0;
            bool checkTrue = int.TryParse(Values, out countryId);
            var lookup = from i in _context.Tournaments
                         orderby i.TournamentId
                         where i.CountryId == countryId && i.IsActive == true
                         select new
                         {
                             Value = i.TournamentId,
                             Text = i.TournamentTlEn
                         };
            return new JsonResult(lookup);
        }
        public IActionResult OnGetFillCourseList(string Values)
        {

            int countryId = 0;
            bool checkTrue = int.TryParse(Values, out countryId);
            var lookup = from i in _context.Courses
                         orderby i.CourseId
                         where i.Trainer.CountryId == countryId && i.IsActive == true
                         select new
                         {
                             Value = i.CourseId,
                             Text = i.CourseTlEn
                         };
            return new JsonResult(lookup);
        }
        public IActionResult OnPost(PublicNotification model)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                model.Date = DateTime.Now;
                _context.PublicNotifications.Add(model);
                _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Notification Added successfully");
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
                return Page();
            }
          
            return Redirect("./Index");

        }
    }
}
