using Coach.Data;
using Coach.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Coach.Pages
{
    public class faildCoursePaymentModel : PageModel
    {
        public class failedModel : PageModel
        {
            private CoachContext _context;
            public Course Course { get; set; }
            public failedModel(CoachContext context)
            {
                _context = context;
            }
            public void OnGet(string payment_type, string PaymentID, string Result, int OrderID, DateTime? PostDate, string TranID,
            string Ref, string TrackID, string Auth)
            {
                if (OrderID != 0)
                {
                    Course = _context.Courses.Find(OrderID);
                    if (Course != null)
                    {
                        _context.Courses.Remove(Course);
                    }

                    _context.SaveChanges();

                }

            }
        }
    }
}
