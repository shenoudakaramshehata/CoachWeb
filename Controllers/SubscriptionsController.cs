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
    public class SubscriptionsController : Controller
    {
        private CoachContext _context;
        private readonly ApplicationDbContext _db;


        public SubscriptionsController(ApplicationDbContext db, CoachContext context) {
            _context = context;
            _db = db;

        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var subscriptions = _context.Subscriptions.Select(i => new {
                i.SubscriptionId,
                i.SubDate,
                i.UserId,
                i.EntityTypeId,
                i.EntityId,
                i.Cost,
                i.PaymentMethodId
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "SubscriptionId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(subscriptions, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Subscription();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Subscriptions.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.SubscriptionId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Subscriptions.FirstOrDefaultAsync(item => item.SubscriptionId == key);
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
            var model = await _context.Subscriptions.FirstOrDefaultAsync(item => item.SubscriptionId == key);

            _context.Subscriptions.Remove(model);
            await _context.SaveChangesAsync();
        }



        [HttpGet]
        public async Task<IActionResult> UsersLookup(DataSourceLoadOptions loadOptions)
        {

            var lookup = from i in _db.Users
                         orderby i.Email
                         select new
                         {
                             Value = i.Id,
                             Text = i.Email
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));

        }


        [HttpGet]
        public async Task<IActionResult> EntityTypeLookup(DataSourceLoadOptions loadOptions)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.EntityTypes
                               orderby i.EntityTypeTlen
                               select new
                               {
                                   Value = i.EntityTypeId,
                                   Text = i.EntityTypeTlen
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.EntityTypes
                           orderby i.EntityTypeTlar
                           select new
                           {
                               Value = i.EntityTypeId,
                               Text = i.EntityTypeTlar
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));


        }

        [HttpGet]
        public async Task<IActionResult> PaymentMethodsLookup(DataSourceLoadOptions loadOptions) {
            
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.PaymentMethods
                               orderby i.PaymentMethodTlEn
                               select new
                               {
                                   Value = i.PaymentMethodId,
                                   Text = i.PaymentMethodTlEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.PaymentMethods
                           orderby i.PaymentMethodTlar
                           select new
                           {
                               Value = i.PaymentMethodId,
                               Text = i.PaymentMethodTlar
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));
        }

        private void PopulateModel(Subscription model, IDictionary values) {
            string SUBSCRIPTION_ID = nameof(Subscription.SubscriptionId);
            string SUB_DATE = nameof(Subscription.SubDate);
            string USER_ID = nameof(Subscription.UserId);
            string ENTITY_TYPE_ID = nameof(Subscription.EntityTypeId);
            string ENTITY_ID = nameof(Subscription.EntityId);
            string COST = nameof(Subscription.Cost);
            string PAYMENT_METHOD_ID = nameof(Subscription.PaymentMethodId);

            if(values.Contains(SUBSCRIPTION_ID)) {
                model.SubscriptionId = Convert.ToInt32(values[SUBSCRIPTION_ID]);
            }

            if(values.Contains(SUB_DATE)) {
                model.SubDate = values[SUB_DATE] != null ? Convert.ToDateTime(values[SUB_DATE]) : (DateTime?)null;
            }

            if(values.Contains(USER_ID)) {
                model.UserId = Convert.ToString(values[USER_ID]);
            }

            if(values.Contains(ENTITY_TYPE_ID)) {
                model.EntityTypeId = Convert.ToInt32(values[ENTITY_TYPE_ID]);
            }

            if(values.Contains(ENTITY_ID)) {
                model.EntityId = values[ENTITY_ID] != null ? Convert.ToInt32(values[ENTITY_ID]) : (int?)null;
            }

            if(values.Contains(COST)) {
                model.Cost = values[COST] != null ? Convert.ToDouble(values[COST], CultureInfo.InvariantCulture) : (double?)null;
            }

            if(values.Contains(PAYMENT_METHOD_ID)) {
                model.PaymentMethodId = values[PAYMENT_METHOD_ID] != null ? Convert.ToInt32(values[PAYMENT_METHOD_ID]) : (int?)null;
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