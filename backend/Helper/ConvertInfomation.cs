
using System.IdentityModel.Tokens.Jwt;
using backend.Data;
using backend.Error;
using backend.Jwt;
using backend.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend.Helper
{
    public class ConvertInformation(UserManager<AppUser> userManager,IJwtService jwtService,ApplicationDbContext context)
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IJwtService _jwtService = jwtService;
        private readonly ApplicationDbContext _context = context;
        public AppUser ToAppUser(string token){
            var username = _jwtService.GetUsernameFromToken(token) ?? throw new NotFoundException(token + " không đúng");
            var user =  _userManager.Users.FirstOrDefault(x => x.UserName == username!.ToLower()) ?? throw new NotFoundException("Không tìm thấy người dùng");
            return user;
        }
        public string ToRole(string token){
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var userRole = jwtToken.Claims.First(c => c.Type == "role").Value;
            return userRole;
        }

        public async Task<Customer> ToCustomerFormUser(string token)
        {
            var user = ToAppUser(token);
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.User == user);
            if (customer == null)
            {
                throw new NotFoundException("Không tìm thấy dữ liệu người dùng.");
            }

            return customer;
        }
    }
}