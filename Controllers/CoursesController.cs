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
    public class CoursesController : Controller
    {
        private CoachContext _context;

        public CoursesController(CoachContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var course = _context.Courses.Select(i => new {
                i.CourseId,
                i.CourseTlAr,
                i.CourseTlEn,
                i.TrainerId,
                i.CourseTargetId,
                i.CourseDescAr,
                i.CourseDescEn,
                i.IsActive,
                i.PublishDate,
                i.Pic
            });

            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "CourseId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(course, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Course();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Courses.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.CourseId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Courses.FirstOrDefaultAsync(item => item.CourseId == key);
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
            var model = await _context.Courses.FirstOrDefaultAsync(item => item.CourseId == key);

            _context.Courses.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> CourseTargetLookup(DataSourceLoadOptions loadOptions) {

            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.CourseTargets
                               orderby i.CourseTargetTlEn
                               select new
                               {
                                   Value = i.CourseTargetId,
                                   Text = i.CourseTargetTlEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.CourseTargets
                           orderby i.CourseTargetTlAr
                           select new
                           {
                               Value = i.CourseTargetId,
                               Text = i.CourseTargetTlAr
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> TrainerLookup(DataSourceLoadOptions loadOptions) {
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

        private void PopulateModel(Course model, IDictionary values) {
            string COURSE_ID = nameof(Course.CourseId);
            string COURSE_TL_AR = nameof(Course.CourseTlAr);
            string COURSE_TL_EN = nameof(Course.CourseTlEn);
            string TRAINER_ID = nameof(Course.TrainerId);
            string COURSE_TARGET_ID = nameof(Course.CourseTargetId);
            string COURSE_DESC_AR = nameof(Course.CourseDescAr);
            string COURSE_DESC_EN = nameof(Course.CourseDescEn);
            string IS_ACTIVE = nameof(Course.IsActive);
            string PUBLISH_DATE = nameof(Course.PublishDate);
            string PIC = nameof(Course.Pic);

            if(values.Contains(COURSE_ID)) {
                model.CourseId = Convert.ToInt32(values[COURSE_ID]);
            }

            if(values.Contains(COURSE_TL_AR)) {
                model.CourseTlAr = Convert.ToString(values[COURSE_TL_AR]);
            }

            if(values.Contains(COURSE_TL_EN)) {
                model.CourseTlEn = Convert.ToString(values[COURSE_TL_EN]);
            }

            if(values.Contains(TRAINER_ID)) {
                model.TrainerId = Convert.ToInt32(values[TRAINER_ID]);
            }

            if(values.Contains(COURSE_TARGET_ID)) {
                model.CourseTargetId = Convert.ToInt32(values[COURSE_TARGET_ID]);
            }

            if(values.Contains(COURSE_DESC_AR)) {
                model.CourseDescAr = Convert.ToString(values[COURSE_DESC_AR]);
            }

            if(values.Contains(COURSE_DESC_EN)) {
                model.CourseDescEn = Convert.ToString(values[COURSE_DESC_EN]);
            }

            if(values.Contains(IS_ACTIVE)) {
                model.IsActive = values[IS_ACTIVE] != null ? Convert.ToBoolean(values[IS_ACTIVE]) : (bool?)null;
            }

            if(values.Contains(PUBLISH_DATE)) {
                model.PublishDate = values[PUBLISH_DATE] != null ? Convert.ToDateTime(values[PUBLISH_DATE]) : (DateTime?)null;
            }

            if(values.Contains(PIC)) {
                model.Pic = Convert.ToString(values[PIC]);
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