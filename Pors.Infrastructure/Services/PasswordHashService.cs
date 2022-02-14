using System;
using System.Security.Cryptography;
using Pors.Application.Common.Interfaces;

namespace Pors.Infrastructure.Services
{
    internal class PasswordHashService : IPasswordHashService
    {
        private readonly int SaltSize = 16;
        private readonly int Iterations = 10000;

        public string Hash(string value)
        {
            var hasher = new Rfc2898DeriveBytes(value, SaltSize, Iterations);

            var hashString = GenerateHashString(hasher);

            return hashString;
        }

        public bool Verify(string providedValue, string hashedValue)
        {
            var saltBytes = Convert.FromBase64String(hashedValue).Take(SaltSize).ToArray();

            var hasher = new Rfc2898DeriveBytes(providedValue, saltBytes, Iterations);

            var hashString = GenerateHashString(hasher);

            return hashString == hashedValue;
        }

        private string GenerateHashString(Rfc2898DeriveBytes hasher)
        {
            var hashedValue = hasher.GetBytes(SaltSize);
            var output = hasher.Salt.Concat(hashedValue).ToArray();

            return Convert.ToBase64String(output);
        }
    }
}
