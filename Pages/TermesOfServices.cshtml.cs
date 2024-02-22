using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Coach.Data;

namespace Coach.Pages
{
    public class TermesOfServicesModel : PageModel
    {

        private readonly ILogger<PrivacyModel> _logger;
        public IRequestCultureFeature locale;
        public string BrowserCulture;
        private CoachContext _context;


        public string ContentAr { get; set; }

        public string ContentEn { get; set; }


        public TermesOfServicesModel(CoachContext context)
        {
            _context = context;

        }


        public void OnGet()
        {
            locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            BrowserCulture = locale.RequestCulture.UICulture.ToString();
            var pageContent = _context.PageContents.FirstOrDefault(p => p.PageContentId == 2);
            if (pageContent != null)
            {
                ContentAr = pageContent.ContentAr;
                ContentEn = pageContent.ContentEn;

            }

        }
    }
}
