using DevExtreme.AspNet.Mvc;
using Coach.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coach.Data;
using Microsoft.AspNetCore.Localization;

namespace Coach.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class StatisticsController : Controller
    {
        private CoachContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StatisticsController(Coach.Data.CoachContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public object GetTrainersPerCountry(DataSourceLoadOptions loadOptions)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var listEn = _context.Countries.Include(c => c.Trainers).GroupBy(c => c.CountryId).Select(g => new
                {
                    name = _context.Countries.FirstOrDefault(r => r.CountryId == g.Key).CountryTlEn,
                    count = _context.Countries.FirstOrDefault(r => r.CountryId == g.Key).Trainers.Count()

                }).OrderByDescending(r => r.count);

                return listEn;

            }
            var listAr = _context.Countries.Include(c => c.Trainers).GroupBy(c => c.CountryId).Select(g => new
            {
                name = _context.Countries.FirstOrDefault(r => r.CountryId == g.Key).CountryTlAr,
                count = _context.Countries.FirstOrDefault(r => r.CountryId == g.Key).Trainers.Count()

            }).OrderByDescending(r => r.count);

            return listAr;


        }
        [HttpGet]
        public object GetTrainersPerSection(DataSourceLoadOptions loadOptions)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var listEn = _context.Sections.Include(c => c.Trainers).GroupBy(c => c.SectionId).Select(g => new
                {
                    name = _context.Sections.FirstOrDefault(r => r.SectionId == g.Key).SectionTlEn,
                    count = _context.Sections.FirstOrDefault(r => r.SectionId == g.Key).Trainers.Count()

                }).OrderByDescending(r => r.count);

                return listEn;

            }
            var listAr = _context.Sections.Include(c => c.Trainers).GroupBy(c => c.SectionId).Select(g => new
            {
                name = _context.Sections.FirstOrDefault(r => r.SectionId == g.Key).SectionTlAr,
                count = _context.Sections.FirstOrDefault(r => r.SectionId == g.Key).Trainers.Count()

            }).OrderByDescending(r => r.count);


            return listAr;


        }
        [HttpGet]
        public object GetCampsPerCountry(DataSourceLoadOptions loadOptions)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var listEn = _context.Countries.Include(c => c.Camps).GroupBy(c => c.CountryId).Select(g => new
                {
                    name = _context.Countries.FirstOrDefault(r => r.CountryId == g.Key).CountryTlEn,
                    count = _context.Countries.FirstOrDefault(r => r.CountryId == g.Key).Camps.Count()

                }).OrderByDescending(r => r.count);

                return listEn;

            }
            var listAr = _context.Countries.Include(c => c.Camps).GroupBy(c => c.CountryId).Select(g => new
            {
                name = _context.Countries.FirstOrDefault(r => r.CountryId == g.Key).CountryTlAr,
                count = _context.Countries.FirstOrDefault(r => r.CountryId == g.Key).Camps.Count()

            }).OrderByDescending(r => r.count);

            return listAr;


        }
        [HttpGet]
        public object GetCoursesPerCountry(DataSourceLoadOptions loadOptions)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var listEn = _context.Countries.Include(c => c.Trainers).GroupBy(c => c.CountryId).Select(g => new
                {
                    name = _context.Countries.FirstOrDefault(r => r.CountryId == g.Key).CountryTlEn,
                    count = _context.Courses.Where(c => c.Trainer.CountryId == g.Key).Count()

                }).OrderByDescending(r => r.count);

                return listEn;

            }
            var listAr = _context.Countries.Include(c => c.Trainers).GroupBy(c => c.CountryId).Select(g => new
            {
                name = _context.Countries.FirstOrDefault(r => r.CountryId == g.Key).CountryTlAr,
                count = _context.Courses.Where(c => c.Trainer.CountryId == g.Key).Count()

            }).OrderByDescending(r => r.count);

            return listAr;


        }
        [HttpGet]
        public object GetTournamentsPerCountry(DataSourceLoadOptions loadOptions)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var listEn = _context.Countries.Include(c => c.Tournaments).GroupBy(c => c.CountryId).Select(g => new
                {
                    name = _context.Countries.FirstOrDefault(r => r.CountryId == g.Key).CountryTlEn,
                    count = _context.Countries.FirstOrDefault(r => r.CountryId == g.Key).Tournaments.Count()

                }).OrderByDescending(r => r.count);

                return listEn;

            }
            var listAr = _context.Countries.Include(c => c.Tournaments).GroupBy(c => c.CountryId).Select(g => new
            {
                name = _context.Countries.FirstOrDefault(r => r.CountryId == g.Key).CountryTlAr,
                count = _context.Countries.FirstOrDefault(r => r.CountryId == g.Key).Tournaments.Count()

            }).OrderByDescending(r => r.count);

            return listAr;


        }




    }
}
