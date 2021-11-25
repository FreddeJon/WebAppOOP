using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebAppOOP.Core.ModelDTOS.ProductDTOs.Interfaces;

namespace WebAppOOP.Core.ModelDTOS.ProductDTOs
{
    public class DigitalProduct : Product, IDigitalProduct
    {

        public string GenerateLicenceKey()
        {
            var criptografer = new RNGCryptoServiceProvider();
            var buffer = new byte[10];
            criptografer.GetBytes(buffer);
            var salt = Convert.ToBase64String(buffer);

            var shaHasherToString = new SHA256Managed();
            byte[] bytes = Encoding.UTF8.GetBytes(salt);
            byte[] hash = shaHasherToString.ComputeHash(bytes);
            string computerHash = Convert.ToBase64String(hash);

            return $"Product: {Name}, Key: {computerHash}";
        }
    }

}

