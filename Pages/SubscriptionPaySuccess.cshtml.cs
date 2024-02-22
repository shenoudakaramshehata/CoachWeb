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
using Coach.Entities;
using Coach.Migrations;
using Microsoft.EntityFrameworkCore;

namespace Coach.Pages
{
    public class SubscriptionPaySuccessModel : PageModel
    {
        private CoachContext _context;
        public Subscription subscription { get; set; }
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        private IHostingEnvironment _env;
        private readonly IEmailSender _emailSender;
        public SubscriptionPaySuccessModel(CoachContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
            ApplicationDbContext db, IEmailSender emailSender, IHostingEnvironment env)
        {
            _emailSender = emailSender;
            _context = context;
            _env = env;
            _signInManager = signInManager;
            _userManager = userManager;
            _db = db;
        }
        public async Task <IActionResult> OnGetAsync(string payment_type, string PaymentID, string Result, int OrderID, DateTime? PostDate, string TranID,
        string Ref, string TrackID, string Auth)
        {
            try
            {
                if (OrderID != 0)
                {
                    subscription = _context.Subscriptions.Where(e => e.SubscriptionId == OrderID).FirstOrDefault();

                    if (subscription != null)
                    {
                        subscription.ispaid = true;
                        subscription.payment_type = payment_type;
                        subscription.PaymentID = PaymentID;
                        subscription.Result = Result;
                        subscription.PostDate = DateTime.Now;
                        subscription.TranID = TranID;
                        subscription.Ref = Ref;
                        subscription.TrackID = TrackID;
                        subscription.Auth = Auth;
                        var UpdatedOrder = _context.Subscriptions.Attach(subscription);
                        UpdatedOrder.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        _context.SaveChanges();
                        var userSubscriptions = await _userManager.FindByIdAsync(subscription.UserId);
                        var Course =  _context.Courses.Include(e=>e.Trainer).Where(c => c.CourseId == subscription.EntityId).FirstOrDefault();



                        var webRoot = _env.WebRootPath;

                        var pathToFile = _env.WebRootPath
                               + Path.DirectorySeparatorChar.ToString()
                               + "Templates"
                               + Path.DirectorySeparatorChar.ToString()
                               + "EmailTemplate"
                               + Path.DirectorySeparatorChar.ToString()
                               + "CourseSubscribe.html";
                        var builder = new BodyBuilder();
                        using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                        {

                            builder.HtmlBody = SourceReader.ReadToEnd();

                        }
                        string messageBody = string.Format(builder.HtmlBody,
                            userSubscriptions.FullName,

                           Course.PublishDate.Value.ToShortDateString(),
                           Course.CourseTlEn,
                           Course.Trainer.FullNameEn,
                        subscription.Cost,
                           subscription.SubDate.Value.ToShortDateString()

                           );
                        await _emailSender.SendEmailAsync(userSubscriptions.Email, "Course Subscription", messageBody);
                    }
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
