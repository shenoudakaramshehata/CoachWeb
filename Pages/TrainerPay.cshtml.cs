using Coach.Data;
using Coach.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
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
    public class TrainerPayModel : PageModel
    {
        private CoachContext _context;
        
        private Trainer trainer { get; set; }

        public PaymentMethod paymentMethod { get; set; }
        public HttpClient httpClient { get; set; }

        public TrainerPayModel(HttpClient HttpClient,
            CoachContext context)
        {
            _context = context;
            httpClient = HttpClient;

        }

        //public async Task<IActionResult> OnGetAsync(string Values)
        //{

        //    if (Values != null)
        //    {
        //        var model = JsonConvert.DeserializeObject<Trainer>(Values);

               
                
        //        if (model.CountryId == 0)
        //        {
        //            return Page();
        //        }
        //        //if (model.TrainerPlanId != 0)
        //        //{
        //        //    var plan = _context.TrainerPlans.Find(model.TrainerPlanId);
        //        //    model.SubscriptionCost = plan.Price.Value;
        //        //}

        //        //model.ispaid = false;
        //        model.AddedDate = DateTime.Now;

        //        //var Trainer = _context.Trainers.Where(e => e.Email == model.UserId).FirstOrDefault();

                
        //            try
        //            {
        //                _context.Trainers.Add(model);
        //                _context.SaveChanges();

        //            }
        //            catch (Exception)
        //            {
        //                return RedirectToPage("SomethingwentError");
        //            }
        //            if (model.PaymentMethodId == 2)
        //            {
        //                var trainer=_context.Trainers.Where(e=>e.Email==model.Email).FirstOrDefault();
        //                var requesturl = "https://api.upayments.com/test-payment";
        //                var fields = new
        //                {
        //                    merchant_id = "1201",
        //                    username = "test",
        //                    password = "test",
        //                    order_id = model.TrainerId,
        //                    total_price = model.SubscriptionCost,
        //                    test_mode = 0,
        //                    CstFName = model.FullNameEn,
        //                    CstEmail = model.Email,
        //                    CstMobile = model.Mobile,
        //                    api_key = "jtest123",
        //                    success_url = "http://codewarenet-001-site8.dtempurl.com/TrainerPaySuccess",
        //                    error_url = "http://codewarenet-001-site8.dtempurl.com/TrainerPayFaield",
        //                    //success_url = "https://localhost:44354/TrainerPaySuccess",
        //                    //error_url = "https://localhost:44354/TrainerPayFaield"

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

            

        //    return RedirectToPage("SomethingwentError");
        //}
    }
}
