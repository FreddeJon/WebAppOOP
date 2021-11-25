using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppOOP.Core.ModelDTOS.Interfaces;

namespace WebAppOOP.Core.ModelDTOS
{
    public class Reciept : IEntity
    {
        public Card Card { get; set; }
        public int Id { get; set; }
        public string Key { get; set; }
        public List<string> LicenceKeys { get; set; }
        public ShoppingCart Cart { get; set; }
        public DateTime PayDate { get; set; }
    }
}
