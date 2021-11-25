using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppOOP.Core.ModelDTOS;

namespace WebAppOOP.BusinessLogic.Authentication.CartAuthentication.Interfaces
{
    public interface ICartAuthentication
    {
        ShoppingCart GetByKey(string userKey);
        void Update(ShoppingCart cart);
        void DeleteCart(ShoppingCart cart);
    }
}
