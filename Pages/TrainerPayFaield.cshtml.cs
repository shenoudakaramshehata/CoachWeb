using Coach.Data;
using Coach.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Coach.Pages
{
    public class TrainerPayFaieldModel : PageModel
    {
        
        private CoachContext _context;
        public Trainer trainer { get; set; }
        public TrainerPayFaieldModel(CoachContext context)
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

                    var trainerSub = _context.TrainerSubscriptions.Where(e => e.TrainerSubscriptionId == OrderID).FirstOrDefault();
                    trainer = _context.Trainers.Find(trainerSub.TrainerId);
                    if (trainer != null)
                    {
                        _context.Trainers.Remove(trainer);
                    }

                    _context.SaveChanges();
                    

                }
               
            }
            catch(Exception)
            {
               return RedirectToPage("SomethingwentError");
            }
           
            return Page();

        }
    }
}

