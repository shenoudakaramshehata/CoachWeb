using Coach.Data;
using Coach.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Coach.Pages
{
    public class TrainerSubscriptionFieldPayModel : PageModel
    {
        private CoachContext _context;
        public TrainerSubscription trainerSubscription { get; set; }
        public TrainerSubscriptionFieldPayModel(CoachContext context)
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
                    trainerSubscription = _context.TrainerSubscriptions.Where(e=>e.TrainerSubscriptionId==OrderID).FirstOrDefault();
                    if (trainerSubscription != null)
                    {
                        _context.TrainerSubscriptions.Remove(trainerSubscription);
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
