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

namespace Coach.Areas.Admin.ClientsMessages
{
    public class DeleteCustomerMsgModel : PageModel
    {
        private readonly Coach.Data.CoachContext _context;
        private readonly IToastNotification _toastNotification;
        public DeleteCustomerMsgModel(Coach.Data.CoachContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        public Contact ContactUsMessages { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
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
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            try
            {
                if (id == null)
                {
                    return Redirect("../Error");
                }

                ContactUsMessages = await _context.Contacts.FindAsync(id);

                if (ContactUsMessages != null)
                {
                    _context.Contacts.Remove(ContactUsMessages);
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Customer Message Deleted Successfully");
                }

            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
            }
           
            return Redirect("/Admin/ClientsMessages/Index");
        }
    }
}

