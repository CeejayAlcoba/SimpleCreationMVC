
namespace Utilities.Interfaces
{
    public interface IEncryptUtility
    {
        string GenerateRandomSalt();
        string GenerateHashedPassword(string password, string salt);
    }
}
