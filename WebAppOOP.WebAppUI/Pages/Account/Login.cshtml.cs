using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppOOP.BusinessLogic.Authentication.UserAuthentication.Interfaces;
using WebAppOOP.Core.ModelDTOS;

namespace WebAppOOP.WebAppUI.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IAuthentication _authentication;

        public LoginModel(IAuthentication authentication)
        {
            _authentication = authentication;
        }
        [TempData]
        public string Message { get; set; }

        [BindProperty]
        public UserCredential UserCredential { get; set; }


        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var authentication = _authentication.AuthenticateUser(UserCredential);

            if (authentication.IsAuthenticated)
            {
                await HttpContext.SignInAsync(authentication.Principal);
                TempData["Message"] = authentication.Message;
                return RedirectToPage("/Index");
            }


            TempData["Message"] = authentication.Message;
            return RedirectToPage();
        }
    }
}
