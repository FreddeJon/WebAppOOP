using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppOOP.Core.ModelDTOS;
using WebbAppOOP.Data.DataAccess.Interfaces;

namespace WebAppOOP.WebAppUI.Pages.Profile
{
    public class PayLaterModel : PageModel
    {
        private readonly IDataAccess<Order> _orderAccess;
        private readonly IDataAccess<Reciept> _recieptAccess;

        public PayLaterModel(IDataAccess<Order> orderAccess, IDataAccess<Reciept> recieptAccess)
        {
            _orderAccess = orderAccess;
            _recieptAccess = recieptAccess;
        }
        [BindProperty]
        [Required]
        public Card Card { get; set; }
        public Order Order { get; private set; }

        public IActionResult OnGet(int orderKey)
        {
            Order = _orderAccess.GetById(orderKey);
            if (Order is null) return RedirectToPage("./Orders");

            HttpContext.Session.SetInt32("OrderId", orderKey);
            return Page();
        }

        public IActionResult OnPostPay()
        {
            if (!ModelState.IsValid) return Page();

            Order = _orderAccess.GetById((int)HttpContext.Session.GetInt32("OrderId"));
            Order.IsPaid = true;

            Order.RecieptKey = Order.Key + Order.Id;

            var licenceKeys = new List<string>();

            foreach (var item in Order.ShoppingCart.Products.DigitalProducts)
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
                Key = Order.RecieptKey,
                Cart = Order.ShoppingCart,
                LicenceKeys = licenceKeys,
                PayDate = DateTime.Now
            };

            _orderAccess.Update(Order);
            _recieptAccess.Add(reciept);
            return RedirectToPage("/Profile/Orders");
        }
    }
}
