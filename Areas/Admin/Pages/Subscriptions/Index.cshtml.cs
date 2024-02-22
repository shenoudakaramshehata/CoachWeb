using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coach.Data;
using Coach.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;

namespace Coach.Areas.Admin.Pages.Subscriptions
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
        public List<Subscription> List { get; set; }

        public void OnGet()
        {
            try
            {
                List = _context.Subscriptions.ToList();
                foreach (var item in List)
                {
                   

                    var id = Convert.ToInt32(item.EntityId);
                  
                    if (item.EntityTypeId == 2)
                    {
                        item.EntityName = _context.Camps.FirstOrDefault(c => c.CampId == id)?.CampTlAr;
                    }
                    if (item.EntityTypeId == 3)
                    {
                        item.EntityName = _context.Tournaments.FirstOrDefault(c => c.TournamentId == id)?.TournamentTlAr;
                    }
                    if (item.EntityTypeId == 4)
                    {
                        item.EntityName = _context.Courses.FirstOrDefault(c => c.CourseId == id)?.CourseTlAr;
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
