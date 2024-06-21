using backend.Dto.Auth;
using backend.Dto.Store;
using backend.Helper;
using backend.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController(IAuthentication authentication, ConvertInformation convert) : ControllerBase
    {
        private readonly IAuthentication _authentication = authentication;
        private readonly ConvertInformation _convert = convert;

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var user = await _authentication.Register(registerDto);
            return Ok(user);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var response = await _authentication.Login(loginDto);

            return Ok(response);
        }
        [HttpPost("loginAdmin")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAdmin(LoginDto loginDto)
        {
            if (!ModelState.IsValid){return BadRequest(ModelState);}
            var response = await _authentication.LoginAdmin(loginDto);

            return Ok(response);
        }
        [HttpPost("switch")]
        [Authorize]
        public async Task<IActionResult> Switch([FromBody] ModeDto mode)
        {
            if (!ModelState.IsValid){return BadRequest(ModelState);}
            var response = await _authentication.Switch(mode);
            return Ok(response);
        }

        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> Logout(){
            if (!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var response = await _authentication.Logout();
            return Ok(response);
        }
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> Profile(){
            if (!ModelState.IsValid){
                    return BadRequest(ModelState);
            }
        
            var response = await _authentication.Profile();
            return response switch
            {
                CustomerDto customer => Ok(customer),
                StoreDto store => Ok(store),
                AdminDto admin => Ok(admin),
                _ => BadRequest("Không tìm thế kiểu DTO")
            };
        }
    }
}