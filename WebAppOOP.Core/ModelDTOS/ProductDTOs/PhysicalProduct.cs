using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppOOP.Core.ModelDTOS.ProductDTOs.Interfaces;

namespace WebAppOOP.Core.ModelDTOS.ProductDTOs
{
    public class PhysicalProduct : Product, IPhysicalPoduct
    {
        public decimal Weight { get; set; }
    }
}
