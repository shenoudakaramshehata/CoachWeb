using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Coach.Data;
using NToastNotify;
using Coach.Models;

namespace Coach.Areas.Admin.Pages.Countries
{
    public class AddModel : PageModel
    {
        private CoachContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;

        public AddModel(CoachContext context, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;

        }

        public void OnGet()
        { }
        public IActionResult OnPost(Country model)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var uniqeFileName = "";

                if (Response.HttpContext.Request.Form.Files.Count() > 0)
                {
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
                _context.Countries.Add(model);
                _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Country Added successfully");
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
