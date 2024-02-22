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
    public class PublicNotificationsController : Controller
    {
        private CoachContext _context;

        public PublicNotificationsController(CoachContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var publicnotification = _context.PublicNotifications.Select(i => new {
                i.PublicNotificationId,
                i.Title,
                i.Body,
                i.Date,
                i.EntityTypeId,
                i.EntityId,
                i.CountryId
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "PublicNotificationId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(publicnotification, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values)
        {
            var model = new PublicNotification();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.PublicNotifications.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.PublicNotificationId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values)
        {
            var model = await _context.PublicNotifications.FirstOrDefaultAsync(item => item.PublicNotificationId == key);
            if (model == null)
                return StatusCode(409, "Object not found");

            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task Delete(int key)
        {
            var model = await _context.PublicNotifications.FirstOrDefaultAsync(item => item.PublicNotificationId == key);

            _context.PublicNotifications.Remove(model);
            await _context.SaveChangesAsync();
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
                               where i.CountryIsActive==true
                               select new
                               {
                                   Value = i.CountryId,
                                   Text = i.CountryTlEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.Countries
                           orderby i.CountryTlAr
                           where i.CountryIsActive == true
                           select new
                           {
                               Value = i.CountryId,
                               Text = i.CountryTlAr
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));

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
                               where i.EntityTypeId != 1 && i.EntityTypeId != 5
                               select new
                               {
                                   Value = i.EntityTypeId,
                                   Text = i.EntityTypeTlen
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.EntityTypes
                           orderby i.EntityTypeTlar
                           where i.EntityTypeId != 1 && i.EntityTypeId != 5
                           select new
                           {
                               Value = i.EntityTypeId,
                               Text = i.EntityTypeTlar
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));


        }
        [HttpGet]
        public async Task<IActionResult> CampLookup(DataSourceLoadOptions loadOptions,int? countryId)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();
            if (countryId != null)
            {
                if (BrowserCulture == "en-US")
                {
                    var lookupEn = from i in _context.Camps
                                   orderby i.CampTlEn
                                   where i.CountryId==countryId
                                   select new
                                   {
                                       Value = i.CampId,
                                       Text = i.CampTlEn
                                   };
                    return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
                }
                var lookupAr = from i in _context.Camps
                               orderby i.CampTlAr
                               where i.CountryId == countryId
                               select new
                               {
                                   Value = i.CampId,
                                   Text = i.CampTlAr
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));

            }
            else
            {
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


        }
        [HttpGet]
        public async Task<IActionResult> CourseLookup(DataSourceLoadOptions loadOptions,int? countryId)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();
            if (countryId != null)
            {
                if (BrowserCulture == "en-US")
                {
                    var lookupEn = from i in _context.Courses
                                   orderby i.CourseTlEn
                                   where i.Trainer.CountryId==countryId
                                   select new
                                   {
                                       Value = i.CourseId,
                                       Text = i.CourseTlEn
                                   };
                    return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
                }
                var lookupAr = from i in _context.Courses
                               orderby i.CourseTlAr
                               where i.Trainer.CountryId == countryId
                               select new
                               {
                                   Value = i.CourseId,
                                   Text = i.CourseTlAr
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));
            }
            else
            {
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


        }
        [HttpGet]
        public async Task<IActionResult> TournamentLookup(DataSourceLoadOptions loadOptions,int? countryId)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();
            if (countryId != null)
            {
                if (BrowserCulture == "en-US")
                {
                    var lookupEn = from i in _context.Tournaments
                                   orderby i.TournamentTlEn
                                   where i.CountryId==countryId
                                   select new
                                   {
                                       Value = i.TournamentId,
                                       Text = i.TournamentTlEn
                                   };
                    return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
                }
                var lookupAr = from i in _context.Tournaments
                               orderby i.TournamentTlAr
                               where i.CountryId==countryId
                               select new
                               {
                                   Value = i.TournamentId,
                                   Text = i.TournamentTlAr
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));
            }
            else
            {
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
            


        }
        private void PopulateModel(PublicNotification model, IDictionary values)
        {
            string PUBLIC_NOTIFICATION_ID = nameof(PublicNotification.PublicNotificationId);
            string TITLE = nameof(PublicNotification.Title);
            string BODY = nameof(PublicNotification.Body);
            string DATE = nameof(PublicNotification.Date);
            string ENTITY_TYPE_ID = nameof(PublicNotification.EntityTypeId);
            string ENTITY_ID = nameof(PublicNotification.EntityId);
            string COUNTRY_ID = nameof(PublicNotification.CountryId);

            if (values.Contains(PUBLIC_NOTIFICATION_ID))
            {
                model.PublicNotificationId = Convert.ToInt32(values[PUBLIC_NOTIFICATION_ID]);
            }

            if (values.Contains(TITLE))
            {
                model.Title = Convert.ToString(values[TITLE]);
            }

            if (values.Contains(BODY))
            {
                model.Body = Convert.ToString(values[BODY]);
            }

            if (values.Contains(DATE))
            {
                model.Date = Convert.ToDateTime(values[DATE]);
            }

            if (values.Contains(ENTITY_TYPE_ID))
            {
                model.EntityTypeId = values[ENTITY_TYPE_ID] != null ? Convert.ToInt32(values[ENTITY_TYPE_ID]) : (int?)null;
            }

            if (values.Contains(ENTITY_ID))
            {
                model.EntityId = values[ENTITY_ID] != null ? Convert.ToInt32(values[ENTITY_ID]) : (int?)null;
            }

            if (values.Contains(COUNTRY_ID))
            {
                model.CountryId = Convert.ToInt32(values[COUNTRY_ID]);
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState)
        {
            var messages = new List<string>();

            foreach (var entry in modelState)
            {
                foreach (var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }
    }
}