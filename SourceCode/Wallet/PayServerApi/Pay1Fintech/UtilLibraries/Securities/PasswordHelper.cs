using System;
using System.Security.Cryptography;
using System.Text;

namespace UtilLibraries.Securities
{
    public class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            // SHA512 is disposable by inheritance.  
            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                // Get the hashed string.  
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
