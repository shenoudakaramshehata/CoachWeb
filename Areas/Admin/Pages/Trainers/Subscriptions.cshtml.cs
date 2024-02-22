using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coach.Data;
using Coach.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Coach.Areas.Admin.Pages.Trainers
{
    public class SubscriptionsModel : PageModel
    {
        private CoachContext _context;

        public SubscriptionsModel(CoachContext context)
        {
            _context = context;

        }
        [BindProperty]
        public int trainerId { get; set; }
        public IActionResult OnGet(int id)
        {
            trainerId = id;


            return Page();
        }
    }
}
