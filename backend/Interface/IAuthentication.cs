
using backend.Dto.Auth;
using backend.Jwt;

namespace backend.Interface
{
    public interface IAuthentication
    {
        Task<JwtAuthenticationResponse?> Login(LoginDto loginDto);
        Task<object> Profile(string token);
        Task<NewUserDto> Register(RegisterDto registerDto);
        Task<SwitchDto?> Switch(string token);
        Task<StoreDto> Upgrade(NewStoreDto newStoreDto,string token);
    }
}