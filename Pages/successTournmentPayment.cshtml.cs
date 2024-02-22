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
using Microsoft.AspNetCore.Identity;

namespace Coach.Pages
{
    public class successTournmentPaymentModel : PageModel
    {
        private CoachContext _context;
        public Course Course { get; set; }
        public Tournament Tournament { get; set; }

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private IHostingEnvironment _env;
        public successTournmentPaymentModel(CoachContext context, IEmailSender emailSender, IHostingEnvironment env, UserManager<ApplicationUser> userManager)
        {
            _emailSender = emailSender;
            _context = context;
            _env = env;
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync(string payment_type, string PaymentID, string Result, int OrderID, DateTime? PostDate, string TranID,
        string Ref, string TrackID, string Auth)
        {
            try
            {
                if (OrderID != 0)
                {
                    Tournament = _context.Tournaments.Find(OrderID);

                    Tournament.ispaid = true;
                    Tournament.payment_type = payment_type;
                    Tournament.PaymentID = PaymentID;
                    Tournament.Result = Result;
                    Tournament.TournamentId = OrderID;
                    Tournament.PostDate = PostDate;
                    Tournament.TranID = TranID;
                    Tournament.Ref = Ref;
                    Tournament.TrackID = TrackID;
                    Tournament.Auth = Auth;
                    var UpdatedOrder = _context.Tournaments.Attach(Tournament);
                    UpdatedOrder.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                    var user = await _userManager.FindByIdAsync(Tournament.UserId);
                    var TournmentPlan = _context.TournamentPlans.Where(e => e.TournamentPlanId == Tournament.TournamentPlanId).FirstOrDefault();

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
                       Tournament.PlanStartDate.Value.ToShortDateString(),
                       Tournament.PlanStartDate.Value.ToShortDateString(),
                       Tournament.SubPrice,
                       user.FullName,
                       TournmentPlan.PlanTlAr

                       );
                    await _emailSender.SendEmailAsync(user.Email, "Tournment Plan Subscription", messageBody);




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
