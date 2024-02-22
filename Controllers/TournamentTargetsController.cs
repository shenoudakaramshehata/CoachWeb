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
    public class TournamentTargetsController : Controller
    {
        private CoachContext _context;

        public TournamentTargetsController(CoachContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var tournamenttarget = _context.TournamentTargets.Select(i => new {
                i.TournamentTargetId,
                i.TournamentTargetTlAr,
                i.TournamentTargetTlEn,
                i.IsActive
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "TournamentTargetId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(tournamenttarget, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values)
        {
            var model = new TournamentTarget();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.TournamentTargets.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.TournamentTargetId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values)
        {
            var model = await _context.TournamentTargets.FirstOrDefaultAsync(item => item.TournamentTargetId == key);
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
        public async Task<IActionResult> Delete(int key)
        {
            var tournament = _context.Tournaments.Any(c => c.TournamentTargetId == key);
            if (tournament == false)
            {
                var model = await _context.TournamentTargets.FirstOrDefaultAsync(item => item.TournamentTargetId == key);
                _context.TournamentTargets.Remove(model);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {

                return StatusCode(409, "You cannot delete this Tournament Target");

            }

        }

        private void PopulateModel(TournamentTarget model, IDictionary values)
        {
            string CAMP_TARGET_ID = nameof(TournamentTarget.TournamentTargetId);
            string CAMP_TARGET_TL_AR = nameof(TournamentTarget.TournamentTargetTlAr);
            string CAMP_TARGET_TL_EN = nameof(TournamentTarget.TournamentTargetTlEn);
            string IS_ACTIVE = nameof(TournamentTarget.IsActive);

            if (values.Contains(CAMP_TARGET_ID))
            {
                model.TournamentTargetId = Convert.ToInt32(values[CAMP_TARGET_ID]);
            }

            if (values.Contains(CAMP_TARGET_TL_AR))
            {
                model.TournamentTargetTlAr = Convert.ToString(values[CAMP_TARGET_TL_AR]);
            }

            if (values.Contains(CAMP_TARGET_TL_EN))
            {
                model.TournamentTargetTlEn = Convert.ToString(values[CAMP_TARGET_TL_EN]);
            }

            if (values.Contains(IS_ACTIVE))
            {
                model.IsActive = values[IS_ACTIVE] != null ? Convert.ToBoolean(values[IS_ACTIVE]) : (bool?)null;
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