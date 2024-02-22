using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Coach.Data;
using Coach.Models;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Coach.Areas.Admin.Pages.Sections
{
    public class IndexModel : PageModel
    {

        private CoachContext _context;
        private readonly IToastNotification _toastNotification;

        public IndexModel(CoachContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }
        [BindProperty(SupportsGet = true)]
        public List<Section> sectionList { get; set; }
        public async Task<IActionResult> OnGet()
        {
            try
            {
                sectionList = await _context.Sections.ToListAsync();

            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");

            }
            return Page();
        }
    }
}
