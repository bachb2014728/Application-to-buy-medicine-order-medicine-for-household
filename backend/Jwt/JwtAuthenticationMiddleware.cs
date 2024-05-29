
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using System.Text.Json;
using backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace backend.Jwt
{
    public class JwtAuthenticationMiddleware(
        RequestDelegate next, IConfiguration configuration,
        IServiceScopeFactory serviceScopeFactory
        )
    {
        private readonly RequestDelegate _next = next;
        private readonly IConfiguration _configuration = configuration;
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        public async Task Invoke(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                var allowAnonymousAttribute = endpoint.Metadata.GetMetadata<AllowAnonymousAttribute>();
                if (allowAnonymousAttribute != null)
                {
                    // Nếu endpoint có thuộc tính [AllowAnonymous], cho phép yêu cầu đi qua mà không cần xác thực
                    await _next(context);
                    return;
                }
            }
            var authHeader = context.Request.Headers.Authorization.ToString();

            if (authHeader.StartsWith("Bearer "))
            {
                var token = authHeader[7..];
                var key = Encoding.ASCII.GetBytes(_configuration["JWT:SigningKey"]!);

                var tokenHand = new JwtSecurityTokenHandler();
                if (!tokenHand.CanReadToken(token))
                {
                    context.Response.StatusCode = 400;
                    var response = new
                    {
                        status = (int)HttpStatusCode.BadRequest,
                        message = "Token không đúng định dạng.",
                        instance = "API"
                    };
                    var json = JsonSerializer.Serialize(response);
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(json);
                    return;
                }
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    // Lấy ApplicationDBContext từ scope mới
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();

                    // Kiểm tra xem token có tồn tại trong cơ sở dữ liệu hay không
                    var tokenInDb = await dbContext.Tokens.FirstOrDefaultAsync(t => t.Code == token);
                    if (tokenInDb == null)
                    {
                        context.Response.StatusCode = 404;
                        var response = new
                        {
                            status = (int)HttpStatusCode.NotFound,
                            message = "Token đã cũ.",
                            instance = "API"
                        };
                        var json = JsonSerializer.Serialize(response);
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(json);
                        return;
                    }
                }
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.ReadJwtToken(token);
                    if (jwtToken.ValidTo < DateTime.UtcNow)
                    {
                        context.Response.StatusCode = 401;
                        var response = new
                        {
                            status = (int)HttpStatusCode.Unauthorized,
                            message = "Token đã hết hạn.",
                            instance = "API"
                        };
                        var json = JsonSerializer.Serialize(response);
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(json);
                        return;
                    }

                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);
                }
                catch (SecurityTokenInvalidSignatureException)
                {
                    // Lỗi khi giải mã 
                    context.Response.StatusCode = 401;
                    var response = new
                    {
                        status = (int)HttpStatusCode.Unauthorized,
                        message = "Không thể giải mã token.",
                        instance = "API"
                    };
                    var json = JsonSerializer.Serialize(response);
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(json);
                    return;
                }
                catch (Exception e)
                {
                    context.Response.StatusCode = 401;
                    var response = new
                    {
                        status = (int)HttpStatusCode.Unauthorized,
                        message = e.Message,
                        instance = "API"
                    };
                    var json = JsonSerializer.Serialize(response);
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(json);
                    return;
                }
            }
            else
            {
                context.Response.StatusCode = 401;
                var response = new
                {
                    status = (int)HttpStatusCode.Unauthorized,
                    message = "Yêu cầu này yêu cầu xác thực.",
                    instance = "API"
                };
                var json = JsonSerializer.Serialize(response);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
                return;
            }

            await _next(context);
        }
    }
}