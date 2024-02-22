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
    public class CampPlansController : Controller
    {
        private CoachContext _context;

        public CampPlansController(CoachContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var campplans = _context.CampPlans.Select(i => new {
                i.CampPlanId,
                i.PlanTlAr,
                i.PlanTlEn,
                i.IsActive,
                i.Price,
                i.DurationInMonth,
                i.CountryId
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "CampPlanId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(campplans, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new CampPlan();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.CampPlans.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.CampPlanId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.CampPlans.FirstOrDefaultAsync(item => item.CampPlanId == key);
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
            var model = await _context.CampPlans.FirstOrDefaultAsync(item => item.CampPlanId == key);

            _context.CampPlans.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> CountriesLookup(DataSourceLoadOptions loadOptions) {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.Countries
                               orderby i.CountryTlEn
                               select new
                               {
                                   Value = i.CountryId,
                                   Text = i.CountryTlEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.Countries
                           orderby i.CountryTlAr
                           select new
                           {
                               Value = i.CountryId,
                               Text = i.CountryTlAr
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));
        }

        private void PopulateModel(CampPlan model, IDictionary values) {
            string CAMP_PLAN_ID = nameof(CampPlan.CampPlanId);
            string PLAN_TL_AR = nameof(CampPlan.PlanTlAr);
            string PLAN_TL_EN = nameof(CampPlan.PlanTlEn);
            string IS_ACTIVE = nameof(CampPlan.IsActive);
            string PRICE = nameof(CampPlan.Price);
            string DURATION_IN_MONTH = nameof(CampPlan.DurationInMonth);
            string COUNTRY_ID = nameof(CampPlan.CountryId);

            if(values.Contains(CAMP_PLAN_ID)) {
                model.CampPlanId = Convert.ToInt32(values[CAMP_PLAN_ID]);
            }

            if(values.Contains(PLAN_TL_AR)) {
                model.PlanTlAr = Convert.ToString(values[PLAN_TL_AR]);
            }

            if(values.Contains(PLAN_TL_EN)) {
                model.PlanTlEn = Convert.ToString(values[PLAN_TL_EN]);
            }

            if(values.Contains(IS_ACTIVE)) {
                model.IsActive = values[IS_ACTIVE] != null ? Convert.ToBoolean(values[IS_ACTIVE]) : (bool?)null;
            }

            if(values.Contains(PRICE)) {
                model.Price = values[PRICE] != null ? Convert.ToDouble(values[PRICE], CultureInfo.InvariantCulture) : (double?)null;
            }

            if(values.Contains(DURATION_IN_MONTH)) {
                model.DurationInMonth = values[DURATION_IN_MONTH] != null ? Convert.ToInt32(values[DURATION_IN_MONTH]) : (int?)null;
            }

            if(values.Contains(COUNTRY_ID)) {
                model.CountryId = values[COUNTRY_ID] != null ? Convert.ToInt32(values[COUNTRY_ID]) : (int?)null;
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