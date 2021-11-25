using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppOOP.Core.ModelDTOS;
using WebbAppOOP.Data.DataAccess.Interfaces;

namespace WebAppOOP.WebAppUI.Pages.Profile
{
    [Authorize]
    public class OrdersModel : PageModel
    {
        private readonly IDataAccess<Order> _orderAccess;

        public OrdersModel(IDataAccess<Order> orderAccess)
        {
            _orderAccess = orderAccess;
        }
        public List<Order> Orders { get; set; }

        [BindProperty]
        public int OrderID { get; set; }

        public string PaidSort { get; private set; }

        public void OnGet(string sortOrder)
        {
            PaidSort = String.IsNullOrEmpty(sortOrder) ? "" : sortOrder;

            Orders = LoadOrders();

            Orders = PaidSort switch
            {
                "?sort=IsPaid" => Orders.OrderByDescending(s => s.IsPaid).ToList(),
                "?sort=NotPaid" => Orders.OrderBy(s => s.IsPaid).ToList(),
                "?sort=HighestPrice" => Orders.OrderByDescending(s => s.ShoppingCart.GetCost()).ToList(),
                "?sort=LowestPrice" => Orders.OrderBy(s => s.ShoppingCart.GetCost()).ToList(),
                _ => Orders.OrderBy(s => s.OrderDate).ToList(),
            };
        }


        private List<Order> LoadOrders()
        {
            Orders = (from order in _orderAccess.GetAll()
                      where order.Key == GetUserKey()
                      select order).ToList();
            return Orders;
        }
        private string GetUserKey()
        {
            return ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Sid).Value;
        }
    }
}
