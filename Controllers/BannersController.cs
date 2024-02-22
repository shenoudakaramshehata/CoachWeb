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
    public class BannersController : Controller
    {
        private CoachContext _context;

        public BannersController(CoachContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var banner = _context.Banners.Select(i => new {
                i.BannerId,
                i.BannerPic,
                i.EntityTypeId,
                i.EntityId,
                i.BannerIsActive,
                i.BannerOrderIndex
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "BannerId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(banner, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Banner();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Banners.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.BannerId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Banners.FirstOrDefaultAsync(item => item.BannerId == key);
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
            var model = await _context.Banners.FirstOrDefaultAsync(item => item.BannerId == key);

            _context.Banners.Remove(model);
            await _context.SaveChangesAsync();
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
        public async Task<IActionResult> CampLookup(DataSourceLoadOptions loadOptions)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.Camps
                               orderby i.CampTlEn
                               select new
                               {
                                   Value = i.CampId,
                                   Text = i.CampTlEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.Camps
                           orderby i.CampTlAr
                           select new
                           {
                               Value = i.CampId,
                               Text = i.CampTlAr
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));


        }

          [HttpGet]
        public async Task<IActionResult> CourseLookup(DataSourceLoadOptions loadOptions)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.Courses
                               orderby i.CourseTlEn
                               select new
                               {
                                   Value = i.CourseId,
                                   Text = i.CourseTlEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.Courses
                           orderby i.CourseTlAr
                           select new
                           {
                               Value = i.CourseId,
                               Text = i.CourseTlAr
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));


        }
         [HttpGet]
        public async Task<IActionResult> TournamentLookup(DataSourceLoadOptions loadOptions)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.Tournaments
                               orderby i.TournamentTlEn
                               select new
                               {
                                   Value = i.TournamentId,
                                   Text = i.TournamentTlEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.Tournaments
                           orderby i.TournamentTlAr
                           select new
                           {
                               Value = i.TournamentId,
                               Text = i.TournamentTlAr
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));


        }
        [HttpGet]
        public async Task<IActionResult> TrainerLookup(DataSourceLoadOptions loadOptions)
        {
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

        private void PopulateModel(Banner model, IDictionary values) {
            string BANNER_ID = nameof(Banner.BannerId);
            string BANNER_PIC = nameof(Banner.BannerPic);
            string ENTITY_TYPE_ID = nameof(Banner.EntityTypeId);
            string ENTITY_ID = nameof(Banner.EntityId);
            string BANNER_IS_ACTIVE = nameof(Banner.BannerIsActive);
            string BANNER_ORDER_INDEX = nameof(Banner.BannerOrderIndex);

            if(values.Contains(BANNER_ID)) {
                model.BannerId = Convert.ToInt32(values[BANNER_ID]);
            }

            if(values.Contains(BANNER_PIC)) {
                model.BannerPic = Convert.ToString(values[BANNER_PIC]);
            }

            if(values.Contains(ENTITY_TYPE_ID)) {
                model.EntityTypeId = values[ENTITY_TYPE_ID] != null ? Convert.ToInt32(values[ENTITY_TYPE_ID]) : (int?)null;
            }

            if(values.Contains(ENTITY_ID)) {
                model.EntityId = Convert.ToString(values[ENTITY_ID]);
            }

            if(values.Contains(BANNER_IS_ACTIVE)) {
                model.BannerIsActive = values[BANNER_IS_ACTIVE] != null ? Convert.ToBoolean(values[BANNER_IS_ACTIVE]) : (bool?)null;
            }

            if(values.Contains(BANNER_ORDER_INDEX)) {
                model.BannerOrderIndex = values[BANNER_ORDER_INDEX] != null ? Convert.ToInt32(values[BANNER_ORDER_INDEX]) : (int?)null;
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