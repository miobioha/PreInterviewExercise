using System;
using System.Security.Cryptography;
using System.Text;

namespace Banking.Core.Services
{
    public class Sha256HashingService : IHashingService
    {
        public string Hash(string value)
        {
            SHA256 sha256 = SHA256.Create();
            return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(value)));
        }
    }
}
