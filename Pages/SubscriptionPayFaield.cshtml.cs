using Coach.Data;
using Coach.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
namespace Coach.Pages
{
    public class SubscriptionPayFaieldModel : PageModel
    {
        private CoachContext _context;
        public Subscription subscription { get; set; }

        public SubscriptionPayFaieldModel(CoachContext context)
        {
            _context = context;
        }
        public IActionResult OnGet(string payment_type, string PaymentID, string Result, int OrderID, DateTime? PostDate, string TranID,
        string Ref, string TrackID, string Auth)
        {
            try
            {
                if (OrderID != 0)
                {
                    subscription = _context.Subscriptions.Where(e => e.SubscriptionId == OrderID).FirstOrDefault();
                    if (subscription != null)
                    {
                        _context.Subscriptions.Remove(subscription);
                        _context.SaveChanges();
                    }
                }

            }
            catch (Exception)
            {
                return RedirectToPage("SomethingwentError");
            }

            return Page();

        }
    }
}
