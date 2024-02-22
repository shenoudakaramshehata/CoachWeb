using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Coach.Data;
using Coach.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Coach.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CourseImagesController : Controller
    {
        private CoachContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;


        public CourseImagesController(CoachContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;


        }
        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var courseimage = _context.CourseImages.Select(i => new {
                i.CourseImageId,
                i.CourseId,
                i.Pic
            });

            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "CourseImageId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(courseimage, loadOptions));
        }



     
        [HttpGet]
        public object GetImages([FromQuery] int id)
        {
            var images = _context.CourseImages.Where(p => p.CourseId == id).Select(i => new {
                i.Pic,
                i.CourseId,
                i.CourseImageId
            });
            return images;
        }
        [HttpPost]
        public async Task<int> RemoveImageById([FromQuery] int id)
        {
            var courseImage = _context.CourseImages.FirstOrDefault(p => p.CourseImageId == id);
            if (courseImage!=null)
            {
                var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/Course/" + courseImage.Pic);
                if (System.IO.File.Exists(ImagePath))
                {
                    System.IO.File.Delete(ImagePath);
                }
                _context.CourseImages.Remove(courseImage);
                _context.SaveChanges();
                return id;

            }
            return 0;

        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new CourseImage();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.CourseImages.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.CourseImageId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.CourseImages.FirstOrDefaultAsync(item => item.CourseImageId == key);
            if(model == null)
                return StatusCode(409, "Object not found");

            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task Delete(int key) {
            var model = await _context.CourseImages.FirstOrDefaultAsync(item => item.CourseImageId == key);

            _context.CourseImages.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> CourseLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Courses
                         orderby i.CourseTlAr
                         select new {
                             Value = i.CourseId,
                             Text = i.CourseTlAr
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(CourseImage model, IDictionary values) {
            string COURSE_IMAGE_ID = nameof(CourseImage.CourseImageId);
            string COURSE_ID = nameof(CourseImage.CourseId);
            string PIC = nameof(CourseImage.Pic);

            if(values.Contains(COURSE_IMAGE_ID)) {
                model.CourseImageId = Convert.ToInt32(values[COURSE_IMAGE_ID]);
            }

            if(values.Contains(COURSE_ID)) {
                model.CourseId = Convert.ToInt32(values[COURSE_ID]);
            }

            if(values.Contains(PIC)) {
                model.Pic = Convert.ToString(values[PIC]);
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState) {
            var messages = new List<string>();

            foreach(var entry in modelState) {
                foreach(var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }
    }
}