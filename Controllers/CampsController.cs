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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using System.IO;
using Coach.ReportsModel;


namespace Coach.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CampsController : Controller
    {
        private readonly CoachContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _hostEnvironment;
      


        public CampsController(CoachContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
            ApplicationDbContext db, RoleManager<IdentityRole> roleManager, IWebHostEnvironment hostEnvironment
           )
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _db = db;
            _roleManager = roleManager;
            _hostEnvironment = hostEnvironment;


        }


        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var camps = _context.Camps.Select(i => new {
                i.CampId,
                i.UserId,
                i.CampTlAr,
                i.CampTlEn,
                i.CampDescAr,
                i.CampDescEn,
                i.IsActive,
                i.StartDate,
                i.EndDate,
                i.Pic,
                i.CampTypeId,
                i.CountryId,
                i.CampTargetId,
                i.CampPlanId,
                i.JoinStart,
                i.JoinEnd,
                i.SubPrice,
                i.Remarks,
                i.Cost
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "CampId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(camps, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Camp();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Camps.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.CampId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Camps.FirstOrDefaultAsync(item => item.CampId == key);
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
            var model = await _context.Camps.FirstOrDefaultAsync(item => item.CampId == key);

            _context.Camps.Remove(model);
            await _context.SaveChangesAsync();
        }

        [HttpGet]
        public async Task<IActionResult> CampTargetLookup(DataSourceLoadOptions loadOptions)
        {


            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.CampTargets
                               orderby i.CampTargetTlEn
                               select new
                               {
                                   Value = i.CampTargetId,
                                   Text = i.CampTargetTlEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.CampTargets
                           orderby i.CampTargetTlAr
                           select new
                           {
                               Value = i.CampTargetId,
                               Text = i.CampTargetTlAr
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));




        }

        [HttpGet]
        public async Task<IActionResult> CampTypeLookup(DataSourceLoadOptions loadOptions)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.CampTypes
                               orderby i.CampTypeTlEn
                               select new
                               {
                                   Value = i.CampTypeId,
                                   Text = i.CampTypeTlEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.CampTypes
                           orderby i.CampTypeTlAr
                           select new
                           {
                               Value = i.CampTypeId,
                               Text = i.CampTypeTlAr
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));


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
        [HttpGet]
        public object GetImages([FromQuery] int id)
        {
            var images = _context.CampImages.Where(p => p.CampId == id).Select(i => new {
                i.Pic,
                i.CampId,
                i.CampImageId
            });
            return images;
        }

        [HttpPost]
        public int RemoveImageById([FromQuery] int id)
        {
            var Image = _context.CampImages.FirstOrDefault(p => p.CampImageId == id);
            if (Image != null)
            {
                var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/Camp/" + Image.Pic);
                if (System.IO.File.Exists(ImagePath))
                {
                    System.IO.File.Delete(ImagePath);
                }
                _context.CampImages.Remove(Image);
                _context.SaveChanges();
                return id;

            }
            return 0;

        }

        [HttpGet]
        public async Task<IActionResult> CampPlansLookup(DataSourceLoadOptions loadOptions) {
           var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.CampPlans
                               orderby i.PlanTlEn
                               select new
                               {
                                   Value = i.CampPlanId,
                                   Text = i.PlanTlEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.CampPlans
                           orderby i.PlanTlAr
                           select new
                           {
                               Value = i.CampPlanId,
                               Text = i.PlanTlAr
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));
        }
        [HttpGet]
        public async Task<IActionResult> CampUsersLookup()
        {
            List<Users> ds = new List<Users>();

            var camps = _context.Camps.Select(e=>e.UserId).Distinct().ToList();
            foreach (var item in camps)
            {
                var user = await _userManager.FindByIdAsync(item);
                var userobj = new Users()
                {
                    FullName = user.FullName,
                    UserId = user.Id
                };
                ds.Add(userobj);
            }
              var lookup= from i in ds
                          select new
                          {
                              Value = i.UserId,
                              Text = i.FullName
                          };
            
            return Json(lookup);

        }


        [HttpGet]
        public async Task<IActionResult> UsersLookup(DataSourceLoadOptions loadOptions)
        {
                var lookup = from i in _db.Users
                               orderby i.Email
                               select new
                               {
                                   Value = i.Id,
                                   Text = i.Email
                               };
                return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
          
        }


        private void PopulateModel(Camp model, IDictionary values) {
            string CAMP_ID = nameof(Camp.CampId);
            string USER_ID = nameof(Camp.UserId);
            string CAMP_TL_AR = nameof(Camp.CampTlAr);
            string CAMP_TL_EN = nameof(Camp.CampTlEn);
            string CAMP_DESC_AR = nameof(Camp.CampDescAr);
            string CAMP_DESC_EN = nameof(Camp.CampDescEn);
            string IS_ACTIVE = nameof(Camp.IsActive);
            string START_DATE = nameof(Camp.StartDate);
            string END_DATE = nameof(Camp.EndDate);
            string PIC = nameof(Camp.Pic);
            string CAMP_TYPE_ID = nameof(Camp.CampTypeId);
            string COUNTRY_ID = nameof(Camp.CountryId);
            string CAMP_TARGET_ID = nameof(Camp.CampTargetId);
            string CAMP_PLAN_ID = nameof(Camp.CampPlanId);
            string SUB_START_DATE = nameof(Camp.JoinStart);
            string SUB_END_DATE = nameof(Camp.JoinEnd);
            string SUB_PRICE = nameof(Camp.SubPrice);
            string REMARKS = nameof(Camp.Remarks);
            string COST = nameof(Camp.Cost);

            if(values.Contains(CAMP_ID)) {
                model.CampId = Convert.ToInt32(values[CAMP_ID]);
            }

            if(values.Contains(USER_ID)) {
                model.UserId = Convert.ToString(values[USER_ID]);
            }

            if(values.Contains(CAMP_TL_AR)) {
                model.CampTlAr = Convert.ToString(values[CAMP_TL_AR]);
            }

            if(values.Contains(CAMP_TL_EN)) {
                model.CampTlEn = Convert.ToString(values[CAMP_TL_EN]);
            }

            if(values.Contains(CAMP_DESC_AR)) {
                model.CampDescAr = Convert.ToString(values[CAMP_DESC_AR]);
            }

            if(values.Contains(CAMP_DESC_EN)) {
                model.CampDescEn = Convert.ToString(values[CAMP_DESC_EN]);
            }

          

            if(values.Contains(IS_ACTIVE)) {
                model.IsActive = values[IS_ACTIVE] != null ? Convert.ToBoolean(values[IS_ACTIVE]) : (bool?)null;
            }

            if(values.Contains(START_DATE)) {
                model.StartDate = values[START_DATE] != null ? Convert.ToDateTime(values[START_DATE]) : (DateTime?)null;
            }

            if(values.Contains(END_DATE)) {
                model.EndDate = values[END_DATE] != null ? Convert.ToDateTime(values[END_DATE]) : (DateTime?)null;
            }

            if(values.Contains(PIC)) {
                model.Pic = Convert.ToString(values[PIC]);
            }

            if(values.Contains(CAMP_TYPE_ID)) {
                model.CampTypeId = Convert.ToInt32(values[CAMP_TYPE_ID]);
            }

            if(values.Contains(COUNTRY_ID)) {
                model.CountryId = Convert.ToInt32(values[COUNTRY_ID]);
            }

            if(values.Contains(CAMP_TARGET_ID)) {
                model.CampTargetId = Convert.ToInt32(values[CAMP_TARGET_ID]);
            }

            if(values.Contains(CAMP_PLAN_ID)) {
                model.CampPlanId = Convert.ToInt32(values[CAMP_PLAN_ID]);
            }

            if(values.Contains(SUB_START_DATE)) {
                model.JoinStart = values[SUB_START_DATE] != null ? Convert.ToDateTime(values[SUB_START_DATE]) : (DateTime?)null;
            }

            if(values.Contains(SUB_END_DATE)) {
                model.JoinEnd = values[SUB_END_DATE] != null ? Convert.ToDateTime(values[SUB_END_DATE]) : (DateTime?)null;
            }

            if(values.Contains(SUB_PRICE)) {
                model.SubPrice = values[SUB_PRICE] != null ? Convert.ToDouble(values[SUB_PRICE], CultureInfo.InvariantCulture) : (double?)null;
            }

            if(values.Contains(REMARKS)) {
                model.Remarks = Convert.ToString(values[REMARKS]);
            }

            if(values.Contains(COST)) {
                model.Cost = values[COST] != null ? Convert.ToDouble(values[COST], CultureInfo.InvariantCulture) : (double?)null;
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