using Coach.Data;
using Coach.Entities.Notification;
using Coach.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coach.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly CoachContext _context;

        public NotificationController(INotificationService notificationService, CoachContext context)
        {
            _notificationService = notificationService;
            _context = context;
        }


        [HttpPost]
        public async Task<IActionResult> SendNotification(NotificationModel model)
        {

            var notificationModel = new NotificationModel();

            notificationModel.DeviceId = "/public";
            notificationModel.IsAndroiodDevice = true;
            notificationModel.Title = "Hello everyone";
            notificationModel.Body = "You Have A New Order";
            model.IsAndroiodDevice = true;

            var result = await _notificationService.SendNotification(model);

            return Ok(result);

        }
    }
}

