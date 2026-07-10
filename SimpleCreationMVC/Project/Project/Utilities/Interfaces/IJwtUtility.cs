
using Models;

namespace Utilities.Interfaces
{
    public interface IJwtUtility
    {
        string GenerateToken(int userId);
    }
}
