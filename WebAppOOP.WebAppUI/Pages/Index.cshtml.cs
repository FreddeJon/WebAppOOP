using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WebAppOOP.BusinessLogic.Authentication.CartAuthentication.Interfaces;
using WebAppOOP.Core.ModelDTOS;
using WebAppOOP.Core.ModelDTOS.ProductDTOs;
using WebbAppOOP.Data.DataAccess.Interfaces;

namespace WebAppOOP.WebAppUI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IProductDataAccess _productDataAccess;
        private readonly ICartAuthentication _cartAccess;

        public IndexModel(IProductDataAccess productDataAccess, ICartAuthentication cartAccess)
        {

            _productDataAccess = productDataAccess;
            _cartAccess = cartAccess;
        }

        public string SortOrder { get; private set; }
        public List<Product> Products { get; private set; }


        [TempData]
        public string Message { get; set; }
        [BindProperty]
        public int ProductId { get; set; }
        [BindProperty]
        public string Search { get; set; }


        public void OnGet(string sortOrder)
        {
            LoadProducts();
            SortOrder = String.IsNullOrEmpty(sortOrder) ? "" : sortOrder;

       

            Products = SortOrder switch
            {
                "?sort=HigestPrice" => Products.OrderByDescending(p => p.Price).ToList(),
                "?sort=LowestPrice" => Products.OrderBy(p => p.Price).ToList(),
                "?sort=Physical" => Products.OrderByDescending(p => p.ProductType).ToList(),
                "?sort=Digital" => Products.OrderBy(p => p.ProductType).ToList(),
                _ => Products.OrderByDescending(p => p.Name).ToList(),
            };
        }



        public IActionResult OnPostSearch()
        {
            LoadProducts();
            if (Search is not null)
            {
                var temp = (from prod in Products
                            where prod.Name.ToLower().Contains(Search.ToLower()) || prod.Name.ToLower().StartsWith(Search.ToLower())
                            orderby prod.Name
                            select prod).ToList();

                if (temp.Count > 0)
                {
                    Products = temp;
                }
                else
                {
                    TempData["Message"] = "Not found";
                    return RedirectToPage();
                }
            }
            return Page();
        }

        public void OnPostAdd()
        {
            AddToCart(LoadShoppingCart(), ProductId);
            LoadProducts();
        }


        private void AddToCart(ShoppingCart cart, int productId)
        {
            if (cart.GetList().Any(x => x.Id == productId))
            {
                var product = cart.GetList().FirstOrDefault(x => x.Id == productId);
                product.Quantity++;
            }
            else
            {
                var product = _productDataAccess.GetById(productId);
                product.Quantity++;
                cart.Add(product);
            }
            UpdateShoppingCart(cart);
        }


        private List<Product> LoadProducts()
        {
            Products = _productDataAccess.GetAll();
            return Products;
        }

        private ShoppingCart LoadShoppingCart() => UserIsAuthenticated() ? _cartAccess.GetByKey(GetUserKey()) : _cartAccess.GetByKey("LOCALCART");
        private bool UserIsAuthenticated() => User.Identity.IsAuthenticated;
        private string GetUserKey() => ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Sid).Value;
        private void UpdateShoppingCart(ShoppingCart cart) => _cartAccess.Update(cart);
    }
}
