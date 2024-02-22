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
    public class successModel : PageModel
    {
        private CoachContext _context;
        public Camp Camp { get; set; }

        private readonly IEmailSender _emailSender;
        private IHostingEnvironment _env;
        private readonly UserManager<ApplicationUser> _userManager;
        public successModel(CoachContext context,IEmailSender emailSender, IHostingEnvironment env, UserManager<ApplicationUser> userManager)
        {
            _emailSender=emailSender;
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
                    Camp = _context.Camps.Find(OrderID);

                    if (Camp != null)
                    {
                        Camp.ispaid = true;
                        Camp.payment_type = payment_type;
                        Camp.PaymentID = PaymentID;
                        Camp.Result = Result;
                        Camp.CampId = OrderID;
                        Camp.PostDate = DateTime.Now;
                        Camp.TranID = TranID;
                        Camp.Ref = Ref;
                        Camp.TrackID = TrackID;
                        Camp.Auth = Auth;
                        var UpdatedOrder = _context.Camps.Attach(Camp);
                        UpdatedOrder.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        _context.SaveChanges();
                        var user = await _userManager.FindByIdAsync(Camp.UserId);
                        var CampPlan = _context.CampPlans.Where(e => e.CampPlanId == Camp.CampPlanId).FirstOrDefault();

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
                           Camp.PlanStartDate.Value.ToShortDateString(),
                           Camp.PlanStartDate.Value.ToShortDateString(),
                           Camp.SubPrice,
                           user.FullName,
                           CampPlan.PlanTlAr

                           );
                        await _emailSender.SendEmailAsync(user.Email, "Camp Plan Subscription", messageBody);


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
