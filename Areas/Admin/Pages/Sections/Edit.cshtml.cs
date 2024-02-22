using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Coach.Data;
using Coach.Models;
using NToastNotify;

namespace Coach.Areas.Admin.Pages.Sections
{
    public class EditModel : PageModel
    {

        private CoachContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;

        public EditModel(CoachContext context, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;

        }


        [BindProperty]
        public Section section { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {
           
            try
            {

                
                section = await _context.Sections.FirstOrDefaultAsync(m => m.SectionId == id);
                if (section == null)
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




        public async Task<IActionResult> OnPostAsync(int id)
        {
          
            
            try
            {
                var model = _context.Sections.Where(c => c.SectionId == id).FirstOrDefault();
                if (model == null)
                {
                    return Page();
                }
                var uniqeFileName = "";

                if (Response.HttpContext.Request.Form.Files.Count() > 0)
                {
                    var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/Section/" + model.SectionPic);
                    if (System.IO.File.Exists(ImagePath))
                    {
                        System.IO.File.Delete(ImagePath);
                    }
                    string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/Section");
                    string ext = Path.GetExtension(Response.HttpContext.Request.Form.Files[0].FileName);
                    uniqeFileName = Guid.NewGuid() + ext;
                    string uploadedImagePath = Path.Combine(uploadFolder, uniqeFileName);
                    using (FileStream fileStream = new FileStream(uploadedImagePath, FileMode.Create))
                    {
                        Response.HttpContext.Request.Form.Files[0].CopyTo(fileStream);
                    }
                    model.SectionPic = "Images/Section/" + uniqeFileName;
                }

                model.IsActive = section.IsActive;
                model.SectionTlAr = section.SectionTlAr;
                model.SectionTlEn = section.SectionTlEn;
                model.SectionOrderIndex = section.SectionOrderIndex;

                _context.Attach(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Section Edited successfully");

            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
                return Page();

            }

            return RedirectToPage("./Index");
        }

    }
}
