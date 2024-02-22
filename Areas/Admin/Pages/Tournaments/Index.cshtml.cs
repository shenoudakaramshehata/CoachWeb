    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coach.Data;
using Coach.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;

namespace Coach.Areas.Admin.Pages.Tournaments
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
        public List<Tournament> tournamentList { get; set; }
        public ActionResult OnGet()
        {
            try
            {
                tournamentList = _context.Tournaments.ToList();

            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");

            }
            return Page();
        }
    }
}
