using backend.Model;

namespace backend.Jwt
{
    public interface IJwtService
    {
        string CreateToken(AppUser user);
        string GetUsernameFromToken(string token);
        bool IsTokenExpired(string token);
    }
}