
using backend.Dto;
using backend.Dto.Auth;
using backend.Dto.Customer;
using backend.Jwt;

namespace backend.Interface
{
    public interface IAuthentication
    {
        Task<JwtAuthenticationResponse?> Login(LoginDto loginDto);
        Task<JwtAuthenticationResponse?> LoginAdmin(LoginDto loginDto);
        Task<ApiObject> Logout();
        Task<CustomerDto> Profile(); 
        Task<NewUserDto> Register(RegisterDto registerDto);
        Task<AdminDto?> ProfileOfAdmin();
    }
}