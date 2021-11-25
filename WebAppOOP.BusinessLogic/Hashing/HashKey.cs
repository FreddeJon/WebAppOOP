using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebAppOOP.BusinessLogic.Hashing.Interface;

namespace WebAppOOP.BusinessLogic.Hashing
{
    public class HashKey : IHash
    {
        public string GenerateSalt()
        {
            var criptografer = new RNGCryptoServiceProvider();
            var buffer = new byte[10];
            criptografer.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }

        public string HashString(string password ="", string salt = "")
        {
            var shaHasherToString = new SHA256Managed();
            byte[] bytes = Encoding.UTF8.GetBytes(password + salt);
            byte[] hash = shaHasherToString.ComputeHash(bytes);
            string computerHash = Convert.ToBase64String(hash);

            return computerHash;
        }
    }
}
