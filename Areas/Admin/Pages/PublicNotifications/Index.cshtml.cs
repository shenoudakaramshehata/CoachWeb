using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Coach.Data;
using Coach.Models;
using Microsoft.AspNetCore.Localization;
using NToastNotify;

namespace Coach.Areas.Admin.Pages.PublicNotifications
{
    public class IndexModel : PageModel
    {
        private CoachContext _context;
        private readonly IToastNotification _toastNotification;

        public IndexModel(CoachContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }

        [BindProperty(SupportsGet = true)]
        public List<PublicNotification> PublicNotificationLst { get; set; }

        public void OnGet()
        {
            try
            {

                PublicNotificationLst = _context.PublicNotifications.ToList();

                foreach (var item in PublicNotificationLst)
                {



                    if (item.EntityTypeId == 2)
                    {
                        item.EntityNameAr = _context.Camps.FirstOrDefault(c => c.CampId == item.EntityId)?.CampTlAr;
                        item.EntityNameEn = _context.Camps.FirstOrDefault(c => c.CampId == item.EntityId)?.CampTlEn;
                    }
                    if (item.EntityTypeId == 3)
                    {
                        item.EntityNameAr = _context.Tournaments.FirstOrDefault(c => c.TournamentId == item.EntityId)?.TournamentTlAr;
                        item.EntityNameEn = _context.Tournaments.FirstOrDefault(c => c.TournamentId == item.EntityId)?.TournamentTlEn;
                    }
                    if (item.EntityTypeId == 4)
                    {
                        item.EntityNameAr = _context.Courses.FirstOrDefault(c => c.CourseId == item.EntityId)?.CourseTlAr;
                        item.EntityNameEn = _context.Courses.FirstOrDefault(c => c.CourseId == item.EntityId)?.CourseTlEn;
                    }


                }
            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");
            }
            }
        }
}
