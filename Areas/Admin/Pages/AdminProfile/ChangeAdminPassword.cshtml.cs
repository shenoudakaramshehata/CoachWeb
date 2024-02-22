using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Coach.Data;
using Coach.Entities;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using NToastNotify;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coach.Areas.Admin.Pages.AdminProfile
{
    public class ChangeAdminPasswordModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IToastNotification _toastNotification;
        [BindProperty]
        public Coach.Entities.ChangePasswordModel resetPasswordModel { get; set; }
        public ChangeAdminPasswordModel(SignInManager<ApplicationUser> signInManager, IToastNotification toastNotification, ILogger<LogoutModel> logger, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _toastNotification = toastNotification;
        }
        public void OnGet()
        {
        }

       
        public async Task<IActionResult> OnPost()
        {
            try
            {
                if (!ModelState.IsValid)
                   return Page();
                if (resetPasswordModel.ConfirmPassword != resetPasswordModel.NewPassword)
                {
                    _toastNotification.AddErrorToastMessage("Password not matched");
                }
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userid);
                var Result = await _userManager.ChangePasswordAsync(user, resetPasswordModel.CurrentPassword, resetPasswordModel.NewPassword);
                if (!Result.Succeeded)
                {
                    foreach (var error in Result.Errors)
                    {
                        ModelState.TryAddModelError("", error.Description);
                    }
                    return Page();

                }
                _toastNotification.AddSuccessToastMessage("Password Changed");

                
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went Error");
            }
            return RedirectToPage("../Index");
        }
    }
}
