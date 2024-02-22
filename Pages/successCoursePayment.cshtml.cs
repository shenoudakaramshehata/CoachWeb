using Coach.Data;
using Coach.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;


namespace Coach.Pages
{
    public class successCoursePaymentModel : PageModel
    {
        private CoachContext _context;
        public Course Course { get; set; }

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        public successCoursePaymentModel(CoachContext context, IEmailSender emailSender, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync(string payment_type, string PaymentID, string Result, int OrderID, DateTime? PostDate, string TranID,
        string Ref, string TrackID, string Auth)
        {
            //if (OrderID != 0)
            //{
            //    Course = _context.Courses.Find(OrderID);

            //        Course.ispaid = true;
            //        Course.payment_type = payment_type;
            //        Course.PaymentID = PaymentID;
            //        Course.Result = Result;
            //        Course.CourseId = OrderID;
            //        Course.PostDate = PostDate;
            //        Course.TranID = TranID;
            //        Course.Ref = Ref;
            //        Course.TrackID = TrackID;
            //        Course.Auth = Auth;
            //        var UpdatedOrder = _context.Courses.Attach(Course);
            //        UpdatedOrder.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            //        _context.SaveChanges();

            //    var Trainer = _context.Trainers.Where(e => e.TrainerId == Course.TrainerId).FirstOrDefault();
            //    var traineruser = await _userManager.FindByIdAsync(Trainer.UserId);


            //    await _emailSender.SendEmailAsync(
            //           traineruser.Email,
            //           "Payment success",
            //           $"thanks for your paying.");

                



                
            //}
            return Page();

        }
    }
}
