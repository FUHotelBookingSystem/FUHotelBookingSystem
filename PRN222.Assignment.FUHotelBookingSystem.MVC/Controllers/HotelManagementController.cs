using Microsoft.AspNetCore.Mvc;

namespace PRN222.Assignment.FUHotelBookingSystem.MVC.Controllers
{
    public class HotelManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
