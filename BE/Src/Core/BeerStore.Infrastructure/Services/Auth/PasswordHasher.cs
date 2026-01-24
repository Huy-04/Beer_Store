using BeerStore.Application.Interface.Services;

namespace BeerStore.Infrastructure.Services.Auth
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int BcryptWorkFactor = 12;

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BcryptWorkFactor);
        }

        public bool VerifyPassword(string password, string hash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch
            {
                return false;
            }
        }
    }
}
