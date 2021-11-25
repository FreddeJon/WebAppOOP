using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppOOP.Core.ModelDTOS;
using WebbAppOOP.Data.DataAccess.Interfaces;

namespace WebAppOOP.WebAppUI.Pages.Profile
{
    [Authorize]
    public class OrderDetailsModel : PageModel
    {
        private readonly IDataAccess<Order> _orderAccess;

        public OrderDetailsModel(IDataAccess<Order> orderAccess)
        {
            _orderAccess = orderAccess;
        }

        public Order Order { get; private set; }

        public IActionResult OnGet(int id)
        {
            Order = _orderAccess.GetById(id);
            if (Order is null)
            {
                return RedirectToPage("./Orders ");
            }
            return Page();
        }
    }
}
