using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRN222.Assignment.FUHotelBookingSystem.Service.BookingServices;
using PRN222.Assignment.FUHotelBookingSystem.Service.UserServices;

namespace PRN222.Assignment.FUHotelBookingSystem.MVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly IBookingService _bookingService;

        public LoginController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string email, string password)
        {
            var user = _bookingService.GetUserQueryable().FirstOrDefault(u => u.Email == email);
            if (user != null && user.PasswordHash == password && (user.Role == "Admin" || user.Role == "Staff"))
            {
                return RedirectToAction("Index", "Overview");
            }

            ViewBag.ErrorMessage = "Invalid login attempt.";
            return View();
        }
    }
}
