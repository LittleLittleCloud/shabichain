using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShabiChain.Core
{
    public static class Utils
    {
        public static string ComputeHash(string str)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(str));
                return Encoding.UTF8.GetString(bytes);
            }
        }
    }
}
