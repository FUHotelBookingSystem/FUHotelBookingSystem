using Microsoft.AspNetCore.Mvc;

namespace PRN222.Assignment.FUHotelBookingSystem.MVC.Controllers
{
    public class OverviewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
