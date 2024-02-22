using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Coach.Data;

namespace Coach.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;


        public LoginModel(SignInManager<ApplicationUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "User Name")]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)


        {
            //bool x = await _roleManager.RoleExistsAsync("Admin");
            //if (!x)   
            //{
            //    // first we create Admin rool    
            //    var role = new IdentityRole();
            //    role.Name = "Customer";
            //    await _roleManager.CreateAsync(role);

            //    //Here we create a Admin super user who will maintain the website
            //    //
            //    var user = new IdentityUser { UserName = "Admin", Email = "Admin@ifo.com" };
            //    var result = await _userManager.CreateAsync(user, "P@ssw0rd");
            //     var roleResult =await _userManager.AddToRoleAsync(user, "Admin");



            //}

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.UserName , Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var userExists = await _userManager.FindByNameAsync(Input.UserName);
                    
                    var roleNames = await _userManager.GetRolesAsync(userExists);
                    _logger.LogInformation("User logged in.");

                    if (roleNames.Count==0)
                    {
                        await _signInManager.SignOutAsync();
                        _logger.LogInformation("User logged out.");

                    }
                    if (roleNames.FirstOrDefault()=="Customer")
                    {
                        return Redirect("~"+ returnUrl);
                    }
                    if (roleNames.FirstOrDefault() == "Admin")
                    {
                        //return Redirect("~"+ returnUrl);
                        return Redirect("~/Admin/Index");
                    }
                    await _signInManager.SignOutAsync();
                    _logger.LogInformation("User logged out.");
                    return Redirect("~/");
                        

                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
