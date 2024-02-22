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
using Coach.Models;

namespace Coach.Areas.Admin.Pages.Banners
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
        {

        }
        public IActionResult OnPost(Banner model)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                if (model.EntityTypeId==1)
                {
                    model.EntityId=Request.Form["TrainerId"];
                }
                if (model.EntityTypeId==2)
                {
                    model.EntityId=Request.Form["CampId"];
                }
                if (model.EntityTypeId==3)
                {
                    model.EntityId=Request.Form["TournamentId"];
                }
                if (model.EntityTypeId==4)
                {
                    model.EntityId=Request.Form["CourseId"];
                }

                if (model.EntityTypeId == 5)
                {
                    if (model.EntityId == null|| model.EntityId=="")
                    {
                        _toastNotification.AddErrorToastMessage("enter link");
                        return Page();
                    }

                }
                var uniqeFileName = "";

                if (Response.HttpContext.Request.Form.Files.Count() > 0)
                {
                    string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/Banner");
                    string ext = Path.GetExtension(Response.HttpContext.Request.Form.Files[0].FileName);
                    uniqeFileName = Guid.NewGuid() + ext;
                    string uploadedImagePath = Path.Combine(uploadFolder, uniqeFileName);
                    using (FileStream fileStream = new FileStream(uploadedImagePath, FileMode.Create))
                    {
                        Response.HttpContext.Request.Form.Files[0].CopyTo(fileStream);
                    }
                    model.BannerPic = "Images/Banner/" + uniqeFileName;
                }
                _context.Banners.Add(model);
                _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Banner Added successfully");
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
