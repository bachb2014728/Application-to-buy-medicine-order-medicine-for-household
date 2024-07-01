using backend.Data;
using backend.Error.Handler;
using backend.Interface;
using backend.Middleware;
using backend.Jwt;
using backend.Model;
using backend.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(option => {
    option.AddPolicy("Cors", policy => {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    option =>
    {
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "Đặt thuốc, mua thuốc hộ API", Version = "v1" });
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
            }
        });
    }
);



builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("WebApiDatabase"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    // options.Password.RequireDigit = true;
    // options.Password.RequireLowercase = true;
    // options.Password.RequireUppercase = true;
    // options.Password.RequireNonAlphanumeric = true;
    // options.Password.RequiredLength = 12;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 0;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
}).AddEntityFrameworkStores<ApplicationDbContext>().AddApiEndpoints();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]!)
        )
    };
});


builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthentication, AuthenticationService>();
builder.Services.AddScoped<ICategory,CategoryService>();
builder.Services.AddScoped<IStore,StoreService>();
builder.Services.AddScoped<IProduct, ProductService>();
builder.Services.AddScoped<ICustomer, CustomerService>();
builder.Services.AddScoped<INotification, NotificationService>();
builder.Services.AddScoped<IVoucher, VoucherService>();
builder.Services.AddScoped<IOrder, OrderService>();
builder.Services.AddScoped<IReceiver, ReceiverService>();

builder.Services.AddScoped<IComment, CommentService>();
builder.Services.AddScoped<IUse, UseService>();
builder.Services.AddScoped<IManufacturer, ManufacturerService>();
builder.Services.AddScoped<IDosageForm, DosageFormService>();
builder.Services.AddScoped<IContraindication, ContraindicationService>();
builder.Services.AddScoped<ICart, CartService>();
builder.Services.AddScoped<backend.Helper.ConvertInformation>();

builder.Services.AddExceptionHandler<AppExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("Cors");

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();

app.UseMiddleware<NotFoundMiddleware>();
app.UseMiddleware<JwtAuthenticationMiddleware>();

app.UseExceptionHandler();

app.UseAuthorization();

app.MapControllers();

app.Run();
