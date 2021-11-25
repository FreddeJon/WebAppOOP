using System.Collections.Generic;
using WebAppOOP.Core.ModelDTOS.ProductDTOs;

namespace WebbAppOOP.Data.DataAccess.Interfaces
{
    public interface IProductDataAccess
    {
        List<Product> GetAll();
        Product GetById(int id);
        void Update(Product item);
        void Remove(Product item);
        int GetNewID();
    }
}
