using CodeMegaVNPay.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PRN222.Assignment.FUHotelBookingSystem.Repository.Model;
using PRN222.Assignment.FUHotelBookingSystem.Repository.Repositories;
using PRN222.Assignment.FUHotelBookingSystem.Repository.UOW;
using PRN222.Assignment.FUHotelBookingSystem.Service.BookingServices;
using PRN222.Assignment.FUHotelBookingSystem.Service.CookieService;
using PRN222.Assignment.FUHotelBookingSystem.Service.HotelServices;
using PRN222.Assignment.FUHotelBookingSystem.Service.MessageServices;
using PRN222.Assignment.FUHotelBookingSystem.Service.PaymentService;
using PRN222.Assignment.FUHotelBookingSystem.Service.RedisService;
using PRN222.Assignment.FUHotelBookingSystem.Service.RoomServices;
using PRN222.Assignment.FUHotelBookingSystem.Service.UserServices;
using StackExchange.Redis;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình các dịch vụ trong ứng dụng
builder.Services.AddDbContext<FuhotelBookingSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký UnitOfWork và các Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepoGeneric<>), typeof(RepoGeneric<>));

// Đăng ký các Service cần thiết
builder.Services.AddScoped<ICookieService, CookieService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IVnPayService, VnPayService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IUSerCreateService, USerCreateService>();
builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

// Cấu hình Cookie Authentication cho ứng dụng Razor
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Trang đăng nhập của bạn
        options.LogoutPath = "/Account/Logout"; // Trang đăng xuất
        options.ExpireTimeSpan = TimeSpan.FromDays(7); // Thời gian hết hạn cookie
        options.SlidingExpiration = true; // Gia hạn cookie khi có yêu cầu mới
        options.Cookie.HttpOnly = true; // Chỉ có thể truy cập cookie qua HTTP (giúp bảo mật)
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Tùy chọn bảo mật cho cookie
        options.Cookie.Name = "ReLogin"; // Tên cookie
        options.Cookie.SameSite = SameSiteMode.Lax; // Giúp cookie được duy trì trong các request cross-site (tốt cho callback)
    });

// Cấu hình Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian session hết hạn
    options.Cookie.HttpOnly = true; // Bảo mật cookie
    options.Cookie.IsEssential = true; // Session bắt buộc
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();

// Cấu hình JWT Authentication cho API
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        RequireExpirationTime = true,
        ValidateLifetime = true
    };
});

builder.Services.AddAuthorization();
builder.Services.AddSignalR();
var redisConfig = builder.Configuration.GetSection("RedisString");
var redisConnectionString = $"{redisConfig["Redis"]}:{redisConfig["Port"]},password={redisConfig["Password"]},ssl=False";

// Đăng ký Redis Connection Multiplexer vào DI container
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));

// Cấu hình Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

// Sử dụng Session và CookieAuthentication cho Razor Pages
app.UseSession();
app.UseAuthentication();  // Đảm bảo authentication luôn chạy trước authorization
app.UseAuthorization();

// Cấu hình các middleware khác
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Cấu hình routing
app.UseRouting();

// Chuyển hướng trang chủ nếu đường dẫn là "/"
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/HomePage/Index");
        return;
    }
    await next();
});

app.MapRazorPages(); // Sử dụng Razor Pages
app.Run();
