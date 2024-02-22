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
    public class LookupController : Controller
    {
        private CoachContext _context;

        public LookupController(CoachContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> CountriesLookup(DataSourceLoadOptions loadOptions)
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

    }
}