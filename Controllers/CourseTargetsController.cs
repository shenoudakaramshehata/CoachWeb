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

namespace Coach.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CourseTargetsController : Controller
    {
        private CoachContext _context;

        public CourseTargetsController(CoachContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var coursetarget = _context.CourseTargets.Select(i => new {
                i.CourseTargetId,
                i.CourseTargetTlAr,
                i.CourseTargetTlEn,
                i.IsActive
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "CourseTargetId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(coursetarget, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new CourseTarget();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.CourseTargets.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.CourseTargetId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.CourseTargets.FirstOrDefaultAsync(item => item.CourseTargetId == key);
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
        public async Task<IActionResult> Delete(int key)
        {
            var course = _context.Courses.Any(c => c.CourseTargetId == key);
            if (course == false)
            {
                var model = await _context.CourseTargets.FirstOrDefaultAsync(item => item.CourseTargetId == key);
                _context.CourseTargets.Remove(model);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {

                return StatusCode(409, "You cannot delete this Course Target");

            }

        }


        private void PopulateModel(CourseTarget model, IDictionary values) {
            string COURSE_TARGET_ID = nameof(CourseTarget.CourseTargetId);
            string COURSE_TARGET_TL_AR = nameof(CourseTarget.CourseTargetTlAr);
            string COURSE_TARGET_TL_EN = nameof(CourseTarget.CourseTargetTlEn);
            string IS_ACTIVE = nameof(CourseTarget.IsActive);

            if(values.Contains(COURSE_TARGET_ID)) {
                model.CourseTargetId = Convert.ToInt32(values[COURSE_TARGET_ID]);
            }

            if(values.Contains(COURSE_TARGET_TL_AR)) {
                model.CourseTargetTlAr = Convert.ToString(values[COURSE_TARGET_TL_AR]);
            }

            if(values.Contains(COURSE_TARGET_TL_EN)) {
                model.CourseTargetTlEn = Convert.ToString(values[COURSE_TARGET_TL_EN]);
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