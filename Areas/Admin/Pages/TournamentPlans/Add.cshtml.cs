using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Coach.Data;

namespace Coach.Areas.Admin.Pages.TournamentPlans
{
    public class AddModel : PageModel
    {
        private CoachContext _context;
        private readonly IToastNotification _toastNotification;

        public AddModel(CoachContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }

        public void OnGet()
        { }
        public IActionResult OnPost(Models.TournamentPlan model)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                _context.TournamentPlans.Add(model);
                _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("TournamentPlan Added successfully");
            }
            catch (Exception)
            {


                _toastNotification.AddErrorToastMessage("Something went wrong");
                return Page();
            }


            return Redirect("./Index");

        }
    }
}
