using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222.Assignment.FUHotelBookingSystem.Service.CookieService;

namespace PRN222.Assignment.FUHotelBookingSystem.RazorPages.Pages.AuhthenticatePage
{
    
    public class LogoutModel : PageModel
    {
        private readonly ICookieService _cookieService;
        public LogoutModel(ICookieService cookieService)
        {
            _cookieService = cookieService;
        }

        public IActionResult OnGet()
        {

            var token = HttpContext?.Session.GetString("Token");
            _cookieService.RemoveCookie(token);
            HttpContext.Session.Clear();

            return RedirectToPage("/Index");
        }
    }
}
