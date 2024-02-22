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
using NToastNotify;
using Coach.Data;
using Coach.Models;

namespace Coach.Areas.Admin.Pages.Banners
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
        public Banner banner { get; set; }
        [BindProperty]
        public int? TranierId { get; set; }
        public int? CampId { get; set; }
        public int? TournamentId { get; set; }
        public int? CourseId { get; set; }
       
        public IActionResult OnGet(int id)
        {
            try
            {
                banner = _context.Banners.Where(c => c.BannerId == id).FirstOrDefault();
                if (banner == null)
                {
                    return Redirect("../Error");
                }
                if (banner.EntityTypeId == 1)
                {
                   TranierId = int.Parse(banner.EntityId);
                    banner.EntityId = "";
                }
                else if (banner.EntityTypeId == 2)
                {
                    CampId = int.Parse(banner.EntityId);
                    banner.EntityId = "";
                }
                else if (banner.EntityTypeId == 3)
                {
                    TournamentId = int.Parse(banner.EntityId);
                    banner.EntityId = "";
                }
                else if (banner.EntityTypeId == 4)
                {
                    CourseId = int.Parse(banner.EntityId);
                    banner.EntityId = "";
                }
             
               
            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");

            }


            return Page();
        }
        public IActionResult OnPost(int id)
        
        {
            

            try
            {
                var model = _context.Banners.Where(c => c.BannerId == id).FirstOrDefault();
                if (model==null)
                {
                   return Redirect("../Error");
                }

                if (banner.EntityTypeId == 1)
                {
                    model.EntityId = Request.Form["TrainerId"];
                }
                if (banner.EntityTypeId == 2)
                {
                    model.EntityId = Request.Form["CampId"];
                }
                if (banner.EntityTypeId == 3)
                {
                    model.EntityId = Request.Form["TournamentId"];
                }
                if (banner.EntityTypeId == 4)
                {
                    model.EntityId = Request.Form["CourseId"];
                }

                if (banner.EntityTypeId == 5)
                {
                    if (banner.EntityId == null|| banner.EntityId=="")
                    {
                        _toastNotification.AddErrorToastMessage("enter link");

                        banner.BannerPic = model.BannerPic;
                       
                        return Page();
                    }
                    model.EntityId = banner.EntityId;
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
                    var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/Banner/" + model.BannerPic);
                    if (System.IO.File.Exists(ImagePath))
                    {
                        System.IO.File.Delete(ImagePath);
                    }
                    model.BannerPic = "Images/Banner/"+uniqeFileName;
                }
                model.EntityTypeId = banner.EntityTypeId;
                model.BannerIsActive = banner.BannerIsActive;
                model.BannerOrderIndex = banner.BannerOrderIndex;
                _context.Attach(model).State = EntityState.Modified;
                 _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Banner Edited successfully");

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
