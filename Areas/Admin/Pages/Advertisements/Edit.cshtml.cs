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

namespace Coach.Areas.Admin.Pages.Advertisements
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
        public Adz adz { get; set; }
        [BindProperty]
        public int? TranierId { get; set; }
        public int? CampId { get; set; }
        public int? TournamentId { get; set; }
        public int? CourseId { get; set; }

        public IActionResult OnGet(int id)
        {
            try
            {
                adz = _context.Adzs.Where(c => c.AdzId == id).FirstOrDefault();
                if (adz == null)
                {
                    return Redirect("../Error");
                }
                if (adz.EntityTypeId == 1)
                {
                    TranierId = int.Parse(adz.EntityId);
                    adz.EntityId = "";
                }
                else if (adz.EntityTypeId == 2)
                {
                    CampId = int.Parse(adz.EntityId);
                    adz.EntityId = "";
                }
                else if (adz.EntityTypeId == 3)
                {
                    TournamentId = int.Parse(adz.EntityId);
                    adz.EntityId = "";
                }
                else if (adz.EntityTypeId == 4)
                {
                    CourseId = int.Parse(adz.EntityId);
                    adz.EntityId = "";
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
                var model = _context.Adzs.Where(c => c.AdzId == id).FirstOrDefault();
                if (model==null)
                {
                   return Redirect("../Error");
                }

                if (adz.EntityTypeId == 1)
                {
                    model.EntityId = Request.Form["TrainerId"];
                }
                if (adz.EntityTypeId == 2)
                {
                    model.EntityId = Request.Form["CampId"];
                }
                if (adz.EntityTypeId == 3)
                {
                    model.EntityId = Request.Form["TournamentId"];
                }
                if (adz.EntityTypeId == 4)
                {
                    model.EntityId = Request.Form["CourseId"];
                }

                if (adz.EntityTypeId == 5)
                {
                    if (adz.EntityId == null|| adz.EntityId=="")
                    {
                        _toastNotification.AddErrorToastMessage("enter link");

                        adz.AdzPic = model.AdzPic;
                       
                        return Page();
                    }
                    model.EntityId = adz.EntityId;
                }

                var uniqeFileName = "";

                if (Response.HttpContext.Request.Form.Files.Count() > 0)
                {
                    string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/Adz");
                    string ext = Path.GetExtension(Response.HttpContext.Request.Form.Files[0].FileName);
                    uniqeFileName = Guid.NewGuid() + ext;
                    string uploadedImagePath = Path.Combine(uploadFolder, uniqeFileName);
                    using (FileStream fileStream = new FileStream(uploadedImagePath, FileMode.Create))
                    {
                        Response.HttpContext.Request.Form.Files[0].CopyTo(fileStream);
                    }
                    var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/Adz/" + model.AdzPic);
                    if (System.IO.File.Exists(ImagePath))
                    {
                        System.IO.File.Delete(ImagePath);
                    }
                    model.AdzPic = "Images/Adz/" + uniqeFileName;
                }
                model.EntityTypeId = adz.EntityTypeId;
                model.AdzIsActive = adz.AdzIsActive;
                model.AdzOrderIndex = adz.AdzOrderIndex;
                model.CountryId = adz.CountryId;

                _context.Attach(model).State = EntityState.Modified;
                 _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Adz Edited successfully");

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
