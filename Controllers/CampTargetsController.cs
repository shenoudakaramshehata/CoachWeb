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
using NToastNotify;
using Microsoft.AspNetCore.Localization;

namespace Coach.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CampTargetsController : Controller
    {
        private CoachContext _context;
        private readonly IToastNotification _toastNotification;

        public CampTargetsController(CoachContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var camptarget = _context.CampTargets.Select(i => new {
                i.CampTargetId,
                i.CampTargetTlAr,
                i.CampTargetTlEn,
                i.IsActive
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "CampTargetId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(camptarget, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new CampTarget();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.CampTargets.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.CampTargetId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.CampTargets.FirstOrDefaultAsync(item => item.CampTargetId == key);
            if(model == null)
                return StatusCode(409, "Object not found");

            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> CountryLookup(DataSourceLoadOptions loadOptions)
        {
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

        [HttpDelete]
        public async Task<IActionResult> Delete(int key) {
            var camp = _context.Camps.Any(c => c.CampTargetId == key);
            if (camp==false)
            {
                var model = await _context.CampTargets.FirstOrDefaultAsync(item => item.CampTargetId == key);
                _context.CampTargets.Remove(model);
                await _context.SaveChangesAsync();

                return Ok();
            }
            else
            {

                return StatusCode(409, "You cannot delete this Camp Target");

            }

        }


        private void PopulateModel(CampTarget model, IDictionary values) {
            string CAMP_TARGET_ID = nameof(CampTarget.CampTargetId);
            string CAMP_TARGET_TL_AR = nameof(CampTarget.CampTargetTlAr);
            string CAMP_TARGET_TL_EN = nameof(CampTarget.CampTargetTlEn);
            string IS_ACTIVE = nameof(CampTarget.IsActive);

            if(values.Contains(CAMP_TARGET_ID)) {
                model.CampTargetId = Convert.ToInt32(values[CAMP_TARGET_ID]);
            }

            if(values.Contains(CAMP_TARGET_TL_AR)) {
                model.CampTargetTlAr = Convert.ToString(values[CAMP_TARGET_TL_AR]);
            }

            if(values.Contains(CAMP_TARGET_TL_EN)) {
                model.CampTargetTlEn = Convert.ToString(values[CAMP_TARGET_TL_EN]);
            }

            if(values.Contains(IS_ACTIVE)) {
                model.IsActive = values[IS_ACTIVE] != null ? Convert.ToBoolean(values[IS_ACTIVE]) : (bool?)null;
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