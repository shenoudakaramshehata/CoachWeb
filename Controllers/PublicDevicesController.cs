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
    public class PublicDevicesController : Controller
    {
        private CoachContext _context;

        public PublicDevicesController(CoachContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var publicdevice = _context.PublicDevices.Select(i => new {
                i.PublicDeviceId,
                i.CountryId,
                i.DeviceId,
                i.IsAndroiodDevice
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "PublicDeviceId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(publicdevice, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values)
        {
            var model = new PublicDevice();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.PublicDevices.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.PublicDeviceId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values)
        {
            var model = await _context.PublicDevices.FirstOrDefaultAsync(item => item.PublicDeviceId == key);
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
            var model = await _context.PublicDevices.FirstOrDefaultAsync(item => item.PublicDeviceId == key);

            _context.PublicDevices.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> CountryLookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in _context.Countries
                         orderby i.CountryTlAr
                         select new
                         {
                             Value = i.CountryId,
                             Text = i.CountryTlAr
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(PublicDevice model, IDictionary values)
        {
            string PUBLIC_DEVICE_ID = nameof(PublicDevice.PublicDeviceId);
            string COUNTRY_ID = nameof(PublicDevice.CountryId);
            string DEVICE_ID = nameof(PublicDevice.DeviceId);
            string IS_ANDROIOD_DEVICE = nameof(PublicDevice.IsAndroiodDevice);

            if (values.Contains(PUBLIC_DEVICE_ID))
            {
                model.PublicDeviceId = Convert.ToInt32(values[PUBLIC_DEVICE_ID]);
            }

            if (values.Contains(COUNTRY_ID))
            {
                model.CountryId = Convert.ToInt32(values[COUNTRY_ID]);
            }

            if (values.Contains(DEVICE_ID))
            {
                model.DeviceId = Convert.ToString(values[DEVICE_ID]);
            }

            if (values.Contains(IS_ANDROIOD_DEVICE))
            {
                model.IsAndroiodDevice = Convert.ToBoolean(values[IS_ANDROIOD_DEVICE]);
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
