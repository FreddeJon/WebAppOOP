using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppOOP.BusinessLogic.Authentication.CartAuthentication.Interfaces;
using WebAppOOP.Core.ModelDTOS;
using WebAppOOP.Core.ModelDTOS.ProductDTOs;
using WebbAppOOP.Data.DataAccess.Interfaces;

namespace WebAppOOP.BusinessLogic.Authentication.CartAuthentication
{
    public class CartAuthentication : ICartAuthentication
    {
        private readonly IDataAccess<ShoppingCart> _cartDataAccess;

        public CartAuthentication(IDataAccess<ShoppingCart> cartDataAccess)
        {
            _cartDataAccess = cartDataAccess;
        }
        public void DeleteCart(ShoppingCart cart)
        {
            _cartDataAccess.Remove(cart);
        }

        public ShoppingCart GetByKey(string userKey)
        {
            var cart = _cartDataAccess.GetByKey(userKey);
            if (cart is null)
            {
                cart = new ShoppingCart() { Id = _cartDataAccess.GetNewID(), Key = userKey, Products = new ProductDTO() };
                _cartDataAccess.Add(cart);
            }

            return cart;
        }

        public void Update(ShoppingCart cart)
        {
            _cartDataAccess.Update(cart);
        }
    }
}
