
using backend.Data;
using backend.Dto;
using backend.Dto.Auth;
using backend.Dto.Cart;
using backend.Dto.Product;
using backend.Dto.Store;
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
            var user = _userManager.Users.FirstOrDefault(x => x.UserName == loginDto.Username!.ToLower()) ?? throw new NotFoundException(loginDto.Username + " không tồn tại!");
            var result = await _signinManager.CheckPasswordSignInAsync(user, loginDto.Password!, false);
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
                LoginOn = DateTime.UtcNow,
                IsActive = true,
                AppUserId = user.Id,
            };
            await _context.Tokens.AddAsync(token);
            await _context.SaveChangesAsync();

            return new JwtAuthenticationResponse
            {
                ActivateBy = (user?.ActivateBy) ?? "ADMIN",
                Token = newToken,
            };
        }

        public async Task<JwtAuthenticationResponse?> LoginAdmin(LoginDto loginDto)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.UserName == loginDto.Username!.ToLower()) ?? throw new NotFoundException(loginDto.Username + " không tồn tại!");
            var result = await _signinManager.CheckPasswordSignInAsync(user, loginDto.Password!, false);
            if (!result.Succeeded)
            {
                //TODO:
                throw new NotFoundException("Username hoặc mật khẩu không đúng");
            }
            var userRole = _context.UserRoles.FirstOrDefault(x => x.UserId == user.Id);
            var role = _context.Roles.FirstOrDefault(x => x.Id == userRole!.RoleId);
            if (role!.ToString().ToUpper().Equals("ADMIN"))
            {
                var newToken = _jwtService.CreateToken(user);
                var tokens = await _context.Tokens.Where(x => x.IsActive == true && x.AppUserId == user.Id).ToListAsync();
                if(tokens.Count > 0){
                    foreach (var item in tokens)
                    {
                        item.IsActive = false;
                        item.LogoutAt = DateTime.UtcNow;
                        _context.SaveChanges();
                    }
                }
                var token = new Token
                {
                    Code = newToken,
                    LoginOn = DateTime.UtcNow,
                    IsActive = true,
                    AppUserId = user.Id,
                };
                await _context.Tokens.AddAsync(token);
                await _context.SaveChangesAsync();

                return new JwtAuthenticationResponse
                {
                    ActivateBy = role.ToString().ToUpper(),
                    Token = newToken,
                };
            }
            else
            {
                throw new RoleNotFoundException($"{role} :Tài khoản không có quyền quản trị.");
            }
        }

        public async Task<ApiObject> Logout()
        {
            var tokens = await _context.Tokens.Where(x=>x.Code == GlobalVariables.Token && x.IsActive ==true).ToListAsync();
            if(tokens.Count > 0){
                    foreach (var item in tokens)
                    {
                        item.IsActive = false;
                        item.LogoutAt = DateTime.UtcNow;
                        await _context.SaveChangesAsync();
                    }
                }
            return new ApiObject
            {
                Message = "Đăng xuất thành công."
            };

        }

        public async Task<object> Profile()
        {
            var user = _convert.ToAppUser(GlobalVariables.Token);
            switch (user?.ActivateBy)
            {
                case "CUSTOMER":
                {
                    var customer = await _context.Customers.FirstOrDefaultAsync(x => x.User == user);
                    var listStore = await _context.Stores.Where(x => x.CreatedBy == user).ToArrayAsync();
                    List<StoreItem> stores = [];
                    stores.AddRange(listStore.Select(ToStoreItem));

                    var listCart = await _context.Carts
                        .Where(x => customer != null && x.CustomerId == customer.Id).ToListAsync();
                    List<CartItem> carts = [];
                    foreach (var item in listCart)
                    {
                        var cartItem = await ToCartItem(item);
                        carts.Add(cartItem);
                    }
                    var profile = new CustomerDto
                    {
                        FirstName = customer?.FirstName,
                        LastName = customer?.LastName,
                        Address = customer?.Address,
                        Birthday = customer?.Birthday,
                        Gender = customer?.Gender,
                        PhoneNumber = user?.PhoneNumber,
                        Username = user?.UserName,
                        Email = user?.Email,
                        RoleName = user?.ActivateBy,
                        Carts = carts,
                        Stores = stores,
                    };
                    return profile;
                }
                case "STORE":
                {
                    var store = _context.Stores.Include(x=>x.Avatar).Include(x=>x.Background).Where(x=>x.Mode == true).FirstOrDefault(x => x.CreatedBy == user);
                
                    var storeDto = ToStoreDto(store);
                    storeDto.RoleName = user?.ActivateBy;
                    return storeDto;
                }
                default:
                {
                    var profile = new AdminDto
                    {
                        Username = user?.UserName,
                        Email = user?.Email
                    };
                    return profile;
                }
            }
        }
        private static StoreItem ToStoreItem(Store? store)
        {
            return new StoreItem()
            {
                Id = store.Id,
                Name = store.Name,
                URL = store.URL,
                Avatar = store.Avatar?.Id
            };
        }

        private async Task<CartItem> ToCartItem(Cart cart)
        {
            var product = await _context.Products.Include(x => x.CreatedBy)
                .FirstOrDefaultAsync(x => x.Id == cart.ProductId);
            if (product == null)
            {
                throw new NotFoundException("Không tìm thấy sản phẩm.");
            }
            var productItem = new ProductItem
            {
                Id = product.Id,
                Name = product.Name,
                URL = product.URL,
                Quantity = product.Quantity,
                Price = product.Price,
                Sale = product.Sale,
                CreatedBy = ToStoreItem(product.CreatedBy)
            };
            return new CartItem()
            {
                Id = cart.Id,
                Product = productItem,
                CreatedOn = cart.CreatedOn,
                Quantity = cart.Quantity
            };
        }

        private static StoreDto ToStoreDto(Store store){
            return new StoreDto{
                Id = store.Id,
                Name = store.Name,
                URL = store.URL,
                Avatar = store.Avatar?.Id,
                Background = store.Background?.Id,
                Address = store.Address,
                Info = store.Info,
                Followers = [],
                Status = store.Status,
                Star =store.Star,
                CreatedAt = store.CreatedAt
            };
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
                    var token = _jwtService.CreateToken(appUser);
                    var userNow = _userManager.Users.FirstOrDefault(item => item.UserName == registerDto.Username!.ToLower());
                    var tokenNew = new Token
                    {
                        Code = token,
                        IsActive = true,
                        LoginOn = DateTime.UtcNow,
                        AppUserId = userNow?.Id
                    };
                    await _context.Tokens.AddAsync(tokenNew);
                    await _context.SaveChangesAsync();

                    var customer = new Customer
                    {
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


        public async Task<ApiObject?> Switch(ModeDto mode)
        {
            var user = _convert.ToAppUser(GlobalVariables.Token);
            var result = await _signinManager.CheckPasswordSignInAsync(user, mode.Password!, false);
            if (!result.Succeeded)
            {
                throw new NotFoundException("Mật khẩu không đúng");
            }
            if(user.ActivateBy == "CUSTOMER" && mode.Type!.ToUpper() == "STORE"){
                var store = await _context.Stores.FirstOrDefaultAsync(x=>x.Id == mode.Account) ?? throw new StoreNotFoundException("Không có tài khoản nhà thuốc");
                if (store.Status != "ACTIVE")
                {
                    throw new Exception("Tài khoản nhà thuốc đang đợi xét duyệt hoặc đã bị cấm");
                }
                user.ActivateBy = "STORE";
                var listItem = await _context.Stores.Where(x=>x.CreatedBy == user && x !=store).ToArrayAsync();
                foreach(var item in listItem){
                    item.Mode = false;
                    await _context.SaveChangesAsync();
                }
                store.Mode = true;
                await _context.SaveChangesAsync();
                return new ApiObject{
                    Message = "Chuyển đổi tài khoản thành công."
                };
            }else if(user.ActivateBy == "STORE" && mode.Type!.ToUpper() == "STORE"){
                var store = await _context.Stores.Where(x=>x.Id == mode.Account).FirstOrDefaultAsync(x => x.CreatedBy == user) ?? throw new StoreNotFoundException("Không có tài khoản nhà thuốc");
                if (store.Status != "ACTIVE")
                {
                    throw new Exception("Tài khoản nhà thuốc đang đợi xét duyệt hoặc đã bị cấm");
                }
                var listItem = await _context.Stores.Where(x=>x.CreatedBy == user && x !=store).ToArrayAsync();
                foreach(var item in listItem){
                    item.Mode = false;
                    await _context.SaveChangesAsync();
                }
                store.Mode = true;
                await _context.SaveChangesAsync();
                return new ApiObject{
                    Message = "Chuyển đổi tài khoản thành công."
                };
            }else{
                var listItem = await _context.Stores.Where(x=>x.CreatedBy == user).ToArrayAsync();
                foreach(var item in listItem){
                    item.Mode = false;
                    await _context.SaveChangesAsync();
                }
                user.ActivateBy = "CUSTOMER";
                await _context.SaveChangesAsync();
                return new ApiObject{
                    Message = "Chuyển đổi tài khoản thành công."
                };
            }
           
        }
    }
}