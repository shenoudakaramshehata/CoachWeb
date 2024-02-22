using Coach.Reports;
using Coach.Data;
using Coach.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace Coach.Areas.Admin.Pages.ReportsPages
{
    public class ReportTestModel : PageModel
    {
        private CoachContext _context;
        public rptTestcs report { get; set; }
        public ReportTestModel(CoachContext context)
        {
            _context = context;
           
        }
       
        public IActionResult OnGet()
        {
            var ds = _context.Genders.ToList();

            report = new rptTestcs();
            report.DataSource = ds;
            
            return Page();

        }
    }
}
