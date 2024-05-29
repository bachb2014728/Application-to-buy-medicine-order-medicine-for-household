using backend.Dto.Auth;
using backend.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController(IAuthentication authentication) : ControllerBase
    {
        private readonly IAuthentication _authentication = authentication;

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
        [HttpPost("upgrade")]
        [Authorize]
        public async Task<IActionResult> Upgrade([FromBody] NewStoreDto newStoreDto){
            if (!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var authHeader = HttpContext.Request.Headers.Authorization.ToString();

            var token = authHeader["Bearer ".Length..].Trim();
            var response = await _authentication.Upgrade(newStoreDto,token);
            return Ok(response);
        }

        [HttpGet("switch")]
        [Authorize]
        public async Task<IActionResult> Switch()
        {
            if (!ModelState.IsValid){
                    return BadRequest(ModelState);
                }
            var authHeader = HttpContext.Request.Headers.Authorization.ToString();

            var token = authHeader["Bearer ".Length..].Trim();
            var response = await _authentication.Switch(token);

            return Ok(response);
        }
        
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> Profile(){
            if (!ModelState.IsValid){
                    return BadRequest(ModelState);
            }
            var authHeader = HttpContext.Request.Headers.Authorization.ToString();

            var token = authHeader["Bearer ".Length..].Trim();
            var response = await _authentication.Profile(token);

            if (response is StoreDto store)
            {
                return Ok(store);
            }
            else if (response is CustomerDto customer)
            {
                return Ok(customer);
            }
            else
            {
                return BadRequest("Không tìm thế kiểu DTO");
            }
        }
    }
}