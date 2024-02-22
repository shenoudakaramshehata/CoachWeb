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

namespace Coach.Areas.Admin.Pages.Trainers
{
    public class SubscriptionDetailsModel : PageModel
    {
        private CoachContext _context;
        private readonly IToastNotification _toastNotification;
        public static int trainerSubId = 0;

        public SubscriptionDetailsModel(CoachContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }
        public TrainerSubscription Subscriptions { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {

            try
            {
                Subscriptions = await _context.TrainerSubscriptions.Include(c => c.TrainerPlan).Include(c => c.Trainer).FirstOrDefaultAsync(m => m.TrainerSubscriptionId == id);

                if (Subscriptions == null)
                {
                    return Redirect("../Error");
                }
                trainerSubId = id;
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
                Subscriptions = _context.TrainerSubscriptions.Where(e => e.TrainerSubscriptionId == trainerSubId).FirstOrDefault();
                if (Subscriptions == null)
                {
                    _toastNotification.AddErrorToastMessage("Something went wrong");
                }

                Subscriptions.ispaid = true;
                _context.Attach(Subscriptions).State = EntityState.Modified;
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
