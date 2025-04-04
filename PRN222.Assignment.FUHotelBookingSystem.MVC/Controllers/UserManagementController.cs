using Microsoft.AspNetCore.Mvc;

namespace PRN222.Assignment.FUHotelBookingSystem.MVC.Controllers
{
    public class UserManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
