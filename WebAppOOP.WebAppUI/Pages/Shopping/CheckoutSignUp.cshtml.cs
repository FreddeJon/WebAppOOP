using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppOOP.BusinessLogic.Authentication.CartAuthentication.Interfaces;
using WebAppOOP.BusinessLogic.Authentication.UserAuthentication.Interfaces;
using WebAppOOP.Core.ModelDTOS;

namespace WebAppOOP.WebAppUI.Pages.Shopping
{
    public class CheckoutSignUpModel : PageModel
    {
        private readonly IAuthentication _authentication;

        public CheckoutSignUpModel(IAuthentication authentication)
        {
            _authentication = authentication;
        }


        [TempData]
        public string Message { get; set; }


        [BindProperty]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        public string Password { get; set; }

        [BindProperty]
        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        public IActionResult OnGet()
        {
            return RedirectToPage("/Index");
        }

        public IActionResult OnGetSignUpCheckout()
        {
            if (User.Identity.IsAuthenticated) return RedirectToPage("/Index");
            return Page();
        }

        public async Task<IActionResult> OnPostRegisterAsync()
        {
            if (!ModelState.IsValid) return Page();

            UserCredential userCredential = new() { Email = Email, Password = Password };

            var authentication = _authentication.CreateUser(userCredential);

            if (!authentication.IsAuthenticated)
            {
                TempData["Message"] = authentication.Message;
                return RedirectToPage();
            }
            TempData["Message"] = authentication.Message;
            await HttpContext.SignInAsync(authentication.Principal);

            return RedirectToPage("./Checkout", "FromSignUp");
        }
    }
}
