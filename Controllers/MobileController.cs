using Coach.Data;
using Coach.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coach.Entities;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using Coach.ViewModel;
using DevExpress.XtraRichEdit.Model;
using Coach.ViewModels;
using MimeKit;
using Microsoft.AspNetCore.WebUtilities;


namespace Coach.Controllers
{

    [Route("api/[Controller]/[action]")]

    public class MobileController : Controller
    {

        private readonly CoachContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IEmailSender _emailSender;
        public HttpClient httpClient { get; set; }
        public MobileController(CoachContext context, HttpClient httpClient, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
            ApplicationDbContext db, RoleManager<IdentityRole> roleManager, IWebHostEnvironment hostEnvironment,
            IEmailSender emailSender)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _db = db;
            _roleManager = roleManager;
            _hostEnvironment = hostEnvironment;
            _emailSender = emailSender;
            this.httpClient = httpClient;


        }
        [HttpGet]
        public async Task<ActionResult<ApplicationUser>> Login([FromQuery] string Email, [FromQuery] string Password)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(Email);
                if (user == null)
                {
                    return Ok(new { Status = false, Message = "Something went wrong" });
                }
                var result = await _signInManager.CheckPasswordSignInAsync(user, Password, true);
                if (result.Succeeded)
                {
                    return Ok(new { Status = true, Message = "User Login successfully!", user });
                }
                return Ok(new { Status = false, Message = "Something went wrong" });
            }
            catch (Exception)
            {

                throw;
            }



        }
        [HttpGet]
        public async Task<IActionResult> GetRandomTrainersListByConuntryId(int CountryId)
        {

            if (CountryId == 0)
            {
                return Ok(new { Status = "false", Message = "Country Not Found" });
            }
            try
            {
                var rnd = new Random();
                var TrainerList = await _context.Trainers.Include(e => e.Country).Where(c => c.CountryId == CountryId).ToListAsync();
                var RandomTrainerList = TrainerList.OrderBy(x => rnd.Next()).Take(1);
                return Ok(new { RandomTrainerList });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = "false", Message = ex.Message });
            }

        }
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] Registration registration)
        {
            try
            {
                var userExists = await _userManager.FindByEmailAsync(registration.Email);
                if (userExists != null)
                    return Ok(new { Status = false, Message = "User already exists!" });
                var user = new ApplicationUser
                {
                    UserName = registration.Email,
                    Email = registration.Email,
                    PhoneNumber = registration.Mobile,
                    CountryId = registration.CountryId,
                    FullName = registration.FullName

                };
                var result = await _userManager.CreateAsync(user, registration.Password);
                if (result.Succeeded)
                {
                    return Ok(new { Status = true, Message = "User created successfully!", user });
                }
                return Ok(new { Status = false, Message = "Something went wrong" });
            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, Message = ex.Message });

            }


        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ResetPasswordModel model)
        {
            try
            {

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return Ok(new { Status = false, Message = "User not found" });

                }
                var Result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!Result.Succeeded)
                {
                    foreach (var error in Result.Errors)
                    {
                        ModelState.TryAddModelError(error.Code, error.Description);
                    }
                    return Ok(new { Status = false, Message = ModelState });

                }

                return Ok(new { Status = true, Message = "Password Changed" });

            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, Message = ex.Message });

            }


        }
        [HttpGet]
        public async Task<IActionResult> GetUserById([FromQuery] string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    return Ok(new { Status = false, Message = "user not found" });
                var Trainer = _context.Trainers.Select(
                   i => new
                   {
                       i.TrainerId,
                       i.FullNameAr,
                       i.FullNameEn,
                       i.Email,
                       i.Mobile,
                       i.Tele,
                       i.Fax,
                       i.GenderId,
                       i.Country.CountryTlAr,
                       i.Country.CountryTlEn,
                       i.CountryId,
                       i.Pic,
                       i.Section.SectionTlAr,
                       i.Section.SectionTlEn,
                       i.SectionId,
                       i.DescriptionAr,
                       i.DescriptionEn,
                       TrainerSubscriptions = i.TrainerSubscriptions.OrderByDescending(e => e.TrainerSubscriptionId).Where(e => e.TrainerId == i.TrainerId).FirstOrDefault(),
                       i.UserId,

                       Courses = i.Courses.Select(j => new
                       {
                           j.CourseId,
                           j.CourseTlAr,
                           j.CourseTlEn,
                           j.TrainerId,
                           j.CourseTargetId,
                           j.CourseTarget.CourseTargetTlAr,
                           j.CourseTarget.CourseTargetTlEn,
                           j.CourseDescAr,
                           j.CourseDescEn,
                           j.IsActive,
                           j.PublishDate,
                           j.Pic,
                           j.Cost,
                           CourseImage = j.CourseImages.ToList(),

                       })

                   }).FirstOrDefault(c => c.UserId == user.Id);


                return Ok(new { Status = true, user, Trainer });


            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, Message = ex.Message });

            }


        }
        [HttpPut]
        public IActionResult EditProfile(EditUserProfileVM editUserProfileVM, IFormFile UserImage)
        {

            try
            {

                var user = _userManager.Users.Where(e => e.Id == editUserProfileVM.UserId).FirstOrDefault();
                if (user == null)
                {
                    return Ok(new { Status = false, Message = "User Not Exist" });
                }

                if (UserImage != null)
                {
                    string folder = "Images/UserImages/";
                    user.Pic = UploadImage(folder, UserImage);
                }
                user.FullName = editUserProfileVM.FullName;
                user.PhoneNumber = editUserProfileVM.Phone;

                _db.Attach(user).State = EntityState.Modified;
                _db.SaveChanges();
                return Ok(new { Status = true, Message = "Updated Successfully" });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = ex.Message });

            }
        }
        [HttpGet]
        public IActionResult GetAllCountries()
        {
            try
            {
                var CountryList = _context.Countries.OrderBy(e => e.CountryOrderIndex).Where(e => e.CountryIsActive == true).Select(i => new
                {

                    CountryId = i.CountryId,
                    CountryPic = i.CountryPic,
                    CountryTlAr = i.CountryTlAr,
                    CountryTlEn = i.CountryTlEn,

                }
                ).ToList();


                return Ok(new { Status = true, CountryList = CountryList });
            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, Message = ex.Message });
            }



        }
        [HttpPost]
        public async Task<IActionResult> ForgetPasswordAsync(string Email)
        {
            try
            {
                if (Email != null)
                {
                    var user = await _userManager.FindByEmailAsync(Email);
                    if (user == null)
                    {

                        return Ok(new { Status = false, Message = "User isn't Exist" });

                    }

                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var webRoot = _hostEnvironment.WebRootPath;

                    var pathToFile = _hostEnvironment.WebRootPath
                           + Path.DirectorySeparatorChar.ToString()
                           + "Templates"
                           + Path.DirectorySeparatorChar.ToString()
                           + "EmailTemplate"
                           + Path.DirectorySeparatorChar.ToString()
                           + "ResetPassword.html";
                    var builder = new BodyBuilder();
                    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                    {

                        builder.HtmlBody = SourceReader.ReadToEnd();

                    }
                    string MessageBody = string.Format(builder.HtmlBody,
                     user.UserName,
                     code,
                       string.Format("{0:dddd, d MMMM yyyy}", DateTime.Now)
                       );
                    await _emailSender.SendEmailAsync(
                        user.Email,
                        "Reset Password",
                       MessageBody);

                    return Ok(new { Status = true, Message = "Please check your email to reset your password." });

                }
                else
                {
                    return Ok(new { Status = false, Message = "Must send Email" });
                }
            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, Message = ex.Message });

            }

        }
        [HttpGet]
        public IActionResult GetPageContentById([FromQuery] int PageContentId)
        {

            try
            {

                var pageContent = _context.PageContents.FirstOrDefault(c => c.PageContentId == PageContentId);
                if (pageContent == null)
                {
                    return Ok(new { Status = false, Message = "Object Not Found" });

                }
                return Ok(new { Status = true, pageContent = pageContent });

            }
            catch (Exception)
            {

                return Ok(new { Status = false, Message = "Something went wrong" });

            }
        }
        [HttpPost]
        public IActionResult AddMessage([FromBody] MessageVM MessageVM)
        {
            try
            {
                var model = new Contact()
                {
                    Email = MessageVM.Email,
                    FullName = MessageVM.FullName,
                    Mobile = MessageVM.Mobile,
                    Msg = MessageVM.Msg,
                    TransDate = DateTime.Now

                };
                _context.Contacts.Add(model);
                _context.SaveChanges();
                return Ok(new { Status = true, Message = "Message Sent Successfully" });

            }
            catch (Exception)
            {

                return Ok(new { Status = false, Message = "Something went wrong" });

            }

        }
        [HttpPost]
        public IActionResult AddPublicDevice([FromBody] PublicDevice model)
        {

            try
            {
                var publicDevice = _context.PublicDevices.FirstOrDefault(c => c.DeviceId == model.DeviceId);
                if (publicDevice != null)
                {
                    publicDevice.CountryId = model.CountryId;
                    publicDevice.IsAndroiodDevice = model.IsAndroiodDevice;

                    _context.PublicDevices.Update(publicDevice);
                    _context.SaveChanges();
                    return Ok(new { Status = true, Message = "deviceId edited" });
                }
                _context.PublicDevices.Add(model);
                _context.SaveChanges();
                return Ok(new { Status = true, Message = "deviceId Added" });

            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = ex });
            }
        }
       
        [HttpPost]
        #region Tranier&&Course
        public async Task<IActionResult> AddTrainer(IFormFile mainImage, IFormFileCollection Medias, ViewModel.TrainerModelVM model)
        {
            try
            {
                var country = _context.Countries.Where(e => e.CountryId == model.CountryId).FirstOrDefault();

                if (country == null)
                {
                    return Ok(new { Status = false, Message = "Please,Enter Valid CountryId" });
                }
                var section = _context.Sections.Where(e => e.SectionId == model.SectionId).FirstOrDefault();

                if (section == null)
                {
                    return Ok(new { Status = false, Message = "Please,Enter Valid sectionId" });
                }
                var plan = _context.TrainerPlans.Where(e => e.TrainerPlanId == model.TrainerPlanId).FirstOrDefault();

                if (plan == null)
                {
                    return Ok(new { Status = false, Message = "Please,Enter Valid TrainerPlanId" });
                }
                var PaymentMethod = _context.PaymentMethods.Where(e => e.PaymentMethodId == model.PaymentMethodId).FirstOrDefault();

                if (PaymentMethod == null)
                {
                    return Ok(new { Status = false, Message = "Please,Enter Valid PaymentMethodId" });
                }
                var trainer = new Trainer()
                {
                    CountryId = model.CountryId,
                    DescriptionAr = model.DescriptionAr,
                    DescriptionEn = model.DescriptionEn,
                    AddedDate = DateTime.Now,
                    Fax = model.Fax,
                    Email = model.Email,
                    GenderId = model.GenderId,
                    Mobile = model.Mobile,
                    SectionId = model.SectionId,
                    Tele = model.Tele,
                    FullNameAr = model.FullNameAr,
                    FullNameEn = model.FullNameEn,
                    UserId = model.UserId,


                };


                if (mainImage != null)
                {
                    string folder = "Images/Trainer/";
                    trainer.Pic = UploadImage(folder, mainImage);
                }
                List<TrainerImage> trainerImagesList = new List<TrainerImage>();
                if (Medias.Count() > 0)
                {
                    for (int i = 0; i < Medias.Count(); i++)
                    {
                        TrainerImage TrainerImageObj = new TrainerImage();
                        if (Medias[i] != null)
                        {
                            string folder = "Images/Trainer/";
                            TrainerImageObj.Pic = UploadImage(folder, Medias[i]);

                        }

                        trainerImagesList.Add(TrainerImageObj);
                    }
                    trainer.TrainerImages = trainerImagesList;
                }
                _context.Trainers.Add(trainer);
                _context.SaveChanges();
                TrainerSubscription trainerSub;

                trainerSub = new TrainerSubscription()
                {
                    TrainerId = trainer.TrainerId,
                    TrainerPlanId = model.TrainerPlanId,
                    Price = plan.Price,
                    PaymentMethodId = model.PaymentMethodId,
                    ispaid = false,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(plan.DurationInMonth.Value)
                };
                _context.TrainerSubscriptions.Add(trainerSub);
                _context.SaveChanges();
                if (model.PaymentMethodId == 2 && trainerSub != null)
                {

                    var requesturl = "https://api.upayments.com/test-payment";
                    var fields = new
                    {
                        merchant_id = "1201",
                        username = "test",
                        password = "test",
                        order_id = trainerSub.TrainerSubscriptionId,
                        total_price = trainerSub.Price,
                        test_mode = 0,
                        CstFName = model.FullNameEn,
                        CstEmail = model.Email,
                        CstMobile = model.Mobile,
                        api_key = "jtest123",
                        success_url = "http://coachkw.net/TrainerSubscriptionSuccessPay",
                        error_url = "http://coachkw.net/TrainerSubscriptionFieldPay",
                        //success_url = "https://localhost:44354/TrainerSubscriptionSuccessPay",
                        //error_url = "https://localhost:44354/TrainerSubscriptionFieldPay"

                    };
                    var content = new StringContent(JsonConvert.SerializeObject(fields), Encoding.UTF8, "application/json");
                    var task = httpClient.PostAsync(requesturl, content);
                    var result = await task.Result.Content.ReadAsStringAsync();
                    var paymenturl = JsonConvert.DeserializeObject<paymenturl>(result);
                    if (paymenturl.status == "success")

                    {
                        return Ok(new { Status = true, paymenturl = paymenturl.paymentURL, trainerId = trainer.TrainerId });

                    }
                    else
                    {
                        return Ok(new { Status = false, Message = paymenturl.error_msg });
                    }
                }
                else
                {
                    return Ok(new { Status = true, paymenturl = "http://coachkw.net/Thankyou", trainerSubId = trainerSub.TrainerId });

                }
            }
            catch (Exception e)
            {
                return Ok(new { Status = false, Message = e.Message });
            }
        }
        [HttpPut]
        public IActionResult EditTrainer(EditTrainerVm model, IFormFile TrainerPic)

        {
            try
            {
                var Trainer = _context.Trainers.Find(model.TrainerId);
                if (Trainer == null)
                {
                    return Ok(new { Status = false, Message = "Trainer not found" });

                }
                var TrainerGender = _context.Genders.Where(e => e.GenderId == model.GenderId).FirstOrDefault();
                if (TrainerGender == null)
                {
                    return Ok(new { Status = false, Message = "Trainer Gender Obj Not Found" });
                }
                var TrainerSection = _context.Sections.Where(e => e.SectionId == model.SectionId).FirstOrDefault();
                if (TrainerSection == null)
                {
                    return Ok(new { Status = false, Message = "Trainer Section Obj Not Found" });
                }
                var TrainerCountryObj = _context.Countries.Where(e => e.CountryId == model.CountryId).FirstOrDefault();
                if (TrainerCountryObj == null)
                {
                    return Ok(new { Status = false, Message = "Trainer CountryObj Not Found" });
                }

                Trainer.DescriptionAr = model.DescriptionAr;
                Trainer.DescriptionEn = model.DescriptionEn;
                Trainer.SectionId = model.SectionId;
                Trainer.GenderId = model.GenderId;
                Trainer.CountryId = model.CountryId;
                Trainer.FullNameAr = model.FullNameAr;
                Trainer.FullNameEn = model.FullNameEn;
                Trainer.Email = model.Email;
                Trainer.Mobile = model.Mobile;
                Trainer.Tele = model.Tele;
                Trainer.Fax = model.Fax;
                string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/Trainer");
                var wwwroot = _hostEnvironment.WebRootPath;
                if (TrainerPic != null)
                {
                    var ImagePath = Path.Combine(wwwroot, "Images/Trainer/" + Trainer.Pic);
                    if (System.IO.File.Exists(ImagePath))
                    {
                        System.IO.File.Delete(ImagePath);
                    }
                    string folder = "Images/Trainer/";
                    Trainer.Pic = UploadImage(folder, TrainerPic);
                }
                var UpdatedTrainer = _context.Trainers.Attach(Trainer);
                UpdatedTrainer.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                return Ok(new { Status = true, Message = "Tranier Edited successfully" });

            }
            catch (Exception e)
            {

                return Ok(new { Status = false, Message = e.Message });

            }
        }
        [HttpDelete]
        public IActionResult DeleteTrainerMedia(int mediaId)
        {
            try

            {
                var TrainerMedia = _context.TrainerImages.Where(e => e.TrainerImageId == mediaId).FirstOrDefault();
                if (TrainerMedia == null)
                {
                    return Ok(new { Status = false, Message = "Trainer Media Obj not Found " });
                }
                _context.TrainerImages.Remove(TrainerMedia);
                _context.SaveChanges();
                return Ok(new { Status = true, Message = "Media Deleted Successfully" });
            }
            catch (Exception e)
            {
                return Ok(new { Status = false, Message = e.Message });
            }
        }

        [HttpPost]
        public IActionResult AddTrainerMedia(int TrainerId, IFormFile Media)
        {
            try
            {
                var Trainer = _context.Trainers.Where(e => e.TrainerId == TrainerId).FirstOrDefault();
                if (Trainer == null)
                {
                    return Ok(new { Status = false, Message = "Trainer not Found " });
                }

                if (Media != null)
                {
                    var TrainerImageObj = new TrainerImage();
                    string folder = "Images/Trainer/";
                    TrainerImageObj.Pic = UploadImage(folder, Media);
                    TrainerImageObj.TrainerId = TrainerId;
                    _context.TrainerImages.Add(TrainerImageObj);
                    _context.SaveChanges();
                    return Ok(new { Status = true, Message = "Media Added Successfully" });
                }
                else
                {
                    return Ok(new { Status = false, Message = "Plz Send Media File" });

                }
            }
            catch (Exception e)
            {

                return Ok(new { Status = false, Message = e.Message });

            }

        }
        [HttpPost]
        public IActionResult AddCourse(CourseVM model, IFormFile coursePic, IFormFileCollection CourseMedias)

        {

            var trainerObj = _context.Trainers.Where(e => e.TrainerId == model.TrainerId).FirstOrDefault();
            if (trainerObj == null)
            {
                return Ok(new { Status = false, Message = "Trainer Not Found" });
            }
            var trainerSubscription = _context.TrainerSubscriptions.Where(e => e.TrainerId == trainerObj.TrainerId).OrderByDescending(e => e.TrainerSubscriptionId).FirstOrDefault();
            if (trainerSubscription == null)
            {
                return Ok(new { Status = false, Message = "You Must Pay the Subscription To Add Course" });
            }
            if (trainerSubscription.ispaid == false)
            {
                return Ok(new { Status = false, Message = "You Must Pay the Subscription To Add Course" });
            }
            if (DateTime.Now > trainerSubscription.EndDate)
            {
                return Ok(new { Status = false, Message = "You Must ReNew of the Subscription First", Show = true });
            }
            var courseTargetObj = _context.CourseTargets.Where(e => e.CourseTargetId == model.CourseTargetId).FirstOrDefault();
            if (courseTargetObj == null)
            {
                return Ok(new { Status = false, Message = "Course Target Obj Not Found" });
            }
            if (model.Cost == 0)
            {
                return Ok(new { Status = false, Message = "Cost Must Be More Than 0" });
            }

            try
            {
                var course = new Course();
                course.CourseDescAr = model.CourseDescAr;
                course.CourseDescEn = model.CourseDescEn;
                course.TrainerId = model.TrainerId;
                course.CourseTargetId = model.CourseTargetId;
                course.CourseTlAr = model.CourseTlAr;
                course.CourseTlEn = model.CourseTlEn;
                course.IsActive = model.IsActive;
                course.PublishDate = model.PublishDate;

                course.Cost = model.Cost;
                if (coursePic != null)
                {
                    string folder = "Images/Course/";
                    course.Pic = UploadImage(folder, coursePic);
                }
                List<CourseImage> courseImagesList = new List<CourseImage>();
                if (CourseMedias.Count() > 0)
                {
                    for (int i = 0; i < CourseMedias.Count(); i++)
                    {
                        CourseImage courseImageObj = new CourseImage();
                        if (CourseMedias[i] != null)
                        {
                            string folder = "Images/Course/";
                            courseImageObj.Pic = UploadImage(folder, CourseMedias[i]);


                        }

                        courseImagesList.Add(courseImageObj);
                    }
                    course.CourseImages = courseImagesList;
                }
                _context.Courses.Add(course);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = ex.Message });
            }



            return Ok(new { Status = true, Message = "Course added successfully" });

        }
        [HttpDelete]
        public IActionResult DeleteCourseMedia(int mediaId)
        {
            try

            {
                var courseMedia = _context.CourseImages.Where(e => e.CourseImageId == mediaId).FirstOrDefault();
                if (courseMedia == null)
                {
                    return Ok(new { Status = false, Message = "Course Media Obj not Found " });
                }


                _context.CourseImages.Remove(courseMedia);
                _context.SaveChanges();
                return Ok(new { Status = true, Message = "Media Deleted Successfully" });

            }
            catch (Exception e)
            {

                return Ok(new { Status = false, Message = e.Message });

            }

        }
        [HttpPost]
        public IActionResult AddCourseMedia(int courseId, IFormFile Media)
        {
            try

            {
                var course = _context.Courses.Where(e => e.CourseId == courseId).FirstOrDefault();
                if (course == null)
                {
                    return Ok(new { Status = false, Message = "Course not Found " });
                }

                if (Media != null)
                {
                    var CourseImageObj = new CourseImage();
                    string folder = "Images/Course/";
                    CourseImageObj.Pic = UploadImage(folder, Media);
                    CourseImageObj.CourseId = courseId;
                    _context.CourseImages.Add(CourseImageObj);
                    _context.SaveChanges();
                    return Ok(new { Status = true, Message = "Media Added Successfully" });

                }
                else
                {
                    return Ok(new { Status = false, Message = "Plz Send Media File" });

                }

            }
            catch (Exception e)
            {

                return Ok(new { Status = false, Message = e.Message });

            }

        }
        [HttpPut]
        public IActionResult EditCourse(EditCourseVm model, IFormFile coursePic)

        {
            try
            {
                var course = _context.Courses.Find(model.CourseId);
                if (course == null)
                {
                    return Ok(new { Status = false, Message = "course not found" });

                }
                var courseTargetObj = _context.CourseTargets.Where(e => e.CourseTargetId == model.CourseTargetId).FirstOrDefault();
                if (courseTargetObj == null)
                {
                    return Ok(new { Status = false, Message = "Course Target Obj Not Found" });
                }
                if (model.Cost == 0)
                {
                    return Ok(new { Status = false, Message = "Cost Must Be More Than 0" });
                }
                course.CourseDescAr = model.CourseDescAr;
                course.CourseDescEn = model.CourseDescEn;
                course.CourseTargetId = model.CourseTargetId;
                course.CourseTlAr = model.CourseTlAr;
                course.CourseTlEn = model.CourseTlEn;
                course.IsActive = model.IsActive;
                course.PublishDate = model.PublishDate;
                course.Cost = model.Cost;

                string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/Course");
                var wwwroot = _hostEnvironment.WebRootPath;
                if (coursePic != null)
                {


                    var ImagePath = Path.Combine(wwwroot, "Images/Course/" + course.Pic);
                    if (System.IO.File.Exists(ImagePath))
                    {
                        System.IO.File.Delete(ImagePath);
                    }
                    string folder = "Images/Course/";
                    course.Pic = UploadImage(folder, coursePic);
                }
                var UpdatedCourse = _context.Courses.Attach(course);
                UpdatedCourse.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                return Ok(new { Status = true, Message = "Course Edit successfully" });

            }
            catch (Exception e)
            {

                return Ok(new { Status = false, Message = e.Message });

            }
        }
        [HttpGet]
        public IActionResult GetAllPaymentMethods()
        {
            try
            {
                var PaymentList = _context.PaymentMethods.Select(i => new
                {
                    i.PaymentMethodId,
                    i.PaymentMethodTlar,
                    i.PaymentMethodTlEn,


                }
                ).ToList();


                return Ok(new { Status = true, PaymentList = PaymentList });
            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, Message = ex.Message });
            }



        }
        [HttpGet]
        public IActionResult GetAllSections()
        {
            try
            {
                var SectionsList = _context.Sections.OrderBy(e => e.SectionOrderIndex).Where(e => e.IsActive == true).Select(i => new
                {
                    SectionId = i.SectionId,
                    SectionTlAr = i.SectionTlAr,
                    SectionTlEn = i.SectionTlEn,
                    SectionPic = i.SectionPic,



                }
                ).ToList();


                return Ok(new { Status = true, SectionsList = SectionsList });
            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, Message = ex.Message });
            }



        }
        [HttpGet]
        public IActionResult GetTrainersBySectionId(int sectionId, int countryId)
        {
            try
            {
                var SectionObj = _context.Sections.Where(e => e.SectionId == sectionId).FirstOrDefault();
                if (SectionObj == null)
                {
                    return Ok(new { Status = false, Message = "Section not Found " });
                }
                var activeTrainerList = _context.Trainers.Where(c => c.CountryId == countryId && c.SectionId == sectionId&&
                 c.TrainerSubscriptions.OrderByDescending(e => e.TrainerSubscriptionId).FirstOrDefault().ispaid && c.TrainerSubscriptions.OrderByDescending(e => e.TrainerSubscriptionId).FirstOrDefault().EndDate >= DateTime.Now
                ).Select(
                  i => new
                  {
                      i.TrainerId,
                      i.FullNameAr,
                      i.FullNameEn,
                      i.Email,
                      i.Mobile,
                      i.Tele,
                      i.Fax,
                      i.GenderId,
                      i.Country.CountryTlAr,
                      i.Country.CountryTlEn,
                      i.CountryId,
                      i.Pic,
                      i.Section.SectionTlAr,
                      i.Section.SectionTlEn,
                      i.SectionId,
                      i.DescriptionAr,
                      i.DescriptionEn,
                      TrainerSubscriptions = i.TrainerSubscriptions.OrderByDescending(e => e.TrainerSubscriptionId).Where(e => e.TrainerId == i.TrainerId).FirstOrDefault(),
                      i.UserId,

                      Courses = i.Courses.Select(j => new
                      {
                          j.CourseId,
                          j.CourseTlAr,
                          j.CourseTlEn,
                          j.TrainerId,
                          j.CourseTargetId,
                          j.CourseTarget.CourseTargetTlAr,
                          j.CourseTarget.CourseTargetTlEn,
                          j.CourseDescAr,
                          j.CourseDescEn,
                          j.IsActive,
                          j.PublishDate,
                          j.Pic,
                          j.Cost,
                          CourseImage = j.CourseImages.ToList(),

                      })

                  }).ToList();
                return Ok(new { Status = true, activeTrainerList = activeTrainerList });

            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, Message = ex.Message });
            }



        }
        [HttpGet]
        public IActionResult GetAllTrainerPlansByCountryId(int countryId)

        {
            try
            {
                var TrainerPlansList = _context.TrainerPlans.Where(e => e.CountryId == countryId && e.IsActive == true).Select(i => new
                {
                    i.TrainerPlanId,
                    i.PlanTlAr,
                    i.PlanTlEn,
                    i.Price,
                    i.CountryId
                }
                ).ToList();


                return Ok(new { Status = true, TrainerPlansList = TrainerPlansList });
            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, Message = ex.Message });
            }



        }
        [HttpGet]
        public IActionResult GetAllTrainersByCountryId(int CountryId)
        {
            try
            {

                var activeTrainerList = _context.Trainers.Where(c => c.CountryId == CountryId &&
                c.TrainerSubscriptions.OrderByDescending(e => e.TrainerSubscriptionId).FirstOrDefault().ispaid && c.TrainerSubscriptions.OrderByDescending(e => e.TrainerSubscriptionId).FirstOrDefault().EndDate >= DateTime.Now
               ).Select(
                   i => new
                   {
                       i.TrainerId,
                       i.FullNameAr,
                       i.FullNameEn,
                       i.Email,
                       i.Mobile,
                       i.Tele,
                       i.Fax,
                       i.GenderId,
                       i.Country.CountryTlAr,
                       i.Country.CountryTlEn,
                       i.CountryId,
                       i.Pic,
                       i.Section.SectionTlAr,
                       i.Section.SectionTlEn,
                       i.SectionId,
                       i.DescriptionAr,
                       i.DescriptionEn,
                       TrainerSubscriptions = i.TrainerSubscriptions.OrderByDescending(e => e.TrainerSubscriptionId).Where(e => e.TrainerId == i.TrainerId).FirstOrDefault(),
                       i.UserId,

                       Courses = i.Courses.Select(j => new
                       {
                           j.CourseId,
                           j.CourseTlAr,
                           j.CourseTlEn,
                           j.TrainerId,
                           j.CourseTargetId,
                           j.CourseTarget.CourseTargetTlAr,
                           j.CourseTarget.CourseTargetTlEn,
                           j.CourseDescAr,
                           j.CourseDescEn,
                           j.IsActive,
                           j.PublishDate,
                           j.Pic,
                           j.Cost,
                           CourseImage = j.CourseImages.ToList(),

                       })

                   }).ToList();
                return Ok(new { Status = true, activeTrainerList = activeTrainerList });
            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, Message = ex.Message });

            }

        }
        [HttpGet]
        public IActionResult GetCousersByTrainerId(int trainerId)
        {
            try
            {
                var trainer = _context.Trainers.Where(e => e.TrainerId == trainerId).FirstOrDefault();
                if (trainer == null)
                {
                    return Ok(new { Status = false, Message = "trainer not found" });

                }

                var coursesList = _context.Courses.Where(e => e.TrainerId == trainerId && e.PublishDate >= DateTime.Now)
               .Select(
                   j => new
                   {
                       j.CourseId,
                       j.CourseTlAr,
                       j.CourseTlEn,
                       j.TrainerId,
                       j.CourseTargetId,
                       j.CourseTarget.CourseTargetTlAr,
                       j.CourseTarget.CourseTargetTlEn,
                       j.CourseDescAr,
                       j.CourseDescEn,
                       j.IsActive,
                       j.PublishDate,
                       j.Pic,
                       j.Cost,
                       CourseImage = j.CourseImages.ToList(),

                   }).ToList();


                return Ok(new { Status = true, coursesList = coursesList });
            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, Message = ex.Message });

            }
        }

        [HttpGet]
        public IActionResult GetAllCousersByTrainerId(int trainerId)
        {
            try
            {
                var trainer = _context.Trainers.Where(e => e.TrainerId == trainerId).FirstOrDefault();
                if (trainer == null)
                {
                    return Ok(new { Status = false, Message = "trainer not found" });

                }

                var coursesList = _context.Courses.Where(e => e.TrainerId == trainerId)
               .Select(
                   j => new
                   {
                       j.CourseId,
                       j.CourseTlAr,
                       j.CourseTlEn,
                       j.TrainerId,
                       j.CourseTargetId,
                       j.CourseTarget.CourseTargetTlAr,
                       j.CourseTarget.CourseTargetTlEn,
                       j.CourseDescAr,
                       j.CourseDescEn,
                       j.IsActive,
                       j.PublishDate,
                       j.Pic,
                       j.Cost,
                       CourseImage = j.CourseImages.ToList(),

                   }).ToList();


                return Ok(new { Status = true, coursesList = coursesList });
            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, Message = ex.Message });

            }
        }
        [HttpGet]
        public IActionResult GetAdzList([FromQuery] int CountryId)
        {
            try
            {

                var AdzList = _context.Adzs.OrderBy(e => e.AdzOrderIndex).Where(e => e.CountryId == CountryId && e.AdzIsActive == true).Select(i => new
                {
                    AdzId = i.AdzId,
                    CountryId = i.CountryId,
                    AdzPic = i.AdzPic,
                    EntityTypeId = i.EntityTypeId,
                    EntityId = i.EntityId,

                }
                ).ToList();
                return Ok(new { Status = true, AdzList = AdzList });
            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, Message = ex.Message });
            }



        }
        #region Public Functions
        [ApiExplorerSettings(IgnoreApi = true)]
        private string UploadImage(string folderPath, IFormFile file)
        {

            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;

            string serverFolder = Path.Combine(_hostEnvironment.WebRootPath, folderPath);

            file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return folderPath;
        }
        #endregion
        [HttpPost]
        public async Task<IActionResult> AddTrainerSubscription([FromBody] TrainerSubscriptionVM trainerSubscriptionVM)
        {
            try
            {
                var trainer = _context.Trainers.Where(e => e.TrainerId == trainerSubscriptionVM.TrainerId).FirstOrDefault();
                if (trainer == null)
                {
                    return Ok(new { Status = false, message = "Trainer Not Found" });
                }

                var plan = _context.TrainerPlans.Find(trainerSubscriptionVM.TrainerPlanId);
                if (plan == null)
                {
                    return Ok(new { Status = false, message = "Plan Not Found" });
                }
                var LastTrainerSubscription = _context.TrainerSubscriptions.Where(e => e.TrainerId == trainer.TrainerId).OrderByDescending(e => e.TrainerSubscriptionId).FirstOrDefault();

                if (LastTrainerSubscription != null)
                {
                    if (LastTrainerSubscription.ispaid == false && LastTrainerSubscription.PaymentMethodId == 2)
                    {
                        _context.TrainerSubscriptions.Remove(LastTrainerSubscription);
                    }
                    else if (LastTrainerSubscription.EndDate > DateTime.Now && LastTrainerSubscription.ispaid == true)
                    {
                        return Ok(new { Status = false, Message = "Trainer Aleardy Subscriped In Plan" });

                    }

                }

                var trainerSubscription = new TrainerSubscription()
                {
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(plan.DurationInMonth.Value),
                    Price = plan.Price,
                    TrainerId = trainerSubscriptionVM.TrainerId,
                    PaymentMethodId = trainerSubscriptionVM.PaymentMethodId,
                    TrainerPlanId = trainerSubscriptionVM.TrainerPlanId
                };
                _context.TrainerSubscriptions.Add(trainerSubscription);
                await _context.SaveChangesAsync();
                if (trainerSubscriptionVM.PaymentMethodId == 2 && trainerSubscription != null)
                {

                    var requesturl = "https://api.upayments.com/test-payment";
                    var fields = new
                    {
                        merchant_id = "1201",
                        username = "test",
                        password = "test",
                        order_id = trainerSubscription.TrainerSubscriptionId,
                        total_price = trainerSubscription.Price,
                        test_mode = 0,
                        CstFName = trainer.FullNameEn,
                        CstEmail = trainer.Email,
                        CstMobile = trainer.Mobile,
                        api_key = "jtest123",
                        success_url = "http://coachkw.net/TrainerSubscriptionSuccessPay",
                        error_url = "http://coachkw.net/TrainerSubscriptionFieldPay",

                    };
                    var content = new StringContent(JsonConvert.SerializeObject(fields), Encoding.UTF8, "application/json");
                    var task = httpClient.PostAsync(requesturl, content);
                    var result = await task.Result.Content.ReadAsStringAsync();
                    var paymenturl = JsonConvert.DeserializeObject<paymenturl>(result);
                    if (paymenturl.status == "success")

                    {
                        return Ok(new { Status = true, paymenturl = paymenturl.paymentURL, TrainerId = trainer.TrainerId, TrainerSubscriptionId = trainerSubscription.TrainerSubscriptionId });
                    }
                    else
                    {
                        return Ok(new { Status = false, Message = paymenturl.error_msg });
                    }
                }

                else
                {

                    return Ok(new { Status = true, paymenturl = "http://coachkw.net/Thankyou", TrainerId = trainer.TrainerId });


                }
            }

            catch (Exception e)
            {
                return Ok(new { Status = false, Message = e.Message });
            }


        }
        [HttpPost]
        public async Task<IActionResult> SubscribeToCourse([FromBody] SubscriptionModelVm model)
        {
            try
            {

                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    return Ok(new { Status = false, Message = "User Not Found" });
                }
                var course = _context.Courses.Where(e => e.CourseId == model.CourseId).FirstOrDefault();

                if (course == null)
                {
                    return Ok(new { Status = false, Message = "course Not Found" });
                }
                var PaymentMethod = _context.PaymentMethods.Where(e => e.PaymentMethodId == model.PaymentMethodId).FirstOrDefault();

                if (PaymentMethod == null)
                {
                    return Ok(new { Status = false, Message = "Payment Method Not Found" });
                }
                if (_context.Subscriptions.Any(e => e.EntityId == model.CourseId && e.UserId == model.UserId && e.ispaid == true))
                {
                    return Ok(new { Status = false, Message = "User Is Already Subscriped" });

                }
                var subscriptionObj = new Subscription()
                {
                    Cost = course.Cost,
                    EntityId = course.CourseId,
                    EntityName = "Course",
                    UserId = model.UserId,
                    PaymentMethodId = model.PaymentMethodId,
                    SubDate = DateTime.Now,
                    ispaid = false,
                    EntityTypeId = 4
                };

                _context.Subscriptions.Add(subscriptionObj);
                _context.SaveChanges();

                if (model.PaymentMethodId == 2 && subscriptionObj != null)
                {

                    var requesturl = "https://api.upayments.com/test-payment";
                    var fields = new
                    {
                        merchant_id = "1201",
                        username = "test",
                        password = "test",
                        order_id = subscriptionObj.SubscriptionId,
                        total_price = subscriptionObj.Cost,
                        test_mode = 0,
                        CstFName = user.FullName,
                        CstEmail = user.Email,
                        CstMobile = user.PhoneNumber,
                        api_key = "jtest123",
                        success_url = "http://coachkw.net/SubscriptionPaySuccess",
                        error_url = "http://coachkw.net/SubscriptionPayFaield",


                    };
                    var content = new StringContent(JsonConvert.SerializeObject(fields), Encoding.UTF8, "application/json");
                    var task = httpClient.PostAsync(requesturl, content);
                    var result = await task.Result.Content.ReadAsStringAsync();
                    var paymenturl = JsonConvert.DeserializeObject<paymenturl>(result);
                    if (paymenturl.status == "success")

                    {
                        return Ok(new { Status = true, paymenturl = paymenturl.paymentURL });

                    }
                    else
                    {
                        return Ok(new { Status = false, Message = paymenturl.error_msg });
                    }
                }
                else
                {

                    return Ok(new { Status = true, paymenturl = "http://coachkw.net/Thankyou" });

                }
            }
            catch (Exception e)
            {
                return Ok(new { Status = false, Message = e.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetUserSubscribedCourses([FromQuery] string UserId)
        {
            try
            {

                var CoursesList = await _context.Subscriptions.Where(c => c.UserId == UserId && c.EntityTypeId == 4 &&c.ispaid == true).Select(e => e.EntityId).ToListAsync();
                List<Course> courses = new List<Course>();
                if (CoursesList.Count != 0)
                {

                    foreach (var item in CoursesList)
                    {
                        var course = _context.Courses.Include(e=>e.CourseImages).Include(e=>e.Trainer).Where(e => e.CourseId == item).FirstOrDefault();
                        if (course != null)
                        {
                            courses.Add(course);
                        }

                    }
                }
                return Ok(new { Status = true, courses = courses });

            }
            catch (Exception)
            {
                return Ok(new { Status = false, message = "Something went wrong" });

            }

        }
        [HttpGet]
        public IActionResult GetAllGender()
        {
            try
            {
                var GenderList = _context.Genders.Select(i => new
                {
                    i.GenderId,
                    i.GenderTlAr,
                    i.GenderTlEn,

                }
                ).ToList();


                return Ok(new { Status = true, GenderList = GenderList });
            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, Message = ex.Message });
            }

        }
        #endregion
        #region Camp
        [HttpGet]
        public IActionResult GetAllCampTarget()
        {
            try
            {
                var CampTargetList = _context.CampTargets.Where(e => e.IsActive == true).Select(i => new
                {
                    i.CampTargetId,
                    i.CampTargetTlEn,
                    i.CampTargetTlAr,
                }
                ).ToList();


                return Ok(new { Status = true, CampTargetList = CampTargetList });
            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, Message = ex.Message });
            }



        }
        [HttpGet]
        public IActionResult GetAllCampTypes()
        {
            try
            {
                var CampTypeList = _context.CampTypes.Where(e => e.IsActive == true).Select(i => new
                {
                    i.CampTypeId,
                    i.CampTypeTlAr,
                    i.CampTypeTlEn,
                }
                ).ToList();


                return Ok(new { Status = true, CampTypeList = CampTypeList });
            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, Message = ex.Message });
            }



        }
        [HttpGet]
        public IActionResult GetAllCampsPlansByCountryId(int countryId)

        {
            try
            {
                var CampPlansList = _context.CampPlans.Where(e => e.CountryId == countryId && e.IsActive == true).Select(i => new
                {
                    i.CampPlanId,
                    i.PlanTlAr,
                    i.PlanTlEn,
                    i.Price,
                    i.CountryId
                }
                ).ToList();


                return Ok(new { Status = true, CampPlansList = CampPlansList });
            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, Message = ex.Message });
            }



        }
        [HttpPost]
        public async Task<IActionResult> AddCamp(IFormFile mainImage, IFormFileCollection Medias, [FromForm] CampVm campVm)
        {

            var user = await _userManager.FindByIdAsync(campVm.UserId);
            if (user == null)
            {
                return Ok(new { Status = false, message = "Please,Enter Valid UserId" });
            }
            var campType = _context.CampTypes.Where(e => e.CampTypeId == campVm.CampTypeId).FirstOrDefault();
            if (campType == null)
            {
                return Ok(new { Status = false, message = "Please,Enter Valid Camp Type Id" });
            }
            var campTarget = _context.CampTargets.Where(e => e.CampTargetId == campVm.CampTargetId).FirstOrDefault();

            if (campTarget == null)
            {
                return Ok(new { Status = false, message = "Please,Enter Valid CampTargetId" });
            }
            var country = _context.Countries.Where(e => e.CountryId == campVm.CountryId).FirstOrDefault();

            if (country == null)
            {
                return Ok(new { Status = false, message = "Please,Enter Valid CountryId" });
            }
            var campPlan = _context.CampPlans.Where(e => e.CampPlanId == campVm.CampPlanId).FirstOrDefault();

            if (campPlan == null)
            {
                return Ok(new { Status = false, message = "Please,Enter Valid CampPlanId" });
            }


            var campObj = new Camp()
            {
                CampTlAr = campVm.CampTlAr,
                CampTlEn = campVm.CampTlEn,
                StartDate = campVm.StartDate,
                EndDate = campVm.EndDate,
                JoinStart = campVm.JoinStart,
                JoinEnd = campVm.JoinEnd,
                SubPrice = campPlan.Price,
                Cost = campVm.Cost,
                ispaid = false,
                UserId = campVm.UserId,
                CampDescEn = campVm.CampDescEn,
                CampDescAr = campVm.CampDescAr,
                CampTargetId = campVm.CampTargetId,
                PaymentMethodId = campVm.PaymentMethodId,
                CampPlanId = campVm.CampPlanId,
                CampTypeId = campVm.CampTypeId,
                CountryId = campVm.CountryId,
                IsActive = campVm.IsActive,
                PlanStartDate = DateTime.Now,
                PlanEndDate = DateTime.Now.AddMonths(campPlan.DurationInMonth.Value),

            };
            if (mainImage != null)
            {
                string folder = "Images/Camp/";
                campObj.Pic = UploadImage(folder, mainImage);
            }
            List<CampImage> campImagesList = new List<CampImage>();
            if (Medias.Count() > 0)
            {
                for (int i = 0; i < Medias.Count(); i++)
                {
                    CampImage campImageObj = new CampImage();
                    if (Medias[i] != null)
                    {
                        string folder = "Images/Camp/";
                        campImageObj.Pic = UploadImage(folder, Medias[i]);

                    }

                    campImagesList.Add(campImageObj);
                }
                campObj.CampImages = campImagesList;
            }

            try
            {
                _context.Camps.Add(campObj);
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                return Ok(new { Status = false, Message = e.Message });
            }
            if (campObj.PaymentMethodId == 1)
            {
                return Ok(new { Status = true, paymenturl = "http://coachkw.net/Thankyou", campId = campObj.CampId });

            }
            if (campObj.PaymentMethodId == 2)
            {

                var requesturl = "https://api.upayments.com/test-payment";
                var fields = new
                {
                    merchant_id = "1201",
                    username = "test",
                    password = "test",
                    order_id = campObj.CampId,
                    total_price = campObj.SubPrice,
                    test_mode = 0,
                    CstFName = user.FullName,
                    CstEmail = user.Email,
                    CstMobile = user.PhoneNumber,
                    api_key = "jtest123",
                    success_url = "http://coachkw.net/success",
                    error_url = "http://coachkw.net/failed"
                    //success_url = "https://localhost:44354/success",
                    //error_url = "https://localhost:44354/failed"

                };
                var content = new StringContent(JsonConvert.SerializeObject(fields), Encoding.UTF8, "application/json");
                var task = httpClient.PostAsync(requesturl, content);
                var result = await task.Result.Content.ReadAsStringAsync();
                var paymenturl = JsonConvert.DeserializeObject<paymenturl>(result);
                if (paymenturl.status == "success")

                {
                    return Ok(new { Staust = true, paymenturl = paymenturl.paymentURL, campId = campObj.CampId });
                }
                else
                {
                    return Ok(new { Staus = false, Message = paymenturl.error_msg });
                }


            }
            return Ok(new { Staus = false, Message = "Something Error" });

        }
        [HttpPost]
        public IActionResult AddCampMedia(int campId, IFormFile Media)
        {
            try

            {
                var camp = _context.Camps.Where(e => e.CampId == campId).FirstOrDefault();
                if (camp == null)
                {
                    return Ok(new { Status = false, Message = "Camp not Found " });
                }

                if (Media != null)
                {
                    var CampImageObj = new CampImage();
                    string folder = "Images/Camp/";
                    CampImageObj.Pic = UploadImage(folder, Media);
                    CampImageObj.CampId = campId;
                    _context.CampImages.Add(CampImageObj);
                    _context.SaveChanges();
                    return Ok(new { Status = true, Message = "Media Added Successfully" });

                }
                else
                {
                    return Ok(new { Status = false, Message = "Plz Send Media File" });

                }

            }
            catch (Exception e)
            {

                return Ok(new { Status = false, Message = e.Message });

            }

        }
        [HttpDelete]
        public IActionResult DeleteCampMedia(int mediaId)
        {
            try

            {
                var campMedia = _context.CampImages.Where(e => e.CampImageId == mediaId).FirstOrDefault();
                if (campMedia == null)
                {
                    return Ok(new { Status = false, Message = "Camp Media Obj not Found " });
                }


                _context.CampImages.Remove(campMedia);
                _context.SaveChanges();
                return Ok(new { Status = true, Message = "Media Deleted Successfully" });

            }
            catch (Exception e)
            {

                return Ok(new { Status = false, Message = e.Message });

            }

        }
        [HttpGet]
        public IActionResult GetAllCourseTarget()
        {
            try
            {
                var CourseTarget = _context.CourseTargets.Select(i => new
                {
                    i.CourseTargetId,
                    i.CourseTargetTlEn,
                    i.CourseTargetTlAr,
                }
                ).ToList();


                return Ok(new { Status = true, CourseTarget = CourseTarget });
            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, Message = ex.Message });
            }

        }
        [HttpPut]
        public IActionResult EditCamp(EditCampVm model, IFormFile CampPic)

        {
            try
            {
                var Camp = _context.Camps.Find(model.CampId);
                if (Camp == null)
                {
                    return Ok(new { Status = false, Message = "Camp not found" });

                }
                var CampTargetObj = _context.CampTargets.Where(e => e.CampTargetId == model.CampTargetId).FirstOrDefault();
                if (CampTargetObj == null)
                {
                    return Ok(new { Status = false, Message = "Camp Target Obj Not Found" });
                }
                var CampTypeObj = _context.CampTypes.Where(e => e.CampTypeId == model.CampTypeId).FirstOrDefault();
                if (CampTypeObj == null)
                {
                    return Ok(new { Status = false, Message = "Camp Type Obj Not Found" });
                }
                var CampCountryObj = _context.Countries.Where(e => e.CountryId == model.CountryId).FirstOrDefault();
                if (CampCountryObj == null)
                {
                    return Ok(new { Status = false, Message = "Camp Country Obj Not Found" });
                }

                if (model.Cost == 0)
                {
                    return Ok(new { Status = false, Message = "Cost Must Be More Than 0" });
                }

                Camp.CampDescAr = model.CampDescAr;
                Camp.CampDescEn = model.CampDescEn;
                Camp.CampTargetId = model.CampTargetId;
                Camp.CampTypeId = model.CampTypeId;
                Camp.CountryId = model.CountryId;
                Camp.CampTlAr = model.CampTlAr;
                Camp.CampTlEn = model.CampTlEn;
                Camp.IsActive = model.IsActive;
                Camp.StartDate = model.StartDate;
                Camp.EndDate = model.EndDate;
                Camp.JoinStart = model.JoinStart;
                Camp.JoinEnd = model.JoinEnd;
                Camp.Cost = model.Cost;

                string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/Camp");
                var wwwroot = _hostEnvironment.WebRootPath;
                if (CampPic != null)
                {
                    var ImagePath = Path.Combine(wwwroot, "Images/Camp/" + Camp.Pic);
                    if (System.IO.File.Exists(ImagePath))
                    {
                        System.IO.File.Delete(ImagePath);
                    }
                    string folder = "Images/Camp/";
                    Camp.Pic = UploadImage(folder, CampPic);
                }
                var UpdatedCamp = _context.Camps.Attach(Camp);
                UpdatedCamp.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                return Ok(new { Status = true, Message = "Camp Edited successfully" });

            }
            catch (Exception e)
            {

                return Ok(new { Status = false, Message = e.Message });

            }
        }
        [HttpGet]
        public IActionResult GetAllCampPlansByCountryId(int countryId)

        {
            try
            {
                var CampPlansList = _context.CampPlans.Where(e => e.CountryId == countryId && e.IsActive == true).Select(i => new
                {
                    i.CampPlanId,
                    i.PlanTlAr,
                    i.PlanTlEn,
                    i.Price,
                    i.CountryId
                }
                ).ToList();


                return Ok(new { Status = true, CampPlansList = CampPlansList });
            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, Message = ex.Message });
            }



        }
        [HttpPost]
        public async Task<IActionResult> SubscribeToCamp([FromBody] SubscriptionCampModelVm model)
        {
            try
            {

                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    return Ok(new { Status = false, Message = "User Not Found" });
                }
                var camp = _context.Camps.Where(e => e.CampId == model.CampId).FirstOrDefault();

                if (camp == null)
                {
                    return Ok(new { Status = false, Message = "camp Not Found" });
                }
                var PaymentMethod = _context.PaymentMethods.Where(e => e.PaymentMethodId == model.PaymentMethodId).FirstOrDefault();

                if (PaymentMethod == null)
                {
                    return Ok(new { Status = false, Message = "Payment Method Not Found" });
                }
                if (_context.Subscriptions.Any(e => e.EntityId == model.CampId && e.UserId == model.UserId&&e.ispaid==true))
                {
                    return Ok(new { Status = false, Message = "User Is Already Subscriped" });
                }
                var subscriptionObj = new Subscription()
                {
                    Cost = camp.Cost,
                    EntityId = camp.CampId,
                    EntityName = "Camp",
                    UserId = model.UserId,
                    PaymentMethodId = model.PaymentMethodId,
                    SubDate = DateTime.Now,
                    ispaid = false,
                    EntityTypeId = 2
                };
                _context.Subscriptions.Add(subscriptionObj);
                _context.SaveChanges();

                if (model.PaymentMethodId == 2 && subscriptionObj != null)
                {

                    var requesturl = "https://api.upayments.com/test-payment";
                    var fields = new
                    {
                        merchant_id = "1201",
                        username = "test",
                        password = "test",
                        order_id = subscriptionObj.SubscriptionId,
                        total_price = subscriptionObj.Cost,
                        test_mode = 0,
                        CstFName = user.FullName,
                        CstEmail = user.Email,
                        CstMobile = user.PhoneNumber,
                        api_key = "jtest123",
                        success_url = "http://coachkw.net/SubscriptionCampPaySuccess",
                        error_url = "http://coachkw.net/SubscriptionCampPayFailed",
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(fields), Encoding.UTF8, "application/json");
                    var task = httpClient.PostAsync(requesturl, content);
                    var result = await task.Result.Content.ReadAsStringAsync();
                    var paymenturl = JsonConvert.DeserializeObject<paymenturl>(result);
                    if (paymenturl.status == "success")

                    {
                        return Ok(new { Status = true, paymenturl = paymenturl.paymentURL });

                    }
                    else
                    {
                        return Ok(new { Status = false, Message = paymenturl.error_msg });
                    }
                }
                else
                {

                    return Ok(new { Status = true, paymenturl = "http://coachkw.net/Thankyou" });

                }
            }
            catch (Exception e)
            {
                return Ok(new { Status = false, Message = e.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUserSubscribedCamp([FromQuery] string UserId)
        {
            try
            {

                var CampList = await _context.Subscriptions.Where(c => c.UserId == UserId && c.EntityTypeId == 2 && c.ispaid == true).Select(e => e.EntityId).ToListAsync();
                List<Camp> camps = new List<Camp>();
                if (CampList.Count != 0)
                {

                    foreach (var item in CampList)
                    {
                        var camp = _context.Camps.Include(e=>e.CampImages).Include(e => e.Country).Include(e => e.CampTarget).Include(e => e.CampType).Include(e => e.CampPlan).Where(e => e.CampId == item).FirstOrDefault();
                        if (camp != null)
                        {
                            camps.Add(camp);
                        }

                    }
                }
                return Ok(new { Status = true, camps = camps });
            }
            catch (Exception)
            {
                return Ok(new { Status = false, message = "Something went wrong" });
            }

        }
        [HttpGet]
        public IActionResult GetCampsbyUserId([FromQuery] string UserId)
        {
            try
            {
                var CampList = _context.Camps.Include(e => e.CampPlan).Include(e => e.CampTarget).Include(e => e.Country).Where(c => c.UserId == UserId && c.ispaid == true).Select(
                   j => new
                   {
                       j.CampId,
                       j.CampTlAr,
                       j.CampTlEn,
                       j.UserId,
                       j.CampTargetId,
                       j.CampTarget.CampTargetTlAr,
                       j.CampTarget.CampTargetTlEn,
                       j.CampPlanId,
                       j.CampPlan.PlanTlAr,
                       j.CampPlan.PlanTlEn,
                       j.CountryId,
                       j.Country.CountryTlAr,
                       j.Country.CountryTlEn,
                       j.CampTypeId,
                       j.CampType.CampTypeTlAr,
                       j.CampType.CampTypeTlEn,
                       j.CampDescAr,
                       j.CampDescEn,
                       j.IsActive,
                       j.StartDate,
                       j.EndDate,
                       j.JoinStart,
                       j.JoinEnd,
                       j.Pic,
                       j.Cost,
                       CampsImages = j.CampImages.ToList(),

                   }).ToList();

                return Ok(new { Status = true, CampList = CampList });
            }
            catch (Exception)
            {
                return Ok(new { Status = false, Message = "Something went wrong" });
            }

        }
        [HttpGet]
        public IActionResult GetCampListByCountry([FromQuery] int CountryId)
        {
            try
            {
                var CampList = _context.Camps.Include(e => e.CampPlan).Include(e => e.CampTarget).Include(e => e.Country).Where(c => c.CountryId == CountryId && c.PlanEndDate >= DateTime.Now && c.IsActive == true && c.ispaid == true).Select(
                   j => new
                   {
                       j.CampId,
                       j.CampTlAr,
                       j.CampTlEn,
                       j.UserId,
                       j.CampTargetId,
                       j.CampTarget.CampTargetTlAr,
                       j.CampTarget.CampTargetTlEn,
                       j.CampPlanId,
                       j.CampPlan.PlanTlAr,
                       j.CampPlan.PlanTlEn,
                       j.CountryId,
                       j.Country.CountryTlAr,
                       j.Country.CountryTlEn,
                       j.CampTypeId,
                       j.CampType.CampTypeTlAr,
                       j.CampType.CampTypeTlEn,
                       j.CampDescAr,
                       j.CampDescEn,
                       j.IsActive,
                       j.StartDate,
                       j.EndDate,
                       j.Pic,
                       j.Cost,
                       CampsImages = j.CampImages.ToList(),

                   }).ToList();

                return Ok(new { Status = true, CampList });

            }
            catch (Exception)
            {

                return Ok(new { Status = false, Message = "Something went wrong" });

            }
        }
        #endregion
        [HttpDelete]
        public IActionResult DeleteUserAccount(string userId)
        {
            var user = _db.Users.Where(e => e.Id == userId).FirstOrDefault();
            try
            {
                if (user == null)
                {
                    return Ok(new { Status = false, Message = "user Not Found" });
                }

                _db.Users.Remove(user);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                return Ok(new { Status = false, Message = e.Message });

            }
            return Ok(new { Status = true, Message = "user Account Deleted Successfully" });
        }
        [HttpGet]
        public IActionResult GetBannerList()
        {
            try
            {
                var bannerList =  _context.Banners.OrderBy(e => e.BannerOrderIndex).Where(c => c.BannerIsActive == true).Select(i => new
                {
                    BannerId = i.BannerId,
                    BannerPic = i.BannerPic,
                    EntityTypeId = i.EntityTypeId,
                    EntityId = i.EntityId,
                   
                }
                ).ToList();
                return Ok(new { bannerList });
            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, Message = ex.Message });
            }
        }

        #region Tournment
        [HttpGet]
        public IActionResult GetAllTournmentTarget()
        {
            try
            {
                var TournmentTargetList = _context.TournamentTargets.Where(e => e.IsActive == true).Select(i => new
                {
                    i.TournamentTargetId,
                    i.TournamentTargetTlAr,
                    i.TournamentTargetTlEn,
                }
                ).ToList();


                return Ok(new { Status = true, TournmentTargetList = TournmentTargetList });
            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, Message = ex.Message });
            }



        }
        [HttpGet]
        public IActionResult GetAllTournmentTypes()
        {
            try
            {
                var TournmentTypesList = _context.TournamentTypes.Where(e => e.IsActive == true).Select(i => new
                {
                    i.TournamentTypeId,
                    i.TournamentTypeTlAr,
                    i.TournamentTypeTlEn,
                }
                ).ToList();


                return Ok(new { Status = true, TournmentTypesList = TournmentTypesList });
            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, Message = ex.Message });
            }



        }
        [HttpGet]
        public IActionResult GetAllTournmentsPlansByCountryId(int countryId)

        {
            try
            {
                var TournmentPlansList = _context.TournamentPlans.Where(e => e.CountryId == countryId && e.IsActive == true).Select(i => new
                {
                    i.TournamentPlanId,
                    i.PlanTlAr,
                    i.PlanTlEn,
                    i.Price,
                    i.CountryId
                }
                ).ToList();


                return Ok(new { Status = true, TournmentPlansList = TournmentPlansList });
            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, Message = ex.Message });
            }



        }
        [HttpPost]
        public async Task<IActionResult> AddTournment(IFormFile mainImage, IFormFileCollection Medias, [FromForm] TournmentModelVm tournmentModelVm)
        {
            if (tournmentModelVm.Cost == 0)
            {
                return Ok(new { Status = false, Message = "Cost Must Be More Than 0" });
            }
            var user = await _userManager.FindByIdAsync(tournmentModelVm.UserId);
            if (user == null)
            {
                return Ok(new { status = false, message = "Please,Enter Valid UserId" });
            }
            var TournmentType = _context.TournamentTypes.Where(e => e.TournamentTypeId == tournmentModelVm.TournamentTypeId).FirstOrDefault();
            if (TournmentType == null)
            {
                return Ok(new { status = false, message = "Please,Enter Valid TournmentTypeId" });
            }
            var TournmentTarget = _context.TournamentTargets.Where(e => e.TournamentTargetId == tournmentModelVm.TournamentTargetId).FirstOrDefault();

            if (TournmentTarget == null)
            {
                return Ok(new { status = false, message = "Please,Enter Valid TournamentTargetId" });
            }
            var country = _context.Countries.Where(e => e.CountryId == tournmentModelVm.CountryId).FirstOrDefault();

            if (country == null)
            {
                return Ok(new { status = false, message = "Please,Enter Valid CountryId" });
            }
            var TournmentPlan = _context.TournamentPlans.Where(e => e.TournamentPlanId == tournmentModelVm.TournamentPlanId).FirstOrDefault();

            if (TournmentPlan == null)
            {
                return Ok(new { status = false, message = "Please,Enter Valid TournmentPlanId" });
            }
            var PaymentMethod = _context.PaymentMethods.Where(e => e.PaymentMethodId == tournmentModelVm.PaymentMethodId).FirstOrDefault();

            if (PaymentMethod == null)
            {
                return Ok(new { status = false, message = "Please,Enter Valid PaymentMethodId" });
            }
         
            var tournmentObj = new Tournament()
            {
                StartDate = tournmentModelVm.StartDate,
                EndDate = tournmentModelVm.EndDate,
                SubStartDate = tournmentModelVm.JoinStart,
                SubEndDate = tournmentModelVm.JoinEnd,
                PlanStartDate = DateTime.Now,
                PlanEndDate = DateTime.Now.AddMonths(TournmentPlan.DurationInMonth.Value),
                SubPrice = TournmentPlan.Price,
                Cost = tournmentModelVm.Cost,
                ispaid = false,
                UserId = tournmentModelVm.UserId,
                TournamentDescEn = tournmentModelVm.TournamentDescEn,
                TournamentDescAr = tournmentModelVm.TournamentDescAr,
                TournamentTlAr = tournmentModelVm.TournamentTlAr,
                TournamentTlEn = tournmentModelVm.TournamentTlEn,
                TournamentTargetId = tournmentModelVm.TournamentTargetId,
                PaymentMethodId = tournmentModelVm.PaymentMethodId,
                TournamentPlanId = tournmentModelVm.TournamentPlanId,
                TournamentTypeId = tournmentModelVm.TournamentTypeId,
                CountryId = tournmentModelVm.CountryId,
                IsActive = tournmentModelVm.IsActive
            };
            if (mainImage != null)
            {
                string folder = "Images/Tournament/";
                tournmentObj.Pic = UploadImage(folder, mainImage);
            }
            List<TournamentImage> TournmentImagesList = new List<TournamentImage>();
            if (Medias.Count() > 0)
            {
                for (int i = 0; i < Medias.Count(); i++)
                {
                    TournamentImage TournmentImageObj = new TournamentImage();
                    if (Medias[i] != null)
                    {
                        string folder = "Images/Tournament/";
                        TournmentImageObj.Pic = UploadImage(folder, Medias[i]);

                    }

                    TournmentImagesList.Add(TournmentImageObj);
                }
                tournmentObj.TournamentImages = TournmentImagesList;
            }

            try
            {
                _context.Tournaments.Add(tournmentObj);
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                return Ok(new { Staus = "false", reason = e.Message });
            }
            if (tournmentObj.PaymentMethodId == 1)
            {
                return Ok(new { status = "true", paymenturl = "http://coachkw.net/Thankyou" });

            }
            if (tournmentObj.PaymentMethodId == 2)
            {

                var requesturl = "https://api.upayments.com/test-payment";
                var fields = new
                {
                    merchant_id = "1201",
                    username = "test",
                    password = "test",
                    order_id = tournmentObj.TournamentId,
                    total_price = tournmentObj.SubPrice,
                    test_mode = 0,
                    CstFName = user.FullName,
                    CstEmail = user.Email,
                    CstMobile = user.PhoneNumber,
                    api_key = "jtest123",
                    success_url = "http://coachkw.net/successTournmentPayment",
                    error_url = "http://coachkw.net/feildTournmentPayment"
                    //success_url = "https://localhost:44354/successTournmentPayment",
                    //error_url = "https://localhost:44354/feildTournmentPayment"

                };
                var content = new StringContent(JsonConvert.SerializeObject(fields), Encoding.UTF8, "application/json");
                var task = httpClient.PostAsync(requesturl, content);
                var result = await task.Result.Content.ReadAsStringAsync();
                var paymenturl = JsonConvert.DeserializeObject<paymenturl>(result);
                if (paymenturl.status == "success")

                {
                    return Ok(new { Status = true, paymenturl = paymenturl.paymentURL, TournamentId = tournmentObj.TournamentId });
                }
                else
                {
                    return Ok(new { Status = false, Message = paymenturl.error_msg });
                }


            }
            return Ok(new { Status = false, Message = "Something Error" });

        }
        [HttpPost]
        public IActionResult AddTournmentMedia(int tournamentId, IFormFile Media)
        {
            try

            {
                var tournment = _context.Tournaments.Where(e => e.TournamentId == tournamentId).FirstOrDefault();
                if (tournment == null)
                {
                    return Ok(new { Status = false, Message = "Tournment not Found " });
                }

                if (Media != null)
                {
                    var tournmentImageObj = new TournamentImage();
                    string folder = "Images/Tournament/";
                    tournmentImageObj.Pic = UploadImage(folder, Media);
                    tournmentImageObj.TournamentId = tournamentId;
                    _context.TournamentImages.Add(tournmentImageObj);
                    _context.SaveChanges();
                    return Ok(new { Status = true, Message = "Media Added Successfully" });

                }
                else
                {
                    return Ok(new { Status = false, Message = "Plz Send Media File" });

                }

            }
            catch (Exception e)
            {

                return Ok(new { Status = false, Message = e.Message });

            }

        }
        [HttpDelete]
        public IActionResult DeleteTournmentMedia(int mediaId)
        {
            try

            {
                var tournmentMedia = _context.TournamentImages.Where(e => e.TournamentImageId == mediaId).FirstOrDefault();
                if (tournmentMedia == null)
                {
                    return Ok(new { Status = false, Message = "Tournment Media Obj not Found " });
                }


                _context.TournamentImages.Remove(tournmentMedia);
                _context.SaveChanges();
                return Ok(new { Status = true, Message = "Media Deleted Successfully" });

            }
            catch (Exception e)
            {

                return Ok(new { Status = false, Message = e.Message });

            }

        }
        [HttpPut]
        public IActionResult EditTournment(EditTournmentVm model, IFormFile TournmentPic)

        {
            try
            {
                var Tournment = _context.Tournaments.Find(model.TournmentId);
                if (Tournment == null)
                {
                    return Ok(new { Status = false, Message = "Tournment not found" });

                }
                var TournmentTargetObj = _context.TournamentTargets.Where(e => e.TournamentTargetId == model.TournamentTargetId).FirstOrDefault();
                if (TournmentTargetObj == null)
                {
                    return Ok(new { Status = false, Message = "Tournment Target Obj Not Found" });
                }
                var TournmentTypeObj = _context.TournamentTypes.Where(e => e.TournamentTypeId == model.TournamentTypeId).FirstOrDefault();
                if (TournmentTypeObj == null)
                {
                    return Ok(new { Status = false, Message = "Tournment Type Obj Not Found" });
                }
                var TournmentCountryObj = _context.Countries.Where(e => e.CountryId == model.CountryId).FirstOrDefault();
                if (TournmentCountryObj == null)
                {
                    return Ok(new { Status = false, Message = "Tournment Country Obj Not Found" });
                }

                if (model.Cost == 0)
                {
                    return Ok(new { Status = false, Message = "Cost Must Be More Than 0" });
                }

                Tournment.TournamentDescAr = model.TournamentDescAr;
                Tournment.TournamentDescEn = model.TournamentDescEn;
                Tournment.TournamentTargetId = model.TournamentTargetId;
                Tournment.TournamentTypeId = model.TournamentTypeId;
                Tournment.CountryId = model.CountryId;
                Tournment.TournamentTlAr = model.TournamentTlAr;
                Tournment.TournamentTlEn = model.TournamentTlEn;
                Tournment.IsActive = model.IsActive;
                Tournment.StartDate = model.StartDate;
                Tournment.EndDate = model.EndDate;
                Tournment.SubStartDate = model.JoinStart;
                Tournment.SubEndDate = model.JoinEnd;
                Tournment.Cost = model.Cost;

                string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/Tournament");
                var wwwroot = _hostEnvironment.WebRootPath;
                if (TournmentPic != null)
                {
                    var ImagePath = Path.Combine(wwwroot, "Images/Tournament/" + Tournment.Pic);
                    if (System.IO.File.Exists(ImagePath))
                    {
                        System.IO.File.Delete(ImagePath);
                    }
                    string folder = "Images/Tournament/";
                    Tournment.Pic = UploadImage(folder, TournmentPic);
                }
                var UpdatedTournment = _context.Tournaments.Attach(Tournment);
                UpdatedTournment.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                return Ok(new { Status = true, Message = "Tournment Edited successfully" });

            }
            catch (Exception e)
            {

                return Ok(new { Status = false, Message = e.Message });

            }
        }

        [HttpPost]
        public async Task<IActionResult> SubscribeToTournment([FromBody] SubscriptionTournmentModelVm model)
        {
            try
            {

                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    return Ok(new { Status = false, Message = "User Not Found" });
                }
                var tournment = _context.Tournaments.Where(e => e.TournamentId == model.TournmentId).FirstOrDefault();

                if (tournment == null)
                {
                    return Ok(new { Status = false, Message = "Tournment Not Found" });
                }
                var PaymentMethod = _context.PaymentMethods.Where(e => e.PaymentMethodId == model.PaymentMethodId).FirstOrDefault();

                if (PaymentMethod == null)
                {
                    return Ok(new { Status = false, Message = "Payment Method Not Found" });
                }
                if (_context.Subscriptions.Any(e => e.EntityId == model.TournmentId && e.UserId == model.UserId && e.ispaid == true))
                {
                    return Ok(new { Status = false, Message = "User Is Already Subscriped" });
                }
                var subscriptionObj = new Subscription()
                {
                    Cost = tournment.Cost,
                    EntityId = tournment.TournamentId,
                    EntityName = "Tournment",
                    UserId = model.UserId,
                    PaymentMethodId = model.PaymentMethodId,
                    SubDate = DateTime.Now,
                    ispaid = false,
                    EntityTypeId = 3
                };
                _context.Subscriptions.Add(subscriptionObj);
                _context.SaveChanges();

                if (model.PaymentMethodId == 2 && subscriptionObj != null)
                {

                    var requesturl = "https://api.upayments.com/test-payment";
                    var fields = new
                    {
                        merchant_id = "1201",
                        username = "test",
                        password = "test",
                        order_id = subscriptionObj.SubscriptionId,
                        total_price = subscriptionObj.Cost,
                        test_mode = 0,
                        CstFName = user.FullName,
                        CstEmail = user.Email,
                        CstMobile = user.PhoneNumber,
                        api_key = "jtest123",
                        success_url = "http://coachkw.net/SubscriptionTournamentPaySuccess",
                        error_url = "http://coachkw.net/SubscriptionTournamentPayFailed",
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(fields), Encoding.UTF8, "application/json");
                    var task = httpClient.PostAsync(requesturl, content);
                    var result = await task.Result.Content.ReadAsStringAsync();
                    var paymenturl = JsonConvert.DeserializeObject<paymenturl>(result);
                    if (paymenturl.status == "success")

                    {
                        return Ok(new { Status = true, paymenturl = paymenturl.paymentURL });

                    }
                    else
                    {
                        return Ok(new { Status = false, Message = paymenturl.error_msg });
                    }
                }
                else
                {

                    return Ok(new { Status = true, paymenturl = "http://coachkw.net/Thankyou" });

                }
            }
            catch (Exception e)
            {
                return Ok(new { Status = false, Message = e.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetUserSubscribedToTournment([FromQuery] string UserId)
        {
            try
            {

                var TournmentList = await _context.Subscriptions.Where(c => c.UserId == UserId && c.EntityTypeId == 3 && c.ispaid == true).Select(e => e.EntityId).ToListAsync();
                List<Tournament> tournments = new List<Tournament>();
                if (TournmentList.Count != 0)
                {

                    foreach (var item in TournmentList)
                    {
                        var tournment = _context.Tournaments.Include(e => e.TournamentImages).Include(e => e.Country).Include(e => e.TournamentTarget).Include(e => e.TournamentType).Include(e => e.TournamentPlan).Where(e => e.TournamentId == item).FirstOrDefault();
                        if (tournment != null)
                        {
                            tournments.Add(tournment);
                        }

                    }
                }
                return Ok(new { Status = true, tournments = tournments });
            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, Message = ex.Message });

            }

        }
        [HttpGet]
        public IActionResult GetTournmentsbyUserId([FromQuery] string UserId)
        {
            try
            {
                var TournmentList = _context.Tournaments.Include(e => e.TournamentPlan).Include(e => e.TournamentTarget).Include(e => e.Country).Where(c => c.UserId == UserId && c.ispaid == true).Select(
                   j => new
                   {
                       j.TournamentId,
                       j.TournamentTlAr,
                       j.TournamentTlEn,
                       j.UserId,
                       j.TournamentTargetId,
                       j.TournamentTarget.TournamentTargetTlAr,
                       j.TournamentTarget.TournamentTargetTlEn,
                       j.TournamentPlanId,
                       j.TournamentPlan.PlanTlAr,
                       j.TournamentPlan.PlanTlEn,
                       j.CountryId,
                       j.Country.CountryTlAr,
                       j.Country.CountryTlEn,
                       j.TournamentTypeId,
                       j.TournamentType.TournamentTypeTlAr,
                       j.TournamentType.TournamentTypeTlEn,
                       j.TournamentDescAr,
                       j.TournamentDescEn,
                       j.IsActive,
                       j.StartDate,
                       j.SubStartDate,
                       j.SubEndDate,
                       j.PlanStartDate,
                       j.PlanEndDate,
                       j.EndDate,
                       j.Pic,
                       j.Cost,
                       TournamentImages = j.TournamentImages.ToList(),

                   }).ToList();

                return Ok(new { Status = true, TournmentList = TournmentList });
            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, Message = ex.Message });

            }
        }

        [HttpGet]
        public IActionResult GetTournmentListByCountry([FromQuery] int CountryId)
        {
            try
            {
                var TournmentList = _context.Tournaments.Include(e => e.TournamentPlan).Include(e => e.TournamentTarget).Include(e => e.Country).Where(c => c.CountryId == CountryId && c.PlanEndDate >= DateTime.Now && c.IsActive == true && c.ispaid == true).Select(
                  j => new
                  {
                      j.TournamentId,
                      j.TournamentTlAr,
                      j.TournamentTlEn,
                      j.UserId,
                      j.TournamentTargetId,
                      j.TournamentTarget.TournamentTargetTlAr,
                      j.TournamentTarget.TournamentTargetTlEn,
                      j.TournamentPlanId,
                      j.TournamentPlan.PlanTlAr,
                      j.TournamentPlan.PlanTlEn,
                      j.CountryId,
                      j.Country.CountryTlAr,
                      j.Country.CountryTlEn,
                      j.TournamentTypeId,
                      j.TournamentType.TournamentTypeTlAr,
                      j.TournamentType.TournamentTypeTlEn,
                      j.TournamentDescAr,
                      j.TournamentDescEn,
                      j.IsActive,
                      j.StartDate,
                      j.EndDate,
                      j.SubStartDate,
                      j.SubEndDate,
                      j.PlanStartDate,
                      j.PlanEndDate,
                      j.Pic,
                      j.Cost,
                      TournamentImages = j.TournamentImages.ToList(),

                  }).ToList();

                return Ok(new { Status = true, TournmentList });

            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, Message =ex.Message });

            }
        }

        #endregion
        [HttpGet]
        public IActionResult GetFAQList()
        {
            try
            {
                var faqsList = _context.FAQs.ToList();
                return Ok(new { Status = true, FaqsList = faqsList });

            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, message = ex.Message });

            }

        }
        [HttpGet]
        public IActionResult GetAllSocialLinks()
        {
            try
            {
                string adminRoleId = _db.Roles.Where(e => e.Name == "Admin").FirstOrDefault().Id;
                var UserAdminId = _db.UserRoles.Where(e => e.RoleId == adminRoleId).FirstOrDefault().UserId;
                var user = _userManager.Users.Where(e => e.Id == UserAdminId).FirstOrDefault();
                var SocialLinks = _context.Configurations.ToList().Take(1).FirstOrDefault();

                if (SocialLinks == null)
                {
                    return Ok(new { Status = false, message = "Object Not Exist" });
                }
                var SocialObj = new SocialLinksVm()
                {
                    Instgramlink = SocialLinks.Instgram,
                    WhatsApplink = SocialLinks.WhatsApp,
                    TwitterLink = SocialLinks.Twitter,
                    LinkedInlink = SocialLinks.LinkedIn,
                    facebooklink = SocialLinks.Facebook,
                    id = SocialLinks.ConfigurationId,
                    AdminEmail = user.Email,
                    AdminPhone = user.PhoneNumber

                };

                return Ok(new { Status = true, SocialLinks = SocialObj });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = ex.Message });

            }


        }
        [HttpGet]
        public IActionResult MakeNotificationIsRead([FromQuery] int PublicNotificationDeviceId)
        {

            try
            {
                var model = _context.PublicNotificationDevices.Find(PublicNotificationDeviceId);
                if (model == null)
                {
                    return Ok(new { Status = false, Message = "Notification not Found" });


                }
                model.IsRead = true;
                _context.PublicNotificationDevices.Update(model);
                _context.SaveChanges();

                return Ok(new { Status = true, Message = "Notification has been read" });

            }
            catch (Exception e)
            {

                return Ok(new { Status = false, Message =e.Message});

            }
        }

        [HttpGet]
        public IActionResult GetPublicNotificationByDeviceId([FromQuery] string deviceId)
        {
            try
            {
                var publicDevice = _context.PublicDevices.FirstOrDefault(c => c.DeviceId == deviceId);
                var List = _context.PublicNotificationDevices.Include(c => c.PublicNotification).Where(c => c.PublicDeviceId == publicDevice.PublicDeviceId);


                return Ok(new { Status = true, List });
            }
            catch (Exception e)
            {

                return Ok(new { Status = false, Message = e.Message });

            }


        }
       
        
        [HttpGet]
        public IActionResult GetCoursebyCoursebycourseId(int courseid)
        {
            try
            {
                if (courseid == 0)
                    return Ok(new { Status = false, Message = "Course not found" });
                var CourseList = _context.Courses.Select(
                   j => new
                   {
                       j.CourseId,
                       j.CourseTlAr,
                       j.CourseTlEn,
                       j.TrainerId,
                       j.CourseTargetId,
                       j.CourseTarget.CourseTargetTlAr,
                       j.CourseTarget.CourseTargetTlEn,
                       j.CourseDescAr,
                       j.CourseDescEn,
                       j.IsActive,
                       j.PublishDate,
                       j.Pic,
                       j.Cost,
                       CourseImage = j.CourseImages.ToList(),
                   }).FirstOrDefault(c => c.CourseId == courseid);
                var Trainer = _context.Trainers.Where(e => e.TrainerId == CourseList.TrainerId).Select(
                   i => new
                   {
                       i.TrainerId,
                       i.FullNameAr,
                       i.FullNameEn,
                       i.Email,
                       i.Mobile,
                       i.Tele,
                       i.Fax,
                       i.GenderId,
                       i.Country.CountryTlAr,
                       i.Country.CountryTlEn,
                       i.CountryId,
                       i.Pic,
                       i.Section.SectionTlAr,
                       i.Section.SectionTlEn,
                       i.SectionId,
                       i.DescriptionAr,
                       i.DescriptionEn,
                       TrainerSubscriptions = i.TrainerSubscriptions.OrderByDescending(e => e.TrainerSubscriptionId).Where(e => e.TrainerId == i.TrainerId).FirstOrDefault(),
                       i.UserId,
                   }).FirstOrDefault();
                return Ok(new { Status = true, CourseList= CourseList,trainer= Trainer });


            }
            catch (Exception ex)
            {

                return Ok(new { Status = false, Message = ex.Message });

            }
        }


        [HttpGet]
        public IActionResult GetTournamentbyTournamentId(int tournamentId)
        {
            try
            {
                if (tournamentId == 0)
                    return Ok(new { Status = false, Message = "Tournament not found" });
                var TournamentList = _context.Tournaments.Include(e => e.TournamentPlan).Include(e => e.TournamentTarget).Include(e => e.Country).Select(
                   j => new
                   {
                       j.TournamentId,
                       j.TournamentTlAr,
                       j.TournamentTlEn,
                       j.UserId,
                       j.TournamentTargetId,
                       j.TournamentTarget.TournamentTargetTlAr,
                       j.TournamentTarget.TournamentTargetTlEn,
                       j.TournamentPlanId,
                       j.TournamentPlan.PlanTlAr,
                       j.TournamentPlan.PlanTlEn,
                       j.CountryId,
                       j.Country.CountryTlAr,
                       j.Country.CountryTlEn,
                       j.TournamentTypeId,
                       j.TournamentType.TournamentTypeTlAr,
                       j.TournamentType.TournamentTypeTlEn,
                       j.TournamentDescAr,
                       j.TournamentDescEn,
                       j.IsActive,
                       j.StartDate,
                       j.EndDate,
                       j.SubStartDate,
                       j.SubEndDate,
                       j.PlanStartDate,
                       j.PlanEndDate,
                       j.Pic,
                       j.Cost,
                       TournamentImages = j.TournamentImages.ToList(),

                   }).FirstOrDefault(c => c.TournamentId == tournamentId);
                return Ok(new { Status = true, TournamentList = TournamentList });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = ex.Message });
            }
        }


        [HttpGet]
        public IActionResult GetCampbyCampId(int campId)
        {
            try
            {
                if (campId == 0)
                    return Ok(new { Status = false, Message = "Camp not found" });
                var CampList = _context.Camps.Include(e => e.CampPlan).Include(e => e.CampTarget).Include(e => e.Country).Select(
                   j => new
                   {
                       j.CampId,
                       j.CampTlAr,
                       j.CampTlEn,
                       j.UserId,
                       j.CampTargetId,
                       j.CampTarget.CampTargetTlAr,
                       j.CampTarget.CampTargetTlEn,
                       j.CampPlanId,
                       j.CampPlan.PlanTlAr,
                       j.CampPlan.PlanTlEn,
                       j.CountryId,
                       j.Country.CountryTlAr,
                       j.Country.CountryTlEn,
                       j.CampTypeId,
                       j.CampType.CampTypeTlAr,
                       j.CampType.CampTypeTlEn,
                       j.CampDescAr,
                       j.CampDescEn,
                       j.IsActive,
                       j.StartDate,
                       j.EndDate,
                       j.JoinStart,
                       j.JoinEnd,
                       j.Pic,
                       j.Cost,
                       CampsImages = j.CampImages.ToList(),

                   }).FirstOrDefault(c => c.CampId == campId);
                return Ok(new { Status = true, CampList = CampList });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = ex.Message });
            }
        }



        [HttpGet]
        public IActionResult GetTrainerbyTrainerId(int TrainerId)
        {
            try
            {
                if (TrainerId == 0)
                    return Ok(new { Status = false, Message = "Trainer not found" });
                var TrainerList = _context.Trainers.Select(
                   i => new
                   {
                       i.TrainerId,
                       i.FullNameAr,
                       i.FullNameEn,
                       i.Email,
                       i.Mobile,
                       i.Tele,
                       i.Fax,
                       i.GenderId,
                       i.Country.CountryTlAr,
                       i.Country.CountryTlEn,
                       i.CountryId,
                       i.Pic,
                       i.Section.SectionTlAr,
                       i.Section.SectionTlEn,
                       i.SectionId,
                       i.DescriptionAr,
                       i.DescriptionEn,
                       TrainerSubscriptions = i.TrainerSubscriptions.OrderByDescending(e => e.TrainerSubscriptionId).Where(e => e.TrainerId == i.TrainerId).FirstOrDefault(),
                       i.UserId,
                       TrainerImage = i.TrainerImages.ToList(),

                       Courses = i.Courses.Select(j => new
                       {
                           j.CourseId,
                           j.CourseTlAr,
                           j.CourseTlEn,
                           j.TrainerId,
                           j.CourseTargetId,
                           j.CourseTarget.CourseTargetTlAr,
                           j.CourseTarget.CourseTargetTlEn,
                           j.CourseDescAr,
                           j.CourseDescEn,
                           j.IsActive,
                           j.PublishDate,
                           j.Pic,
                           j.Cost,
                           CourseImage = j.CourseImages.ToList(),

                       })

                   }).FirstOrDefault(c => c.TrainerId == TrainerId);
                return Ok(new { Status = true, TrainerList = TrainerList});
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = ex.Message });
            }
        }
    }
}
