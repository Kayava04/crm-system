using CRMSystem.WebAPI.Models;

namespace CRMSystem.WebAPI.Auth
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}