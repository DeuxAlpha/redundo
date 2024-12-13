using System;
using System.Security.Cryptography;

namespace Common
{
    public static class Randomizer
    {
        public static string GetRandomBase64String()
        {
            var randomNumber = new byte[32];
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}