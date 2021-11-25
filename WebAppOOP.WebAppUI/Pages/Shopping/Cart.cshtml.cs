using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppOOP.BusinessLogic.Authentication.CartAuthentication.Interfaces;
using WebAppOOP.Core.ModelDTOS;

namespace WebAppOOP.WebAppUI.Pages.Shopping
{
    public class CartModel : PageModel
    {
        private readonly ICartAuthentication _cartAccess;

        public CartModel(ICartAuthentication cartAccess)
        {
            _cartAccess = cartAccess;
        }

        public ShoppingCart Cart { get; private set; }

        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        [Required]
        public string Key { get; set; }


        public IActionResult OnGet()
        {
            LoadShoppingCart();
            if (Cart.GetList().Count < 1)
            {
                return RedirectToPage("./Empty");
            }
            return Page();
        }


        public IActionResult OnPostRemove()
        {
            LoadShoppingCart();
            Cart.Remove(Cart.GetList().FirstOrDefault(x => x.Id == Id));
            UpdateShoppingCart();
            if (Cart.GetList().Count < 1) return RedirectToPage();
            return Page();
        }

        public IActionResult OnPostDecrease()
        {
            LoadShoppingCart();

            var product = Cart.GetList().FirstOrDefault(x => x.Id == Id);
            if (--product.Quantity == 0) Cart.Remove(product);

            UpdateShoppingCart();

            if (Cart.GetList().Count < 1) return RedirectToPage();
            return Page();
        }

        public void OnPostIncrease()
        {

            LoadShoppingCart();

            var product = Cart.GetList().FirstOrDefault(x => x.Id == Id);
            product.Quantity++;

            UpdateShoppingCart();
        }
        public IActionResult OnPostCheckout()
        {
            LoadShoppingCart();

            if (UserIsAuthenticated())
            {
                return RedirectToPage("./Checkout", "SignedIn");
            }
            return RedirectToPage("./Checkout", "SignUp");
        }






        private void LoadShoppingCart() => Cart = UserIsAuthenticated() ? _cartAccess.GetByKey(GetUserKey()) : _cartAccess.GetByKey("LOCALCART");
        private bool UserIsAuthenticated() => User.Identity.IsAuthenticated;
        private string GetUserKey() => ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Sid).Value;
        private void UpdateShoppingCart() => _cartAccess.Update(Cart);



    }
}
