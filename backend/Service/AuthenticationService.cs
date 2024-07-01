
using backend.Data;
using backend.Dto;
using backend.Dto.Auth;
using backend.Dto.Customer;
using backend.Error;
using backend.Helper;
using backend.Interface;
using backend.Jwt;
using backend.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend.Service
{
    public class AuthenticationService(
        UserManager<AppUser> userManager,
        IJwtService jwtService, SignInManager<AppUser> signInManager,
        ApplicationDbContext context, ConvertInformation convert) : IAuthentication
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IJwtService _jwtService = jwtService;
        private readonly SignInManager<AppUser> _signinManager = signInManager;
        private readonly ApplicationDbContext _context = context;
        private readonly ConvertInformation _convert = convert;

        public async Task<JwtAuthenticationResponse?> Login(LoginDto loginDto)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.UserName == loginDto.Username.ToLower()) ?? throw new NotFoundException(loginDto.Username + " không tồn tại!");
            var result = await _signinManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                throw new NotFoundException("Username hoặc mật khẩu không đúng");
            }
            var newToken = _jwtService.CreateToken(user);
            var tokens = await _context.Tokens.Where(x => x.IsActive == true && x.AppUserId == user.Id).ToListAsync();
            foreach (var item in tokens)
            {
                item.IsActive = false;
                item.LogoutAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            var token = new Token
            {
                Code = newToken,
                IsActive = true,
                AppUserId = user.Id,
            };
            await _context.Tokens.AddAsync(token);
            await _context.SaveChangesAsync();

            return new JwtAuthenticationResponse
            {
                Token = newToken
            };
        }

        public async Task<JwtAuthenticationResponse?> LoginAdmin(LoginDto loginDto)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.UserName == loginDto.Username.ToLower()) ?? throw new NotFoundException(loginDto.Username + " không tồn tại!");
            var result = await _signinManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                throw new NotFoundException("Username hoặc mật khẩu không đúng");
            }
            var userRole =await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (userRole == null)
            {
                throw new NotFoundException("Không tìm thấy vai trò người dùng.");
            }
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == userRole.RoleId);
            if (role == null)
            {
                throw new NotFoundException("Không tìm thấy quyền trong dữ liệu.");
            }
            if (role.NormalizedName.Equals("ADMIN"))
            {
                var newToken = _jwtService.CreateToken(user);
                var tokens = await _context.Tokens.Where(x => x.IsActive == true && x.AppUserId == user.Id).ToListAsync();
                if(tokens.Count > 0){
                    foreach (var item in tokens)
                    {
                        item.IsActive = false;
                        item.LogoutAt = DateTime.UtcNow;
                        await _context.SaveChangesAsync();
                    }
                }
                var token = new Token
                {
                    Code = newToken,
                    IsActive = true,
                    AppUserId = user.Id,
                };
                await _context.Tokens.AddAsync(token);
                await _context.SaveChangesAsync();

                return new JwtAuthenticationResponse
                {
                    Token = newToken,
                };
            }
            throw new RoleNotFoundException($"{role} :Tài khoản không có quyền quản trị.");
        }

        public async Task<ApiObject> Logout()
        {
            var tokens = await _context.Tokens.Where(x=>x.Code == GlobalVariables.Token && x.IsActive ==true).ToListAsync();
            if (tokens.Count <= 0)
                return new ApiObject
                {
                    Message = "Đăng xuất thành công."
                };
            foreach (var item in tokens)
            {
                item.IsActive = false;
                item.LogoutAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            return new ApiObject
            {
                Message = "Đăng xuất thành công."
            };

        }

        public async Task<CustomerDto> Profile()
        {
            var user = _convert.ToAppUser(GlobalVariables.Token);
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.User == user);
            
            var profile = new CustomerDto
            {
                Id  = customer.Id,
                FirstName = customer?.FirstName,
                LastName = customer?.LastName,
                Address = customer?.Address,
                ZipCode = customer?.ZipCode,
                Gender = customer?.Gender,
                PhoneNumber = user.PhoneNumber,
                Username = user.UserName,
                Email = user.Email
            };
            return profile;
        }

        public async Task<NewUserDto> Register(RegisterDto registerDto)
        {
            var existingUserWithSameEmail = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUserWithSameEmail != null)
            {
                throw new EmailAlreadyExistsException(registerDto.Email);
            }

            var existingUserWithSameUsername = await _userManager.FindByNameAsync(registerDto.Username);
            if (existingUserWithSameUsername != null)
            {
                throw new AlreadyExistsException($"Username {registerDto.Username} đã tồn tại.");
            }
            var existingUserWithSamePhone = await _userManager.Users
                .FirstOrDefaultAsync(x=>x.PhoneNumber == registerDto.Phone);
            if (existingUserWithSamePhone != null)
            {
                throw new AlreadyExistsException($"Username {registerDto.Phone} đã tồn tại.");
            }
            var appUser = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                PhoneNumber = registerDto.Phone

            };

            var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

            if (!createdUser.Succeeded) throw new ApplicationException(createdUser.Errors.ToString());
            var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
            if (!roleResult.Succeeded) throw new ApplicationException(roleResult.Errors.ToString());
            var token = _jwtService.CreateToken(appUser);
            var userNow = _userManager.Users.FirstOrDefault(item => item.UserName == registerDto.Username.ToLower());
            var tokenNew = new Token
            {
                Code = token,
                IsActive = true,
                AppUserId = userNow?.Id
            };
            await _context.Tokens.AddAsync(tokenNew);
            var customer = new Customer
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Address = registerDto.Address,
                User = userNow
            };
            await _context.Customers.AddAsync(customer);
            var receiver = new Receiver
            {
                Name = registerDto.FirstName + " " + registerDto.LastName,
                Phone = registerDto.Phone,
                Address = registerDto.Address,
                Customer = customer
            };
            await _context.Receivers.AddAsync(receiver);
            await _context.SaveChangesAsync();
            return new NewUserDto
            {
                UserName = appUser.UserName,
                Email = appUser.Email,
                Token = token
            };
        }

        public async Task<AdminDto?> ProfileOfAdmin()
        {
            var user = _convert.ToAppUser(GlobalVariables.Token);
            
            return new AdminDto()
            {
                Username = user.UserName,
                Email = user.Email
            };
        }
    }
}