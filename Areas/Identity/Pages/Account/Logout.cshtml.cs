﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Coach.Data;

namespace Coach.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            //if (returnUrl != null)
            //{
            //    //return LocalRedirect(returnUrl);
            //    return RedirectToPage("/Index");
            //}
            //else
            //{
                //return RedirectToPage();
                return RedirectToPage("/Index");
            //}

        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                //return LocalRedirect(returnUrl);
                return RedirectToPage("/Index");
            }
            else
            {
                //return RedirectToPage();
                return RedirectToPage("/Index");
            }
        }
    }
}
