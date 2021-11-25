using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppOOP.Core.ModelDTOS.ProductDTOs
{
    public class ProductDTO
    {
        public List<DigitalProduct> DigitalProducts { get; set; } = new List<DigitalProduct>();
        public List<PhysicalProduct> PhysicalProducts { get; set; } = new List<PhysicalProduct>();
    }
}
