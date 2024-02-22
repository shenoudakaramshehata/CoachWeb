using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using NToastNotify;
using Coach.Data;
using System;
using Microsoft.EntityFrameworkCore;

namespace Coach.Areas.admin.Pages.AdminProfile
{
    [Authorize]
    public class ChangeEmailModel : PageModel
    {
        private readonly ApplicationDbContext applicationDb;
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IToastNotification _toastNotification;
        [BindProperty]
        public Entities.ChangeEmailVM ChangeEmailVM { get; set; }
        public ChangeEmailModel(ApplicationDbContext applicationDb, UserManager<ApplicationUser> userManager, IToastNotification toastNotification, SignInManager<ApplicationUser> signInManager)
        {
            this.applicationDb = applicationDb;
            _userManger = userManager;
            _toastNotification = toastNotification;
            _signInManager = signInManager;
        }
        public void OnGet()
        {

        }
        //public async Task<IActionResult> OnPost()
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //            return Page();
        //        if (resetPasswordModel.ConfirmPassword != resetPasswordModel.NewPassword)
        //        {
        //            _toastNotification.AddErrorToastMessage("Password not matched");
        //        }
        //        var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //        var user = await _userManager.FindByIdAsync(userid);
        //        var Result = await _userManager.ChangePasswordAsync(user, resetPasswordModel.CurrentPassword, resetPasswordModel.NewPassword);
        //        if (!Result.Succeeded)
        //        {
        //            foreach (var error in Result.Errors)
        //            {
        //                ModelState.TryAddModelError("", error.Description);
        //            }
        //            return Page();

        //        }
        //        _toastNotification.AddSuccessToastMessage("Password Updated Successfully");


        //    }
        //    catch (Exception)
        //    {
        //        _toastNotification.AddErrorToastMessage("Something went Error");
        //    }
        //    return RedirectToPage("../Login");
        //}

        public async Task<IActionResult> OnPost()
        {
            try
            {


                if (!ModelState.IsValid)
                    return Page();
                var user = await _userManger.GetUserAsync(User);
                user.UserName = ChangeEmailVM.NewEmail;
                user.Email = ChangeEmailVM.NewEmail;
                await _userManger.UpdateAsync(user);
                applicationDb.Attach(user).State = EntityState.Modified;
                applicationDb.SaveChanges();
                await _signInManager.RefreshSignInAsync(user);
                _toastNotification.AddSuccessToastMessage("Email Updated Successfully");
                return RedirectToPage("../Index");
            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something Went Error");
                return RedirectToPage("../Index");
            }
        }
    }
}
