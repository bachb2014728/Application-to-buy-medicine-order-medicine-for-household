
using backend.Data;
using backend.Dto.Auth;
using backend.Error;
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
        ApplicationDBContext context) : IAuthentication
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IJwtService _jwtService = jwtService;
        private readonly SignInManager<AppUser> _signinManager = signInManager;
        private readonly ApplicationDBContext _context = context;

        public async Task<JwtAuthenticationResponse?> Login(LoginDto loginDto)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.UserName == loginDto.Username!.ToLower());

            if (user == null) {
                //TODO:
                throw new ApplicationException(loginDto.Username +" không tồn tại!");
                // throw new UsernameNotFound(loginDto.Username +" không tồn tại!");
            
            }
            var result = await _signinManager.CheckPasswordSignInAsync(user, loginDto.Password!, false);

            if (!result.Succeeded){
                 //TODO:
                throw new ApplicationException("username hoặc mật khẩu không đúng");
            }
            var newToken = _jwtService.CreateToken(user);
            var token = await _context.Tokens.FirstOrDefaultAsync(x => x.AppUserId == user.Id);
            token!.Code = newToken;
            await _context.SaveChangesAsync();

            return new JwtAuthenticationResponse {
                ActivateBy = user?.ActivateBy,
                Token = newToken,
            };
        }

        public async Task<object> Profile(string token)
        {
            var username = _jwtService.GetUsernameFromToken(token) ?? throw new ApplicationException(token + " không đúng");
            var user = _userManager.Users.FirstOrDefault(x => x.UserName == username!.ToLower()) ?? throw new ApplicationException("Không tìm thấy người dùng");
            if (user?.ActivateBy == "CUSTOMER") {
                var customer = await _context.Customers.FirstOrDefaultAsync(x => x.User == user);
                var profile = new CustomerDto {
                    FirstName = customer?.FirstName,
                    LastName = customer?.LastName,
                    Address = customer?.Address,
                    Birthday = customer?.Birthday,
                    Gender = customer?.Gender,
                    PhoneNumber = user?.PhoneNumber,
                    Username = username,
                    Email = user?.Email,
                };
                return profile;
            }else{
                var store = _context.Stores.FirstOrDefault(x => x.AppUser == user);
                var profile = new StoreDto {
                    Name = store?.Name,
                    PhoneNumber = user?.PhoneNumber,
                    Username = user?.UserName,
                    Email = user?.Email,
                };
                return profile;
            }
        }

        public async Task<NewUserDto> Register(RegisterDto registerDto)
        {
            var existingUserWithSameEmail = await _userManager.FindByEmailAsync(registerDto.Email!);
            Console.WriteLine(existingUserWithSameEmail);
            if (existingUserWithSameEmail != null)
            {
                throw new EmailAlreadyExistsException(registerDto.Email);
            }

            var existingUserWithSameUsername = await _userManager.FindByNameAsync(registerDto.Username!);
            if (existingUserWithSameUsername != null)
            {
                throw new AlreadyExistsException($"Username {registerDto.Username!} đã tồn tại.");
            }
            var appUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email,
                    PhoneNumber = registerDto.Phone,
                    ActivateBy = "CUSTOMER"

                };

            var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password!);

            if (createdUser.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                if (roleResult.Succeeded)
                {
                    var token =  _jwtService.CreateToken(appUser);
                    var userNow = _userManager.Users.FirstOrDefault(item => item.UserName == registerDto.Username!.ToLower());
                    var tokenNew = new Token{
                        Code = token,
                        ExpiredAt = DateTime.UtcNow.AddDays(1),
                        AppUserId = userNow?.Id
                    };
                    await _context.Tokens.AddAsync(tokenNew);
                    await _context.SaveChangesAsync();

                    var customer = new Customer {
                        FirstName = registerDto.FirstName,
                        LastName = registerDto.LastName,
                        Address = registerDto.Address,
                        User = userNow

                    };
                    await _context.Customers.AddAsync(customer);
                    await _context.SaveChangesAsync();
                    return new NewUserDto
                        {
                            UserName = appUser.UserName!,
                            Email = appUser.Email!,
                            Token = token
                        };
                }
                else
                {
                    //TODO:
                    throw new ApplicationException(roleResult.Errors.ToString());
                }
            }
            else
            {
                //TODO:
                throw new ApplicationException(createdUser.Errors.ToString());
            }
        }


        public async Task<SwitchDto?> Switch(string token)
        {
            var username = _jwtService.GetUsernameFromToken(token) ?? throw new ApplicationException(token + " không đúng");
            var user = _userManager.Users.FirstOrDefault(x => x.UserName == username!.ToLower()) ?? throw new ApplicationException("Không tìm thấy người dùng");

            var existingStore = await _context.Stores.FirstOrDefaultAsync(x => x.AppUser == user);
            if(existingStore == null){
                throw new StoreNotFoundException("Không có tài khoản nhà thuốc");
            }
            if(existingStore.Status != "ACTIVE"){
                throw new Exception("Tài khoản nhà thuốc đang đợi xét duyệt hoặc đã bị cấm");
            }
            user.ActivateBy = user.ActivateBy == "CUSTOMER" ? "STORE" : "CUSTOMER";

            var updateUser = await _userManager.UpdateAsync(user);

            var switchItem = new SwitchDto
            {
                Message = user.ActivateBy == "STORE" ? "Chuyển đổi tài khoản người dùng sang tài khoản nhà thuốc thành công !" 
                : "Chuyển đổi tài khoản nhà thuốc sang tài khoản người dùng thành công",
            };

            return switchItem;
        }

        public async Task<StoreDto> Upgrade(NewStoreDto newStoreDto, string token)
        {
            var username = _jwtService.GetUsernameFromToken(token) ?? throw new ApplicationException(token + " không đúng");
            var user = _userManager.Users.FirstOrDefault(x => x.UserName == username!.ToLower()) ?? throw new ApplicationException("Không tìm thấy người dùng");
            var existingStore = await _context.Stores.FirstOrDefaultAsync(x => x.AppUser == user);
            if(existingStore != null){
                throw new AlreadyExistsException("Đã tồn tại nhà thuốc");
            }
            var store = new Store {
                Name = newStoreDto.Name!,
                Info = newStoreDto.Info!,
                Status = "AWAIT",
                AppUserId = user.Id,
            };
            await _context.Stores.AddAsync(store);
            await _context.SaveChangesAsync();
            
            var newStore = new StoreDto {
                Name = store.Name,
                Info = store.Info,
                Username = username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };
            return newStore;
        }
    }
}