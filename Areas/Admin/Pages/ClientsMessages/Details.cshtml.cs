using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Coach.Data;
using Coach.Models;
using NToastNotify;

namespace Coach.Areas.Admin.Pages.ClientsMessages
{
    public class DetailsModel : PageModel
    {
        private CoachContext _context;
        private readonly IToastNotification _toastNotification;

        public DetailsModel(CoachContext context,  IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }

        public Contact ContactUsMessages { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                if (id == null)
                {
                    return Redirect("../Error");

                }

                ContactUsMessages = await _context.Contacts.FirstOrDefaultAsync(m => m.ContactId == id);

                if (ContactUsMessages == null)
                {
                    return Redirect("../Error");

                }
            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");

            }

            return Page();
        }
    }
}
