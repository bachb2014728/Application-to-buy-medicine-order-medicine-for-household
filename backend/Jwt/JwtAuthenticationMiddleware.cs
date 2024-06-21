
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
using backend.Data;
using backend.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace backend.Jwt
{
    public class JwtAuthenticationMiddleware(
        RequestDelegate next,
        IConfiguration configuration,
        IServiceScopeFactory serviceScopeFactory)
    {
        private readonly RequestDelegate _next = next;
        private readonly IConfiguration _configuration = configuration;
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    
        public async Task Invoke(HttpContext context)
        {
             var endpoint = context.GetEndpoint();

            // Nếu endpoint có thuộc tính [AllowAnonymous], cho phép yêu cầu đi qua mà không cần xác thực
            if (endpoint?.Metadata.GetMetadata<AllowAnonymousAttribute>() != null)
            {
                await _next(context);
                return;
            }
            var authHeader = context.Request.Headers.Authorization.ToString();
            if (authHeader.StartsWith("Bearer "))
            {
                var token = authHeader["Bearer ".Length..].Trim();
                if (!await IsValidToken(context, token))
                {
                    return;
                }
                GlobalVariables.Token = token;
            }
            else
            {
                await DenyAccess(context, "Yêu cầu này yêu cầu xác thực.");
                return;
            }

            await _next(context);
        }

        private async Task<bool> IsValidToken(HttpContext context, string token)
        {
            // Kiểm tra token có tồn tại trong cơ sở dữ liệu hay không
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var tokenNow = dbContext.Tokens.FirstOrDefault(t => t.Code == token);
               
                if (tokenNow?.IsActive == false || tokenNow == null)
                {
                    await DenyAccess(context, "Token không tồn tại hoặc đã hết hạn.");
                    return false;
                }
            }

            // Kiểm tra token có hợp lệ không
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:SigningKey"]!);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out _);
               

            }
            catch
            {
                await DenyAccess(context, "Token không hợp lệ.");
                return false;
            }
            
            return true;
        }

        private static async Task DenyAccess(HttpContext context, string message)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            var response = new { status = 401, message = message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }

}