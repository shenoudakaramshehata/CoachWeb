using Coach.Data;
using Coach.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Pages
{
    public class CoursePayModel : PageModel
    {
        private CoachContext _context;
        public Course Course { get; set; }

        public PaymentMethod paymentMethod { get; set; }
        public HttpClient httpClient { get; set; }

        public CoursePayModel(HttpClient HttpClient,
            CoachContext context)
        {
            _context = context;
            httpClient = HttpClient;

        }
        //public async Task<IActionResult> OnGetAsync(string Values)
        //{
        //    if (Values != null)
        //    {
        //        var model = JsonConvert.DeserializeObject<Course>(Values);
        //        if (model.CourseTargetId == 0)
        //        {
        //            return Page();
        //        }
        //        //model.ispaid = false;
               

        //        var Trainer = _context.Trainers.Where(e => e.TrainerId == model.TrainerId).FirstOrDefault();
        //        if(Trainer !=null)
        //        {
        //            try
        //            {
        //                _context.Courses.Add(model);
        //                _context.SaveChanges();

        //            }
        //            catch (Exception)
        //            {
        //                return RedirectToPage("SomethingwentError");
        //            }
        //            if (model.PaymentMethodId == 2)
        //            {
        //                var requesturl = "https://api.upayments.com/test-payment";
        //                var fields = new
        //                {
        //                    merchant_id = "1201",
        //                    username = "test",
        //                    password = "test",
        //                    order_id = model.CourseId,
        //                    total_price = model.Cost,
        //                    test_mode = 0,
        //                    CstFName = Trainer.FullNameEn,
        //                    CstEmail = Trainer.Email,
        //                    CstMobile = Trainer.Mobile,
        //                    api_key = "jtest123",
        //                    //success_url = "https://localhost:44354/successCoursePayment",
        //                   // error_url = "https://localhost:44354/faildCoursePayment"
        //                    success_url = "http://codewarenet-001-site11.dtempurl.com/successCoursePayment",
        //                    error_url = "http://codewarenet-001-site11.dtempurl.com/faildCoursePayment",
        //                };

        //                var content = new StringContent(JsonConvert.SerializeObject(fields), Encoding.UTF8, "application/json");
        //                var task = httpClient.PostAsync(requesturl, content);
        //                var result = await task.Result.Content.ReadAsStringAsync();
        //                var paymenturl = JsonConvert.DeserializeObject<paymenturl>(result);
        //                if (paymenturl.status == "success")

        //                {
        //                    return Redirect(paymenturl.paymentURL);
        //                }
        //                else
        //                {
        //                    return RedirectToPage("SomethingwentError", new { Message = paymenturl.error_msg });
        //                }
        //            }

        //        }
        //    }

        //    return RedirectToPage("SomethingwentError");
        //}
    }
}
