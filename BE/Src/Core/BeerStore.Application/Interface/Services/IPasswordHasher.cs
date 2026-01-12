namespace BeerStore.Application.Interface.Services
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);

        bool VerifyPassword(string password, string hash);
    }
}
