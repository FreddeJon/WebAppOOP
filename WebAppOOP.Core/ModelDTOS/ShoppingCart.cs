using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppOOP.Core.ModelDTOS.Interfaces;
using WebAppOOP.Core.ModelDTOS.ProductDTOs;

namespace WebAppOOP.Core.ModelDTOS
{
    public class ShoppingCart : IEntity
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public ProductDTO Products { get; set; }

        public string GetCostToString()
        {
            return $"{(Products.DigitalProducts.Sum(x => x.GetCost()) + Products.PhysicalProducts.Sum(x => x.GetCost())):C}";
        }
        public decimal GetCost()
        {
            return (Products.DigitalProducts.Sum(x => x.GetCost()) + Products.PhysicalProducts.Sum(x => x.GetCost()));
        }
        public string GetTotalIncShipping()
        {
            return $"{GetShippingCost() + GetCost():C}";
        }
        public List<Product> GetList()
        {
            var list = new List<Product>();
            list.AddRange(Products.DigitalProducts);
            list.AddRange(Products.PhysicalProducts);
            return list;
        }

        public void Add(Product product)
        {
            if (product.GetType() == typeof(PhysicalProduct))
            {
                Products.PhysicalProducts.Add((PhysicalProduct)product);
            }
            else
            {
                Products.DigitalProducts.Add((DigitalProduct)product);
            }
        }


        public void Remove(Product product)
        {
            if (product.GetType() == typeof(PhysicalProduct))
            {
                Products.PhysicalProducts.Remove((PhysicalProduct)product);
            }
            else
            {
                Products.DigitalProducts.Remove((DigitalProduct)product);
            }
        }

        public decimal GetShippingCost()
        {
            decimal sum = 0;
            foreach (var prod in Products.PhysicalProducts)
            {
                if (prod.Quantity > 1)
                {
                    sum += prod.Weight * prod.Quantity * 1.5M;
                }
                else
                {
                    sum += prod.Weight * 1.5M;
                }
            }
            return sum;
        }
        public string GetShippingCostToString()
        {
            return $"{GetShippingCost():C}";
        }
    }
}
