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
    public class IndexModel : PageModel
    {
        private CoachContext _context;

        public IndexModel(CoachContext context)
        {
            _context = context;

        }
        
        public IActionResult OnGet()
        {
            return Page();
        }
    }
}
