using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Diplom.Classes
{
    internal class PasswordHasher
    {
        public static string PasswordHash(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashPassword = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashPassword);
            }
        }

        public static bool VeryfyPassword(string password, string hashPassword)
        {
            var hash = PasswordHash(password);
            return hash == hashPassword;
        }
    }
}
