using System;
using System.Collections.Generic;
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
using Coach.Entities.Notification;

namespace Coach.Areas.Admin.Pages.PublicNotifications
{
    public class SendModel : PageModel
    {
        private CoachContext _context;
        private readonly IToastNotification _toastNotification;
        private readonly INotificationService _notificationService;

        public SendModel(CoachContext context, INotificationService notificationService, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
            _notificationService = notificationService;


        }
        [BindProperty]
        public PublicNotification publicNotification { get; set; }

        public IActionResult OnGetAsync(int id)
        {
            try
            {
                publicNotification = _context.PublicNotifications.Include(c => c.EntityType).Include(c => c.Country).Where(c => c.PublicNotificationId == id).FirstOrDefault();

                if (publicNotification == null)
                {
                    return Redirect("../Error");
                }



                if (publicNotification.EntityTypeId == 2)
                {
                    publicNotification.EntityNameAr  = _context.Camps.FirstOrDefault(c => c.CampId == publicNotification.EntityId)?.CampTlAr;
                    publicNotification.EntityNameEn = _context.Camps.FirstOrDefault(c => c.CampId == publicNotification.EntityId)?.CampTlEn;
                }
                if (publicNotification.EntityTypeId == 3)
                {
                    publicNotification.EntityNameAr = _context.Tournaments.FirstOrDefault(c => c.TournamentId == publicNotification.EntityId)?.TournamentTlAr;
                    publicNotification.EntityNameEn = _context.Tournaments.FirstOrDefault(c => c.TournamentId == publicNotification.EntityId)?.TournamentTlEn;
                }
                if (publicNotification.EntityTypeId == 4)
                {
                    publicNotification.EntityNameAr = _context.Courses.FirstOrDefault(c => c.CourseId == publicNotification.EntityId)?.CourseTlAr;
                    publicNotification.EntityNameEn = _context.Courses.FirstOrDefault(c => c.CourseId == publicNotification.EntityId)?.CourseTlEn;
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
                publicNotification = _context.PublicNotifications.Include(c => c.EntityType).Include(c => c.Country).Where(c => c.PublicNotificationId == id).FirstOrDefault();

                if (publicNotification == null)
                {
                    return Redirect("../Error");
                }
                if (publicNotification.EntityTypeId == 2)
                {
                    publicNotification.EntityNameAr = _context.Camps.FirstOrDefault(c => c.CampId == publicNotification.EntityId)?.CampTlAr;
                    publicNotification.EntityNameEn = _context.Camps.FirstOrDefault(c => c.CampId == publicNotification.EntityId)?.CampTlEn;
                }
                if (publicNotification.EntityTypeId == 3)
                {
                    publicNotification.EntityNameAr = _context.Tournaments.FirstOrDefault(c => c.TournamentId == publicNotification.EntityId)?.TournamentTlAr;
                    publicNotification.EntityNameEn = _context.Tournaments.FirstOrDefault(c => c.TournamentId == publicNotification.EntityId)?.TournamentTlEn;
                }
                if (publicNotification.EntityTypeId == 4)
                {
                    publicNotification.EntityNameAr = _context.Courses.FirstOrDefault(c => c.CourseId == publicNotification.EntityId)?.CourseTlAr;
                    publicNotification.EntityNameEn = _context.Courses.FirstOrDefault(c => c.CourseId == publicNotification.EntityId)?.CourseTlEn;
                }
                var PublicDeviceList = _context.PublicDevices.Where(c => c.CountryId == publicNotification.CountryId).ToList();

                    foreach (var item in PublicDeviceList)
                    {

                    var notificationModel = new NotificationModel();
                    notificationModel.DeviceId = item.DeviceId;
                    notificationModel.IsAndroiodDevice = item.IsAndroiodDevice;
                    notificationModel.Title = publicNotification.Title;
                    notificationModel.Body = publicNotification.Body;
                    notificationModel.EntityId = publicNotification.EntityId;
                    notificationModel.EntityTypeId = publicNotification.EntityTypeId;
                    var result = await _notificationService.SendNotification(notificationModel);
                    if (result.IsSuccess)
                    {
                        var publicNotificationDeviceExiest = _context.PublicNotificationDevices.Any(c => c.PublicNotificationId == publicNotification.PublicNotificationId
                         && c.PublicDeviceId == item.PublicDeviceId);
                        if (!publicNotificationDeviceExiest)
                        {
                            var publicNotificationDevice = new PublicNotificationDevice()
                            {
                                PublicNotificationId = publicNotification.PublicNotificationId,
                                PublicDeviceId = item.PublicDeviceId,
                                IsRead = false
                            };
                            _context.PublicNotificationDevices.Add(publicNotificationDevice);
                            _context.SaveChanges();
                        }


                    }
                    _toastNotification.AddSuccessToastMessage("Notification Sent successfully");

                }

            }

            
            catch (Exception)

            {

                _toastNotification.AddErrorToastMessage("Sothing want Be Wrong");
                return Page();

            }

            return RedirectToPage("./Index");
        }
    }
}
