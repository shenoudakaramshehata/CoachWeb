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
    public class CampPayModel : PageModel
    {
        private CoachContext _context;
        public Camp Camp { get; set; }
        public Course Course  { get; set; }
        public Tournament Tournament { get; set; }

        public PaymentMethod paymentMethod { get; set; }
        public HttpClient httpClient { get; set; }

        public CampPayModel(HttpClient HttpClient,
            CoachContext context)
        {
            _context = context;
            httpClient = HttpClient;

        }

        public async Task<IActionResult> OnGetAsync(string Values)
        {
            
            if (Values != null)
            {
                var model = JsonConvert.DeserializeObject<Camp>(Values);

                if (model.CampTypeId == 0)
                {
                    return Page();
                }
                if (model.CampTargetId == 0)
                {
                    return Page();
                }
                if (model.CountryId == 0)
                {
                    return Page();
                }
                if (model.CampPlanId !=0)
                {
                    var plan = _context.CampPlans.Find(model.CampPlanId);
                    model.JoinStart = DateTime.Now;
                    model.JoinEnd = DateTime.Now.AddMonths(plan.DurationInMonth.Value);
                    model.SubPrice = plan.Price;
                    model.Cost = plan.Price;
                }

                model.ispaid = false;
                
                var Trainer = _context.Trainers.Where(e=>e.Email == model.UserId).FirstOrDefault();

                if(Trainer != null)
                {
                    try
                    {
                        _context.Camps.Add(model);
                        _context.SaveChanges();

                    }
                    catch (Exception)
                    {
                        return RedirectToPage("SomethingwentError");
                    }
                    if (model.PaymentMethodId == 2)
                    {
                        
                        var requesturl = "https://api.upayments.com/test-payment";
                        var fields = new
                        {
                            merchant_id = "1201",
                            username = "test",
                            password = "test",
                            order_id = model.CampId,
                            total_price = model.Cost,
                            test_mode = 0,
                            CstFName = Trainer.FullNameEn,
                            CstEmail = Trainer.Email,
                            CstMobile = Trainer.Mobile,
                            api_key = "jtest123",
                            success_url = "http://codewarenet-001-site8.dtempurl.com/success",
                            error_url = "http://codewarenet-001-site8.dtempurl.com/failed"
                            //success_url = "https://localhost:44354/success",
                            //error_url = "https://localhost:44354/failed"

                        };
                        var content = new StringContent(JsonConvert.SerializeObject(fields), Encoding.UTF8, "application/json");
                        var task = httpClient.PostAsync(requesturl, content);
                        var result = await task.Result.Content.ReadAsStringAsync();
                        var paymenturl = JsonConvert.DeserializeObject<paymenturl>(result);
                        if (paymenturl.status == "success")

                        {
                            return Redirect(paymenturl.paymentURL);
                        }
                        else
                        {
                            return RedirectToPage("SomethingwentError", new { Message = paymenturl.error_msg });
                        }
                    }
                }

            }

            return RedirectToPage("SomethingwentError");
        }
        //private async Task< IActionResult> SendReq<T>(T model)
        //{
        //    model<T>.ispaid = false;
        //    try
        //    {
        //        _context.Camps.Add(model);
        //        _context.SaveChanges();

        //    }
        //    catch (Exception)
        //    {
        //        return RedirectToPage("SomethingwentError");
        //    }
        //    var Trainer = _context.Trainers.Where(e => e.Email == model.UserId).FirstOrDefault();

        //    if (model.PaymentMethodId == 2)
        //    {
        //        var requesturl = "https://api.upayments.com/test-payment";
        //        var fields = new
        //        {
        //            merchant_id = "1201",
        //            username = "test",
        //            password = "test",
        //            order_id = model.CampId,
        //            total_price = model.Cost,
        //            test_mode = 0,
        //            CstFName = Trainer.FullNameEn,
        //            CstEmail = Trainer.Email,
        //            CstMobile = Trainer.Mobile,
        //            api_key = "jtest123",
        //            //success_url = "http://codewarenet-001-site8.dtempurl.com/success",
        //            //error_url = "http://codewarenet-001-site8.dtempurl.com/failed"
        //            success_url = "https://localhost:44354/success",
        //            error_url = "https://localhost:44354/failed"

        //        };
        //        var content = new StringContent(JsonConvert.SerializeObject(fields), Encoding.UTF8, "application/json");
        //        var task = httpClient.PostAsync(requesturl, content);
        //        var result = await task.Result.Content.ReadAsStringAsync();
        //        var paymenturl = JsonConvert.DeserializeObject<paymenturl>(result);
        //        if (paymenturl.status == "success")

        //        {
        //            return Redirect(paymenturl.paymentURL);
        //        }
        //        else
        //        {
        //            return RedirectToPage("SomethingwentError", new { Message = paymenturl.error_msg });
        //        }
        //    }
        //    return Page();
        //}
    }
    
}
