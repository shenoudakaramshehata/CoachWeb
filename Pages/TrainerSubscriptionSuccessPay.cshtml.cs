using Coach.Data;
using Coach.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using MimeKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
namespace Coach.Pages
{
    public class TrainerSubscriptionSuccessPayModel : PageModel
    {
        private CoachContext _context;
        public TrainerSubscription trainerSubscription { get; set; }
        private IHostingEnvironment _env;
        private readonly IEmailSender _emailSender;
        public TrainerSubscriptionSuccessPayModel(CoachContext context, IEmailSender emailSender, IHostingEnvironment env)
        {
            _emailSender = emailSender;
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> OnGetAsync(string payment_type, string PaymentID, string Result, int OrderID, DateTime? PostDate, string TranID,
        string Ref, string TrackID, string Auth)
        {
            try
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
                        trainerSubscription.TrainerSubscriptionId = OrderID;
                        trainerSubscription.PostDate = DateTime.Now;
                        trainerSubscription.TranID = TranID;
                        trainerSubscription.Ref = Ref;
                        trainerSubscription.TrackID = TrackID;
                        trainerSubscription.Auth = Auth;
                        var UpdatedOrder = _context.TrainerSubscriptions.Attach(trainerSubscription);
                        UpdatedOrder.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        _context.SaveChanges();
                        var trainer = _context.Trainers.Where(e => e.TrainerId == trainerSubscription.TrainerId).FirstOrDefault();
                        var trainerPlan = _context.TrainerPlans.Where(e => e.TrainerPlanId == trainerSubscription.TrainerPlanId).FirstOrDefault();

                        var webRoot = _env.WebRootPath;

                        var pathToFile = _env.WebRootPath
                               + Path.DirectorySeparatorChar.ToString()
                               + "Templates"
                               + Path.DirectorySeparatorChar.ToString()
                               + "EmailTemplate"
                               + Path.DirectorySeparatorChar.ToString()
                               + "Email.html";
                        var builder = new BodyBuilder();
                        using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                        {

                            builder.HtmlBody = SourceReader.ReadToEnd();

                        }
                        string messageBody = string.Format(builder.HtmlBody,
                           trainerSubscription.StartDate.Value.ToShortDateString(),
                           trainerSubscription.EndDate.Value.ToShortDateString(),
                           trainerSubscription.Price,
                           trainer.FullNameAr,
                           trainerPlan.PlanTlAr

                           );
                        await _emailSender.SendEmailAsync(trainer.Email, "Trainer Subscription", messageBody);
                    }
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
