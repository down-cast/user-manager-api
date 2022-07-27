using System.Security.Cryptography;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;

namespace Downcast.UserManager.PasswordManager
{
    public class PasswordHashing : IPasswordManager
    {
        private readonly HashingOptions _hashingOptions;
        private int SaltSize { get; set; } = 16; // 128 bit 
        private int HashSize { get; set; } = 32; // 256 bit

        public PasswordHashing(IOptions<HashingOptions> hashingOptions)
        {
            _hashingOptions = hashingOptions.Value;
        }

        public PasswordInfo HashPassword(string password)
        {
            byte[] passwordSalt = new byte[SaltSize];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(passwordSalt);
            var a = _hashingOptions.Iterations;
            byte[] passwordHash = KeyDerivation.Pbkdf2(
            password: password,
            salt: passwordSalt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: _hashingOptions.Iterations,
            numBytesRequested: HashSize);

            return new PasswordInfo
            {
                PasswordHash = Convert.ToBase64String(passwordHash),
                PasswordSalt = Convert.ToBase64String(passwordSalt),
                Iterations = _hashingOptions.Iterations
            };
        }

        public (bool verified, bool upgradeNeeded) CheckPasswordHash(string password, PasswordInfo passwordInfo)
        {
            if (passwordInfo.Iterations != _hashingOptions.Iterations)
            {
                bool userVerified = AuthenticateWithOldIterations(password, passwordInfo);
                return (userVerified, true);
            }

            byte[] computedHash = ComputeHash(password, passwordInfo.PasswordSalt, _hashingOptions.Iterations);
            return (computedHash.SequenceEqual(Convert.FromBase64String(passwordInfo.PasswordHash)), false);
        }

        private bool AuthenticateWithOldIterations(string password, PasswordInfo passwordInfo)
        {
            byte[] computedHash = ComputeHash(password, passwordInfo.PasswordSalt, passwordInfo.Iterations);
            return (computedHash.SequenceEqual(Convert.FromBase64String(passwordInfo.PasswordHash)));
        }

        private byte[] ComputeHash(string password, string storedSalt, int iterations)
        {
            return KeyDerivation.Pbkdf2(
              password,
              Convert.FromBase64String(storedSalt),
              KeyDerivationPrf.HMACSHA256,
              iterations,
              HashSize);
        }
    }
}
