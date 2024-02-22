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
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;

namespace Coach.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TrainersController : Controller
    {
        private readonly CoachContext _context;
       
        private readonly IWebHostEnvironment _hostEnvironment;



        public TrainersController(CoachContext context, IWebHostEnvironment hostEnvironment
           )
        {
            _context = context;
          
            _hostEnvironment = hostEnvironment;


        }
        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var trainers = _context.Trainers.Select(i => new {
                i.TrainerId,
                i.UserId,
                i.FullNameAr,
                i.FullNameEn,
                i.Email,
                i.Mobile,
                i.Tele,
                i.Fax,
                i.GenderId,
                i.CountryId,
                //i.TrainerPlanId,
                i.Pic,
                i.SectionId,
                i.DescriptionAr,
                i.Country,
                i.Section,
                i.DescriptionEn
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "TrainerId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(trainers, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Trainer();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Trainers.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.TrainerId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Trainers.FirstOrDefaultAsync(item => item.TrainerId == key);
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
            var model = await _context.Trainers.FirstOrDefaultAsync(item => item.TrainerId == key);

            _context.Trainers.Remove(model);
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
        public async Task<IActionResult> GenderLookup(DataSourceLoadOptions loadOptions)
        {

            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.Genders
                               orderby i.GenderTlEn
                               select new
                               {
                                   Value = i.GenderId,
                                   Text = i.GenderTlEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.Genders
                           orderby i.GenderTlAr
                           select new
                           {
                               Value = i.GenderId,
                               Text = i.GenderTlAr
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));


        }

        [HttpGet]
        public async Task<IActionResult> SectionLookup(DataSourceLoadOptions loadOptions)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.Sections
                               orderby i.SectionTlEn
                               select new
                               {
                                   Value = i.SectionId,
                                   Text = i.SectionTlEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.Sections
                           orderby i.SectionTlAr
                           select new
                           {
                               Value = i.SectionId,
                               Text = i.SectionTlAr
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));
        }
        [HttpGet]
        public object GetImages([FromQuery] int id)
        {
            var images = _context.TrainerImages.Where(p => p.TrainerId == id).Select(i => new {
                i.Pic,
                i.TrainerId,
                i.TrainerImageId
            });
            return images;
        }

        [HttpPost]
        public int RemoveImageById([FromQuery] int id)
        {
            var Image = _context.TrainerImages.FirstOrDefault(p => p.TrainerImageId == id);
            if (Image != null)
            {
                var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/Trainer/" + Image.Pic);
                if (System.IO.File.Exists(ImagePath))
                {
                    System.IO.File.Delete(ImagePath);
                }
                _context.TrainerImages.Remove(Image);
                _context.SaveChanges();
                return id;

            }
            return 0;

        }

        private void PopulateModel(Trainer model, IDictionary values) {
            string TRAINER_ID = nameof(Trainer.TrainerId);
            string USER_ID = nameof(Trainer.UserId);
            string FULL_NAME_AR = nameof(Trainer.FullNameAr);
            string FULL_NAME_EN = nameof(Trainer.FullNameEn);
            string EMAIL = nameof(Trainer.Email);
            string MOBILE = nameof(Trainer.Mobile);
            string TELE = nameof(Trainer.Tele);
            string FAX = nameof(Trainer.Fax);
            string GENDER_ID = nameof(Trainer.GenderId);
            string COUNTRY_ID = nameof(Trainer.CountryId);
            //string TRAINER_PLAN_ID = nameof(Trainer.TrainerPlanId);
            string PIC = nameof(Trainer.Pic);
            string SECTION_ID = nameof(Trainer.SectionId);
            string DESCRIPTION_AR = nameof(Trainer.DescriptionAr);
            string DESCRIPTION_EN = nameof(Trainer.DescriptionEn);

            if(values.Contains(TRAINER_ID)) {
                model.TrainerId = Convert.ToInt32(values[TRAINER_ID]);
            }

            if(values.Contains(USER_ID)) {
                model.UserId = Convert.ToString(values[USER_ID]);
            }

            if(values.Contains(FULL_NAME_AR)) {
                model.FullNameAr = Convert.ToString(values[FULL_NAME_AR]);
            }

            if(values.Contains(FULL_NAME_EN)) {
                model.FullNameEn = Convert.ToString(values[FULL_NAME_EN]);
            }

            if(values.Contains(EMAIL)) {
                model.Email = Convert.ToString(values[EMAIL]);
            }

            if(values.Contains(MOBILE)) {
                model.Mobile = Convert.ToString(values[MOBILE]);
            }

            if(values.Contains(TELE)) {
                model.Tele = Convert.ToString(values[TELE]);
            }

            if(values.Contains(FAX)) {
                model.Fax = Convert.ToString(values[FAX]);
            }

            if(values.Contains(GENDER_ID)) {
                model.GenderId = values[GENDER_ID] != null ? Convert.ToInt32(values[GENDER_ID]) : (int?)null;
            }

            if(values.Contains(COUNTRY_ID)) {
                model.CountryId = values[COUNTRY_ID] != null ? Convert.ToInt32(values[COUNTRY_ID]) : (int?)null;
            }

            //if(values.Contains(TRAINER_PLAN_ID)) {
            //    model.TrainerPlanId = values[TRAINER_PLAN_ID] != null ? Convert.ToInt32(values[TRAINER_PLAN_ID]) : (int?)null;
            //}

            if(values.Contains(PIC)) {
                model.Pic = Convert.ToString(values[PIC]);
            }

            if(values.Contains(SECTION_ID)) {
                model.SectionId = Convert.ToInt32(values[SECTION_ID]);
            }

            if(values.Contains(DESCRIPTION_AR)) {
                model.DescriptionAr = Convert.ToString(values[DESCRIPTION_AR]);
            }

            if(values.Contains(DESCRIPTION_EN)) {
                model.DescriptionEn = Convert.ToString(values[DESCRIPTION_EN]);
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