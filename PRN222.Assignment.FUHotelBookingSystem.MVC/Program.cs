using Microsoft.EntityFrameworkCore;
using PRN222.Assignment.FUHotelBookingSystem.Repository.Model;
using PRN222.Assignment.FUHotelBookingSystem.Repository.Repositories;
using PRN222.Assignment.FUHotelBookingSystem.Repository.UOW;
using PRN222.Assignment.FUHotelBookingSystem.Service.BookingServices;
using PRN222.Assignment.FUHotelBookingSystem.Service.CookieService;
using PRN222.Assignment.FUHotelBookingSystem.Service.HotelServices;
using PRN222.Assignment.FUHotelBookingSystem.Service.MessageServices;
using PRN222.Assignment.FUHotelBookingSystem.Service.RedisService;
using PRN222.Assignment.FUHotelBookingSystem.Service.RoomServices;
using PRN222.Assignment.FUHotelBookingSystem.Service.UserServices;

namespace PRN222.Assignment.FUHotelBookingSystem.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<FuhotelBookingSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Đăng ký UnitOfWork và các Repositories
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped(typeof(IRepoGeneric<>), typeof(RepoGeneric<>));

            // Đăng ký các Service cần thiết
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IHotelService, HotelService>();
            builder.Services.AddScoped<IMessageService, MessageService>();
            builder.Services.AddScoped<IRoomService, RoomService>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Overview}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
