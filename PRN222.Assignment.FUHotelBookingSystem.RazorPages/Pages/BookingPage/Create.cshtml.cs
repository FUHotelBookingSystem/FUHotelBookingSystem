using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PRN222.Assignment.FUHotelBookingSystem.Repository.Model;
using PRN222.Assignment.FUHotelBookingSystem.Service.CookieService;
using PRN222.Assignment.FUHotelBookingSystem.Service.HotelServices;
using PRN222.Assignment.FUHotelBookingSystem.Service.RoomServices;

namespace PRN222.Assignment.FUHotelBookingSystem.RazorPages.Pages.BookingPage
{
    public class CreateModel : PageModel
    {
        private readonly IRoomService _roomService;
        private readonly IHotelService _hotelService;
        private readonly ICookieService _cookieService;

        public CreateModel(IRoomService roomService,IHotelService hotelService,ICookieService cookieService)
        {
            _hotelService = hotelService;
            _roomService = roomService;
            _cookieService = cookieService;
        }

        public IActionResult OnGet(int? id)
        {
            var token = HttpContext.Session.GetString("Token");
            User user = _cookieService.GetCookie<User>(token);
            var hotel = _hotelService.getHotelById((int)id);
            var room = _roomService.getAllRoomByHotelId((int)id);
            ViewData["RoomId"] = new SelectList(room, "Id", "RoomNumber");
            //ViewData["UserId"] = new SelectList((System.Collections.IEnumerable)user, "Id", "Email");
            return Page();
        }

        [BindProperty]
        public Booking Booking { get; set; } = default!;


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            return RedirectToPage("./Index");
        }
    }
}
