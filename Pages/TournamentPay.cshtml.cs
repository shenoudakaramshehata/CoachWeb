using Coach.Data;
using Coach.Models;
using Microsoft.AspNetCore.Identity;
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
    public class TournamentPayModel : PageModel
    {
        public UserManager<ApplicationUser> _userManager { get; set; }
        private CoachContext _context;
        public Tournament Tournament { get; set; }

        public PaymentMethod paymentMethod { get; set; }
        public HttpClient httpClient { get; set; }

        public TournamentPayModel(HttpClient HttpClient, UserManager<ApplicationUser> userManager,
            CoachContext context)
        {
            _userManager = userManager;
            _context = context;
            httpClient = HttpClient;

        }
        public async Task<IActionResult> OnGetAsync(string Values)
        {
            if (Values != null)
            {
                var model = JsonConvert.DeserializeObject<Tournament>(Values);
                if (model.TournamentPlanId == 0)
                {
                    return Page();
                }
                    var plan = _context.TournamentPlans.Find(model.TournamentPlanId);
                    model.SubStartDate = DateTime.Now;
                    model.SubEndDate = DateTime.Now.AddMonths(plan.DurationInMonth.Value);
                    model.SubPrice = plan.Price;
                    model.Cost = plan.Price;
                if (model.TournamentTypeId == 0)
                {
                    return Page();
                }
                if (model.TournamentTargetId == 0)
                {
                    return Page();
                }
                if (model.CountryId == 0)
                {
                    return Page();
                }
                
                model.ispaid = false;
                
                var traineruser = await _userManager.FindByIdAsync(model.UserId);
                var Trainer = _context.Trainers.Where(e => e.Email == traineruser.Email).FirstOrDefault();
                if(Trainer != null)
                {
                    try
                    {
                        _context.Tournaments.Add(model);
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
                            order_id = model.TournamentId,
                            total_price = model.Cost,
                            test_mode = 0,
                            CstFName = Trainer.FullNameEn,
                            CstEmail = Trainer.Email,
                            CstMobile = Trainer.Mobile,
                            api_key = "jtest123",
                            //success_url = "https://localhost:44354/successTournmentPayment",
                            //error_url = "https://localhost:44354/feildTournmentPayment"

                            success_url = "http://codewarenet-001-site11.dtempurl.com/successTournmentPayment",
                            error_url = "http://codewarenet-001-site11.dtempurl.com/feildTournmentPayment"

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
    }
}


