using Coach.Data;
using Coach.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Coach.Pages
{
    public class failedModel : PageModel
    {
        private CoachContext _context;
        public Camp Camp { get; set; }

        public failedModel(CoachContext context)
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

                    Camp = _context.Camps.Find(OrderID);

                    if (Camp != null)
                    {
                        _context.Camps.Remove(Camp);
                    }
                }
            } catch(Exception ex)
            {
                return RedirectToPage("SomethingwentError");
            }

            return Page();
        }
    }
}
