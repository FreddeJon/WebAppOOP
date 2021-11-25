using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppOOP.Core.ModelDTOS.Interfaces;

namespace WebAppOOP.Core.ModelDTOS
{
    public class Order : IEntity
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsPaid { get; set; }
        public string RecieptKey { get; set; }
    }
}
