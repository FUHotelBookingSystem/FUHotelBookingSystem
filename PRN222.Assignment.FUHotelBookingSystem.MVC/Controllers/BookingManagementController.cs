using Microsoft.AspNetCore.Mvc;

namespace PRN222.Assignment.FUHotelBookingSystem.MVC.Controllers
{
    public class BookingManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
