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
    public class CampTypesController : Controller
    {
        private CoachContext _context;

        public CampTypesController(CoachContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var camptype = _context.CampTypes.Select(i => new {
                i.CampTypeId,
                i.CampTypeTlAr,
                i.CampTypeTlEn,
                i.IsActive
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "CampTypeId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(camptype, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new CampType();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.CampTypes.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.CampTypeId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.CampTypes.FirstOrDefaultAsync(item => item.CampTypeId == key);
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
            var camp = _context.Camps.Any(c => c.CampTypeId == key);
            if (camp == false)
            {
                var model = await _context.CampTypes.FirstOrDefaultAsync(item => item.CampTypeId == key);

                _context.CampTypes.Remove(model);
                await _context.SaveChangesAsync();

                return Ok();
            }
            else
            {

                return StatusCode(409, "You cannot delete this Camp type");

            }

        }
        private void PopulateModel(CampType model, IDictionary values) {
            string CAMP_TYPE_ID = nameof(CampType.CampTypeId);
            string CAMP_TYPE_TL_AR = nameof(CampType.CampTypeTlAr);
            string CAMP_TYPE_TL_EN = nameof(CampType.CampTypeTlEn);
            string IS_ACTIVE = nameof(CampType.IsActive);

            if(values.Contains(CAMP_TYPE_ID)) {
                model.CampTypeId = Convert.ToInt32(values[CAMP_TYPE_ID]);
            }

            if(values.Contains(CAMP_TYPE_TL_AR)) {
                model.CampTypeTlAr = Convert.ToString(values[CAMP_TYPE_TL_AR]);
            }

            if(values.Contains(CAMP_TYPE_TL_EN)) {
                model.CampTypeTlEn = Convert.ToString(values[CAMP_TYPE_TL_EN]);
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