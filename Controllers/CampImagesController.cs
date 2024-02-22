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
    public class CampImagesController : Controller
    {
        private CoachContext _context;

        public CampImagesController(CoachContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var campimage = _context.CampImages.Select(i => new {
                i.CampImageId,
                i.CampId,
                i.Pic
            });

            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "CampImageId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(campimage, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values)
        {
            var model = new CampImage();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.CampImages.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.CampImageId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values)
        {
            var model = await _context.CampImages.FirstOrDefaultAsync(item => item.CampImageId == key);
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
            var model = await _context.CampImages.FirstOrDefaultAsync(item => item.CampImageId == key);

            _context.CampImages.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> CampLookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in _context.Camps
                         orderby i.CampTlAr
                         select new
                         {
                             Value = i.CampId,
                             Text = i.CampTlAr
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(CampImage model, IDictionary values)
        {
            string CAMP_IMAGE_ID = nameof(CampImage.CampImageId);
            string CAMP_ID = nameof(CampImage.CampId);
            string PIC = nameof(CampImage.Pic);

            if (values.Contains(CAMP_IMAGE_ID))
            {
                model.CampImageId = Convert.ToInt32(values[CAMP_IMAGE_ID]);
            }

            if (values.Contains(CAMP_ID))
            {
                model.CampId = Convert.ToInt32(values[CAMP_ID]);
            }

            if (values.Contains(PIC))
            {
                model.Pic = Convert.ToString(values[PIC]);
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
