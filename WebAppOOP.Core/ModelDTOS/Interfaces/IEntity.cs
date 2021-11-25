using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppOOP.Core.ModelDTOS.Interfaces
{
    public interface IEntity : IKey
    {
        int Id { get; set; }
    }
}
