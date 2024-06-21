
using backend.Dto;
using backend.Dto.Auth;
using backend.Dto.Store;
using backend.Jwt;

namespace backend.Interface
{
    public interface IAuthentication
    {
        Task<JwtAuthenticationResponse?> Login(LoginDto loginDto);
        Task<JwtAuthenticationResponse?> LoginAdmin(LoginDto loginDto);
        Task<ApiObject> Logout();
        Task<object> Profile();
        Task<NewUserDto> Register(RegisterDto registerDto);
        Task<ApiObject?> Switch(ModeDto mode);
    }
}