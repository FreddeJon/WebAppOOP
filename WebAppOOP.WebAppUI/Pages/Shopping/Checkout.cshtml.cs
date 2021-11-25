using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppOOP.BusinessLogic.Authentication.CartAuthentication.Interfaces;
using WebAppOOP.Core.ModelDTOS;
using WebbAppOOP.Data.DataAccess.Interfaces;

namespace WebAppOOP.WebAppUI.Pages.Shopping
{
    public class CheckoutModel : PageModel
    {
        private readonly ICartAuthentication _cartAccess;
        private readonly IDataAccess<Order> _orderAccess;
        private readonly IDataAccess<Reciept> _recieptAccess;

        public ShoppingCart Cart { get; private set; }

        public CheckoutModel(ICartAuthentication cartAccess, IDataAccess<Order> orderAccess, IDataAccess<Reciept> recieptAccess)
        {
            _cartAccess = cartAccess;
            _orderAccess = orderAccess;
            _recieptAccess = recieptAccess;
        }

        [BindProperty]
        [Required]
        public Card Card { get; set; }


        public IActionResult OnGet()
        {
            return RedirectToPage("/Index");
        }

        public void OnGetSignedIn()
        {
            Cart = _cartAccess.GetByKey(GetUserKey());
        }
        public IActionResult OnGetSignUp()
        {
            return RedirectToPage("./CheckoutSignUp", "SignUpCheckout");
        }

        public IActionResult OnGetFromSignUp()
        {
            var cart = _cartAccess.GetByKey(GetUserKey());
            var localCart = _cartAccess.GetByKey("LOCALCART");

            cart.Products = localCart.Products;

            _cartAccess.Update(cart);
            _cartAccess.DeleteCart(localCart);
            return RedirectToPage("./Checkout", "SignedIn");
        }

        public IActionResult OnPostPay()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var order = new Order() { OrderDate = DateTime.Now, Id = _orderAccess.GetNewID(), IsPaid = true, Key = GetUserKey(), ShoppingCart = _cartAccess.GetByKey(GetUserKey()) };
            order.RecieptKey = order.Key + order.Id;
            var licenceKeys = new List<string>();

            foreach (var item in order.ShoppingCart.Products.DigitalProducts)
            {
                for (int i = 0; i < item.Quantity; i++)
                {
                licenceKeys.Add(item.GenerateLicenceKey());
                }
            }

            var reciept = new Reciept()
            {
                Card = Card,
                Id = _recieptAccess.GetNewID(),
                Key = order.RecieptKey,
                Cart = order.ShoppingCart,
                LicenceKeys = licenceKeys,
                PayDate = DateTime.Now 
            };


            _recieptAccess.Add(reciept);
            _orderAccess.Add(order);
            _cartAccess.DeleteCart(_cartAccess.GetByKey(GetUserKey()));

            return RedirectToPage("/Profile/Orders");
        }
        public IActionResult OnPostLater()
        {
            var order = new Order() { OrderDate = DateTime.Now, Id = _orderAccess.GetNewID(), IsPaid = false, Key = GetUserKey(), ShoppingCart = _cartAccess.GetByKey(GetUserKey()) };
            _orderAccess.Add(order);
            _cartAccess.DeleteCart(_cartAccess.GetByKey(GetUserKey()));
            return RedirectToPage("/Profile/Orders");
        }

        private string GetUserKey() => ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Sid).Value;
    }
}
