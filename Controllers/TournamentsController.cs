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
namespace Coach.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TournamentsController : Controller
    {
        private readonly CoachContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _hostEnvironment;



        public TournamentsController(CoachContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
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
            var tournaments = _context.Tournaments.Select(i => new {
                i.TournamentId,
                i.UserId,
                i.TournamentTlAr,
                i.TournamentTlEn,
                i.TournamentDescAr,
                i.TournamentDescEn,
                i.IsActive,
                i.StartDate,
                i.EndDate,
                i.Pic,
                i.TournamentTypeId,
                i.CountryId,
                i.TournamentTargetId,
                i.TournamentPlanId,
                i.SubStartDate,
                i.SubEndDate,
                i.SubPrice,
                i.Remarks,
                i.Cost
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "TournamentId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(tournaments, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Tournament();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Tournaments.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.TournamentId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Tournaments.FirstOrDefaultAsync(item => item.TournamentId == key);
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
            var model = await _context.Tournaments.FirstOrDefaultAsync(item => item.TournamentId == key);

            _context.Tournaments.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> TournamentTargetLookup(DataSourceLoadOptions loadOptions)
        {


            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.TournamentTargets
                               orderby i.TournamentTargetTlEn
                               select new
                               {
                                   Value = i.TournamentTargetId,
                                   Text = i.TournamentTargetTlEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.TournamentTargets
                           orderby i.TournamentTargetTlAr
                           select new
                           {
                               Value = i.TournamentTargetId,
                               Text = i.TournamentTargetTlAr
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));




        }

        [HttpGet]
        public async Task<IActionResult> TournamentTypeLookup(DataSourceLoadOptions loadOptions)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.TournamentTypes
                               orderby i.TournamentTypeTlEn
                               select new
                               {
                                   Value = i.TournamentTypeId,
                                   Text = i.TournamentTypeTlEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.TournamentTypes
                           orderby i.TournamentTypeTlAr
                           select new
                           {
                               Value = i.TournamentTypeId,
                               Text = i.TournamentTypeTlAr
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
            var images = _context.TournamentImages.Where(p => p.TournamentId == id).Select(i => new {
                i.Pic,
                i.TournamentId,
                i.TournamentImageId
            });
            return images;
        }

        [HttpPost]
        public int RemoveImageById([FromQuery] int id)
        {
            var Image = _context.TournamentImages.FirstOrDefault(p => p.TournamentImageId == id);
            if (Image != null)
            {
                var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/Tournament/" + Image.Pic);
                if (System.IO.File.Exists(ImagePath))
                {
                    System.IO.File.Delete(ImagePath);
                }
                _context.TournamentImages.Remove(Image);
                _context.SaveChanges();
                return id;

            }
            return 0;

        }

        [HttpGet]
        public async Task<IActionResult> TournamentPlansLookup(DataSourceLoadOptions loadOptions)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.TournamentPlans
                               orderby i.PlanTlEn
                               select new
                               {
                                   Value = i.TournamentPlanId,
                                   Text = i.PlanTlEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.TournamentPlans
                           orderby i.PlanTlAr
                           select new
                           {
                               Value = i.TournamentPlanId,
                               Text = i.PlanTlAr
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));
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


        private void PopulateModel(Tournament model, IDictionary values) {
            string TOURNAMENT_ID = nameof(Tournament.TournamentId);
            string USER_ID = nameof(Tournament.UserId);
            string TOURNAMENT_TL_AR = nameof(Tournament.TournamentTlAr);
            string TOURNAMENT_TL_EN = nameof(Tournament.TournamentTlEn);
            string TOURNAMENT_DESC_AR = nameof(Tournament.TournamentDescAr);
            string TOURNAMENT_DESC_EN = nameof(Tournament.TournamentDescEn);
            string IS_ACTIVE = nameof(Tournament.IsActive);
            string START_DATE = nameof(Tournament.StartDate);
            string END_DATE = nameof(Tournament.EndDate);
            string PIC = nameof(Tournament.Pic);
            string TOURNAMENT_TYPE_ID = nameof(Tournament.TournamentTypeId);
            string COUNTRY_ID = nameof(Tournament.CountryId);
            string TOURNAMENT_TARGET_ID = nameof(Tournament.TournamentTargetId);
            string TOURNAMENT_PLAN_ID = nameof(Tournament.TournamentPlanId);
            string SUB_START_DATE = nameof(Tournament.SubStartDate);
            string SUB_END_DATE = nameof(Tournament.SubEndDate);
            string SUB_PRICE = nameof(Tournament.SubPrice);
            string REMARKS = nameof(Tournament.Remarks);
            string COST = nameof(Tournament.Cost);

            if(values.Contains(TOURNAMENT_ID)) {
                model.TournamentId = Convert.ToInt32(values[TOURNAMENT_ID]);
            }

            if(values.Contains(USER_ID)) {
                model.UserId = Convert.ToString(values[USER_ID]);
            }

            if(values.Contains(TOURNAMENT_TL_AR)) {
                model.TournamentTlAr = Convert.ToString(values[TOURNAMENT_TL_AR]);
            }

            if(values.Contains(TOURNAMENT_TL_EN)) {
                model.TournamentTlEn = Convert.ToString(values[TOURNAMENT_TL_EN]);
            }

            if(values.Contains(TOURNAMENT_DESC_AR)) {
                model.TournamentDescAr = Convert.ToString(values[TOURNAMENT_DESC_AR]);
            }

            if(values.Contains(TOURNAMENT_DESC_EN)) {
                model.TournamentDescEn = Convert.ToString(values[TOURNAMENT_DESC_EN]);
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

            if(values.Contains(TOURNAMENT_TYPE_ID)) {
                model.TournamentTypeId = Convert.ToInt32(values[TOURNAMENT_TYPE_ID]);
            }

            if(values.Contains(COUNTRY_ID)) {
                model.CountryId = Convert.ToInt32(values[COUNTRY_ID]);
            }

            if(values.Contains(TOURNAMENT_TARGET_ID)) {
                model.TournamentTargetId = Convert.ToInt32(values[TOURNAMENT_TARGET_ID]);
            }

            if(values.Contains(TOURNAMENT_PLAN_ID)) {
                model.TournamentPlanId = Convert.ToInt32(values[TOURNAMENT_PLAN_ID]);
            }

            if(values.Contains(SUB_START_DATE)) {
                model.SubStartDate = values[SUB_START_DATE] != null ? Convert.ToDateTime(values[SUB_START_DATE]) : (DateTime?)null;
            }

            if(values.Contains(SUB_END_DATE)) {
                model.SubEndDate = values[SUB_END_DATE] != null ? Convert.ToDateTime(values[SUB_END_DATE]) : (DateTime?)null;
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