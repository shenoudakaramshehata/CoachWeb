using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Coach.Data;
using Coach.Models;
using Microsoft.AspNetCore.Localization;

namespace Coach.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TrainerSubscriptionsController : Controller
    {
        private CoachContext _context;

        public TrainerSubscriptionsController(CoachContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int trainerId,DataSourceLoadOptions loadOptions) {
            var trainersubscriptions = _context.TrainerSubscriptions.Where(c=>c.TrainerId==trainerId).Select(i => new {
                i.TrainerSubscriptionId,
                i.TrainerId,
                i.TrainerPlanId,
                i.StartDate,
                i.EndDate,
                i.Price,
                i.Remarks
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "TrainerSubscriptionId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(trainersubscriptions, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new TrainerSubscription();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.TrainerSubscriptions.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.TrainerSubscriptionId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.TrainerSubscriptions.FirstOrDefaultAsync(item => item.TrainerSubscriptionId == key);
            if(model == null)
                return StatusCode(409, "Object not found");

            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task Delete(int key) {
            var model = await _context.TrainerSubscriptions.FirstOrDefaultAsync(item => item.TrainerSubscriptionId == key);

            _context.TrainerSubscriptions.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> TrainersLookup(DataSourceLoadOptions loadOptions) {
            
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.Trainers
                               orderby i.FullNameEn
                               select new
                               {
                                   Value = i.TrainerId,
                                   Text = i.FullNameEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.Trainers
                           orderby i.FullNameAr
                           select new
                           {
                               Value = i.TrainerId,
                               Text = i.FullNameAr
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> TrainerPlansLookup(DataSourceLoadOptions loadOptions) {
           
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.TrainerPlans
                               orderby i.PlanTlEn
                               select new
                               {
                                   Value = i.TrainerPlanId,
                                   Text = i.PlanTlEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.TrainerPlans
                           orderby i.PlanTlAr
                           select new
                           {
                               Value = i.TrainerPlanId,
                               Text = i.PlanTlAr
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));

        }

        private void PopulateModel(TrainerSubscription model, IDictionary values) {
            string TRAINER_SUBSCRIPTION_ID = nameof(TrainerSubscription.TrainerSubscriptionId);
            string TRAINER_ID = nameof(TrainerSubscription.TrainerId);
            string TRAINER_PLAN_ID = nameof(TrainerSubscription.TrainerPlanId);
            string START_DATE = nameof(TrainerSubscription.StartDate);
            string END_DATE = nameof(TrainerSubscription.EndDate);
            string PRICE = nameof(TrainerSubscription.Price);
            string REMARKS = nameof(TrainerSubscription.Remarks);

            if(values.Contains(TRAINER_SUBSCRIPTION_ID)) {
                model.TrainerSubscriptionId = Convert.ToInt32(values[TRAINER_SUBSCRIPTION_ID]);
            }

            if(values.Contains(TRAINER_ID)) {
                model.TrainerId = Convert.ToInt32(values[TRAINER_ID]);
            }

            if(values.Contains(TRAINER_PLAN_ID)) {
                model.TrainerPlanId = Convert.ToInt32(values[TRAINER_PLAN_ID]);
            }

            if(values.Contains(START_DATE)) {
                model.StartDate = values[START_DATE] != null ? Convert.ToDateTime(values[START_DATE]) : (DateTime?)null;
            }

            if(values.Contains(END_DATE)) {
                model.EndDate = values[END_DATE] != null ? Convert.ToDateTime(values[END_DATE]) : (DateTime?)null;
            }

            if(values.Contains(PRICE)) {
                model.Price = values[PRICE] != null ? Convert.ToDouble(values[PRICE], CultureInfo.InvariantCulture) : (double?)null;
            }

            if(values.Contains(REMARKS)) {
                model.Remarks = Convert.ToString(values[REMARKS]);
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState) {
            var messages = new List<string>();

            foreach(var entry in modelState) {
                foreach(var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }
    }
}