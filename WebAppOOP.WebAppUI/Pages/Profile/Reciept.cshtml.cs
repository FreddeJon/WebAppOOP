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
    public class RecieptModel : PageModel
    {
        private readonly IDataAccess<Reciept> _recieptAccess;

        public RecieptModel(IDataAccess<Reciept> recieptAccess)
        {
            _recieptAccess = recieptAccess;
        }

        public Reciept Reciept { get; private set; }

        public IActionResult OnGet(string orderKey)
        {
            Reciept = _recieptAccess.GetByKey(orderKey);
            if (Reciept is null)
            {
                return RedirectToPage("./Orders");
            }
            return Page();
        }
    }
}
