using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PRN222.Assignment.FUHotelBookingSystem.Repository.Model;
using PRN222.Assignment.FUHotelBookingSystem.Service.BookingServices;
using PRN222.Assignment.FUHotelBookingSystem.Service.CookieService;
using PRN222.Assignment.FUHotelBookingSystem.Service.HotelServices;
using PRN222.Assignment.FUHotelBookingSystem.Service.RoomServices;
using PRN222.Assignment.FUHotelBookingSystem.Service.UserServices;

namespace PRN222.Assignment.FUHotelBookingSystem.RazorPages.Pages.BookingPage
{
    public class CreateModel : PageModel
    {
        private readonly IRoomService _roomService;
        private readonly IHotelService _hotelService;
        private readonly ICookieService _cookieService;
        private readonly IUSerCreateService _user;
        private readonly IBookingService _bookignService;

        public CreateModel(IRoomService roomService,IHotelService hotelService,ICookieService cookieService,IUSerCreateService uSerCreateService,IBookingService bookingService)
        {
            _hotelService = hotelService;
            _roomService = roomService;
            _cookieService = cookieService;
            _user = uSerCreateService;
            _bookignService = bookingService;
        }

        [BindProperty]
        public Booking Booking { get; set; } = default!;
        [BindProperty]
        public Hotel hotel { get; set; } = default!;
        [BindProperty]
        public User user { get; set; } = default!;

        public IActionResult OnGet(int? id)
        {
            var token = HttpContext.Session.GetString("Token");
            user = _cookieService.GetCookie<User>(token);
            hotel = _hotelService.getHotelById((int)id);
            var room = _roomService.getAllRoomByHotelId((int)id);
            Booking = new Booking();
            
            Booking.UserId = user.Id;
            Booking.BookingDate = DateOnly.FromDateTime(DateTime.Now);



            ViewData["RoomId"] = room.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = $"{r.RoomNumber} - Price: ${r.Price}",
            
            }).ToList();

            return Page();

        }




        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (Booking.CheckinAt == null || Booking.CheckinOut == null)
            {
                ModelState.AddModelError("Booking.CheckinAt", "Check-in date is required.");
                ModelState.AddModelError("Booking.CheckinOut", "Check-out date is required.");
                var room = _roomService.getAllRoomByHotelId((int)id);
                ViewData["RoomId"] = room.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = $"{r.RoomNumber} - Price: ${r.Price}",

                }).ToList();
                return Page();
            }
            if (_bookignService.checkRoomActivate(Booking.CheckinAt, Booking.CheckinOut, Booking.RoomId).Any())
            {
                var room = _roomService.getAllRoomByHotelId((int)id);
                ViewData["RoomId"] = room.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = $"{r.RoomNumber} - Price: ${r.Price}",

                }).ToList();
                ModelState.AddModelError("Booking.CheckinAt", "Phòng đã được đặt trong thời gian này.");
                ModelState.AddModelError("Booking.CheckinOut", "Vui lòng chọn ngày khác.");
                return Page();
            }


            TempData["Hotel"] = JsonConvert.SerializeObject(hotel);
            TempData["Booking"] = JsonConvert.SerializeObject(Booking);
            
            return RedirectToPage("./Details");
        }
    }
}
