using Coach.Data;
using Coach.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Coach.Pages
{
    public class feildTournmentPaymentModel : PageModel
    {
        private CoachContext _context;
        public Camp Camp { get; set; }
        public Course Course { get; set; }
        public Tournament Tournament { get; set; }

        public feildTournmentPaymentModel(CoachContext context)
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

                    Tournament = _context.Tournaments.Find(OrderID);
                    if (Tournament != null)
                    {
                        _context.Tournaments.Remove(Tournament);

                    }
                    _context.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                return RedirectToPage("SomethingwentError");
            }

            return Page();

        }
    }
}

