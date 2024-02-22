using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Coach.Data;
using Coach.Models;
using NToastNotify;

namespace Coach.Areas.Admin.Pages.Advertisements
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
        public bool ArLang { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<Adz> AdzLst { get; set; }

        public void OnGet()
        {
            try
            {
                AdzLst = _context.Adzs.ToList();
                foreach (var item in AdzLst)
                {
                    if (item.EntityId == "" || item.EntityId == null || item.EntityTypeId == 5)
                        continue;

                    var id = Convert.ToInt32(item.EntityId);
                    if (item.EntityTypeId == 1)
                    {
                        item.EntityId = _context.Trainers.FirstOrDefault(c => c.TrainerId == id)?.FullNameAr;
                    }

                    if (item.EntityTypeId == 2)
                    {
                        item.EntityId = _context.Camps.FirstOrDefault(c => c.CampId == id)?.CampTlAr;
                    }
                    if (item.EntityTypeId == 3)
                    {
                        item.EntityId = _context.Tournaments.FirstOrDefault(c => c.TournamentId == id)?.TournamentTlAr;
                    }
                    if (item.EntityTypeId == 4)
                    {
                        item.EntityId = _context.Courses.FirstOrDefault(c => c.CourseId == id)?.CourseTlAr;
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
