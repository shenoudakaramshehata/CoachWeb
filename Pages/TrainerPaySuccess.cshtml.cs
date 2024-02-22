using Coach.Data;
using Coach.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
namespace Coach.Pages
{
    public class TrainerPaySuccessModel : PageModel
    {

        private CoachContext _context;
        public TrainerSubscription trainerSubscription { get; set; }

        private readonly IEmailSender _emailSender;
        public TrainerPaySuccessModel(CoachContext context, IEmailSender emailSender)
        {
            _emailSender = emailSender;
            _context = context;
        }
        public IActionResult OnGetAsync(string payment_type, string PaymentID, string Result, int OrderID, DateTime? PostDate, string TranID,
        string Ref, string TrackID, string Auth)
        {
            if (OrderID != 0)
            {
                trainerSubscription = _context.TrainerSubscriptions.Where(e => e.TrainerSubscriptionId == OrderID).FirstOrDefault();

                if (trainerSubscription != null)
                {

                    trainerSubscription.ispaid = true;
                    trainerSubscription.payment_type = payment_type;
                    trainerSubscription.PaymentID = PaymentID;
                    trainerSubscription.Result = Result;
                    trainerSubscription.TrainerId = OrderID;
                    trainerSubscription.PostDate = PostDate;
                    trainerSubscription.TranID = TranID;
                    trainerSubscription.Ref = Ref;
                    trainerSubscription.TrackID = TrackID;
                    trainerSubscription.Auth = Auth;
                    try
                    {
                        var UpdatedOrder = _context.TrainerSubscriptions.Attach(trainerSubscription);
                        UpdatedOrder.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        var trainer = _context.Trainers.Where(e => e.TrainerId == trainerSubscription.TrainerId).FirstOrDefault();
                        _context.SaveChanges();
                        _emailSender.SendEmailAsync(
                          trainer.Email,
                        "Welcome To Caoch App",
                          $"thanks for your Paying :Mr {trainer.FullNameEn}");
                    }
                    catch (Exception ex)
                    {
                        RedirectToPage("SomethingwentError");
                    }
                    
                }

            }
            return Page();

        }

    }
}

