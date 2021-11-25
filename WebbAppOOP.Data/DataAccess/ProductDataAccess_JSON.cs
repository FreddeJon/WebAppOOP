        using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebAppOOP.Core.ModelDTOS.ProductDTOs;
using WebbAppOOP.Data.DataAccess.Interfaces;

namespace WebbAppOOP.Data.DataAccess
{
    public class ProductDataAccess_JSON : IProductDataAccess
    {
        private readonly IConfiguration _configuration;
        private readonly string _path;

        public ProductDataAccess_JSON(IConfiguration configuration)
        {
            _configuration = configuration;
            _path = _configuration["ProductPath"];
        }

        public List<Product> GetAll() => Read().OrderBy(x => x.Id).ToList();


        public Product GetById(int id) => Read().FirstOrDefault(x => x.Id == id);



        public int GetNewID()
        {
            var tmp = Read();
            if (tmp?.Count > 0)
            {
                return tmp.Max(x => x.Id) + 1;
            }
            return 1;
        }


        public void Remove(Product item)
        {
            var list = Read();
            list.RemoveAll(x => x.Id == item.Id);
            Write(list);
        }

        public void Update(Product item)
        {
            var list = Read();
            var itemToUpdate = list.FirstOrDefault(x => x.Id == item.Id);
            if (itemToUpdate is not null)
            {
                list.Remove(itemToUpdate);
                list.Add(item);
                Write(list);
            }
        }

        private void Write(List<Product> product)
        {
            var physical = product.OfType<PhysicalProduct>().ToList();
            var digital = product.OfType<DigitalProduct>().ToList();

            var list = new ProductDTO
            {
                DigitalProducts = digital,
                PhysicalProducts = physical
            };

            var jsonString = JsonConvert.SerializeObject(list);

            File.WriteAllText(_path, jsonString);
        }
        private List<Product> Read()
        {
            try
            {
                var jsonResponse = File.ReadAllText(_path);
                var jsonObject = JsonConvert.DeserializeObject<ProductDTO>(jsonResponse);

                if (jsonObject is not null)
                {
                    var list = new List<Product>();
                    list.AddRange(jsonObject.DigitalProducts);
                    list.AddRange(jsonObject.PhysicalProducts);
                    return list;
                }
                return new List<Product>();

            }
            catch (Exception)
            {
                return new List<Product>();

            }

        }
    }
}
