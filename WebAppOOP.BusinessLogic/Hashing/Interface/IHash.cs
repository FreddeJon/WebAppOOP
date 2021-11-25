using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppOOP.BusinessLogic.Hashing.Interface
{
    public interface IHash
    {
        string GenerateSalt();
        string HashString(string password = "", string salt = "");
    }
}
