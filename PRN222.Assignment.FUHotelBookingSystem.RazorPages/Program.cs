using CodeMegaVNPay.Services;
using System.Text;
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
using PRN222.Assignment.FUHotelBookingSystem.Service.RoomServices;
using PRN222.Assignment.FUHotelBookingSystem.Service.UserServices;

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

// Cấu hình CookieAuthentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Trang đăng nhập của bạn
        options.LogoutPath = "/Account/Logout"; // Trang đăng xuất
        options.ExpireTimeSpan = TimeSpan.FromDays(7); // Thời gian hết hạn cookie
        options.SlidingExpiration = true; // Gia hạn cookie khi có yêu cầu mới
        options.Cookie.HttpOnly = true; // Chỉ có thể truy cập cookie qua HTTP (giúp bảo mật)
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Tùy chọn bảo mật cho cookie
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

// Cấu hình JWT nếu cần
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

// Cấu hình Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

// Sử dụng Session và CookieAuthentication
app.UseSession();
app.UseAuthentication();
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

// Chuyển hướng trang chủ
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/HomePage/Index");
        return;
    }
    await next();
});

app.MapRazorPages();
app.Run();
