using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Coach.Data;
using Coach.Models;
using NToastNotify;

namespace Coach.Areas.Admin.Pages.country
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
        public Country country { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {
            
            try
            {

                country = await _context.Countries.FirstOrDefaultAsync(m => m.CountryId == id);
                if (country == null)
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
                var model = _context.Countries.Where(c => c.CountryId == id).FirstOrDefault();
                if (model == null)
                {
                    return Page();
                }
                var uniqeFileName = "";

                if (Response.HttpContext.Request.Form.Files.Count() > 0)
                {
                    var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/Country/" + model.CountryPic);
                    if (System.IO.File.Exists(ImagePath))
                    {
                        System.IO.File.Delete(ImagePath);
                    }
                    string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/Country");
                    string ext = Path.GetExtension(Response.HttpContext.Request.Form.Files[0].FileName);
                    uniqeFileName = Guid.NewGuid().ToString("N") + ext;
                    string uploadedImagePath = Path.Combine(uploadFolder, uniqeFileName);
                    using (FileStream fileStream = new FileStream(uploadedImagePath, FileMode.Create))
                    {
                        Response.HttpContext.Request.Form.Files[0].CopyTo(fileStream);
                    }
                    model.CountryPic = "Images/Country/" + uniqeFileName;
                }

                model.CountryIsActive = country.CountryIsActive;
                model.CountryTlAr = country.CountryTlAr;
                model.CountryTlEn = country.CountryTlEn;
                model.CountryOrderIndex = country.CountryOrderIndex;

                _context.Attach(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Country Edited successfully");

            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong");

            }

            return RedirectToPage("./Index");
        }

    }
}
