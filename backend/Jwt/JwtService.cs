using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Data;
using backend.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace backend.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        private readonly SymmetricSecurityKey _key;
        private readonly ApplicationDBContext _context;
        public JwtService(IConfiguration configuration, UserManager<AppUser> userManager,ApplicationDBContext context)
        {
            _configuration = configuration;
            _userManager = userManager;
            _context = context;
            _key =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]!));
        }

        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Email, user.Email!),
                new(JwtRegisteredClaimNames.GivenName, user.UserName!)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds,
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public bool IsValidToken(string token, AppUser user)
        {
            throw new NotImplementedException();
        }

        public string GetUsernameFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Lấy username từ claims
            var usernameClaim = jwtToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.GivenName);
            var username = usernameClaim.Value;

            return username;
        }
        public bool IsTokenExpired(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Lấy thời gian hết hạn từ token
            var expiryDateUnix = jwtToken.Payload.Exp.Value;

            // Chuyển đổi thời gian hết hạn từ dạng Unix timestamp sang DateTime
            var expiryDate = DateTimeOffset.FromUnixTimeSeconds(expiryDateUnix).UtcDateTime;

            // So sánh thời gian hết hạn với thời gian hiện tại
            return expiryDate < DateTime.UtcNow;
        }


    }
}