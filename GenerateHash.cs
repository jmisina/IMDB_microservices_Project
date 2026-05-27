using System;
using System.Security.Cryptography;

namespace UsersAPI.Security
{
    public sealed class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100000;
        private readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;

        public string Hash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);
            return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
        }
    }
}

public class Program
{
    public static void Main()
    {
        var hasher = new UsersAPI.Security.PasswordHasher();
        var password = "AdminPassword123!";
        Console.WriteLine(hasher.Hash(password));
    }
}
