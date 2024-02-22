using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Coach.Data;
using Coach.Models;
using Microsoft.AspNetCore.Authorization;
using NToastNotify;
using Microsoft.AspNetCore.Identity;

namespace Coach.Areas.Admin.Pages.Subscriptions
{

    public class DetailsModel : PageModel
    {
        private CoachContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IToastNotification _toastNotification;
        private readonly ApplicationDbContext _db;

        public DetailsModel(ApplicationDbContext db,CoachContext context, UserManager<ApplicationUser> userManager, IToastNotification toastNotification)
        {
            _context = context;
            _userManager = userManager;

            _toastNotification = toastNotification;
            _db = db;


        }
      
        [BindProperty]
        public Subscription subscription { get; set; }
        [BindProperty]
        public ApplicationUser user { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {

            try
            {
                subscription = _context.Subscriptions.Include(c => c.EntityType).Include(c=>c.PaymentMethod).Where(c => c.SubscriptionId == id).FirstOrDefault();

                if (subscription == null)
                {
                    return Redirect("../Error");
                }

               

                if (subscription.EntityTypeId == 2)
                {
                    subscription.EntityName = _context.Camps.FirstOrDefault(c => c.CampId == Convert.ToInt32(subscription.EntityId))?.CampTlAr;
                }
                if (subscription.EntityTypeId == 3)
                {
                    subscription.EntityName = _context.Tournaments.FirstOrDefault(c => c.TournamentId == Convert.ToInt32(subscription.EntityId))?.TournamentTlAr;
                }
                if (subscription.EntityTypeId == 4)
                {
                    subscription.EntityName = _context.Courses.FirstOrDefault(c => c.CourseId == Convert.ToInt32(subscription.EntityId))?.CourseTlAr;
                }
                user = await _userManager.FindByIdAsync(subscription.UserId);
            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");

            }

            return Page();
        }

        public async Task<IActionResult> OnPost(int id)
        {
            try
            {
                subscription = _context.Subscriptions.Include(c => c.EntityType).Include(c => c.PaymentMethod).Where(c => c.SubscriptionId == id).FirstOrDefault();

                if (subscription == null)
                {
                    return Redirect("../Error");
                }



                if (subscription.EntityTypeId == 2)
                {
                    subscription.EntityName = _context.Camps.FirstOrDefault(c => c.CampId == Convert.ToInt32(subscription.EntityId))?.CampTlAr;
                }
                if (subscription.EntityTypeId == 3)
                {
                    subscription.EntityName = _context.Tournaments.FirstOrDefault(c => c.TournamentId == Convert.ToInt32(subscription.EntityId))?.TournamentTlAr;
                }
                if (subscription.EntityTypeId == 4)
                {
                    subscription.EntityName = _context.Courses.FirstOrDefault(c => c.CourseId == Convert.ToInt32(subscription.EntityId))?.CourseTlAr;
                }
                user = await _userManager.FindByIdAsync(subscription.UserId);
                subscription.ispaid = true;
                _context.Attach(subscription).State = EntityState.Modified;
                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");
            }
            return Page();
        }

    }
}
