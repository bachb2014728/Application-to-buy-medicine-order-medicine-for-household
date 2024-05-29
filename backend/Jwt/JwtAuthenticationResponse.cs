
namespace backend.Jwt
{
    public class JwtAuthenticationResponse
    {
        public string? ActivateBy { get; set; }
        public required string Token { get; set; }
        
    }
}