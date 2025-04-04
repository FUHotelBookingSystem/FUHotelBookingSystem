using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN222.Assignment.FUHotelBookingSystem.Repository.Model;
using PRN222.Assignment.FUHotelBookingSystem.Service.BookingServices;
using PRN222.Assignment.FUHotelBookingSystem.Service.CookieService;
using PRN222.Assignment.FUHotelBookingSystem.Service.HotelServices;
using PRN222.Assignment.FUHotelBookingSystem.Service.RoomServices;

namespace PRN222.Assignment.FUHotelBookingSystem.RazorPages.Pages.BookingPage
{
    public class IndexModel : PageModel
    {
        private readonly IBookingService _bookingService;
        private readonly ICookieService _cookieService;
        private readonly IRoomService _roomService;
        private readonly IHotelService _hotelService;
        public IndexModel(IBookingService bookingService,ICookieService cookieService,IRoomService roomService,IHotelService hotelService)
        {
            _bookingService = bookingService;
            _cookieService = cookieService;
            _hotelService = hotelService;
            _roomService = roomService;
        }

        public IList<Booking> Booking { get;set; } = default!;
        public User user { get; set; } = default!;
        public int PageNumber { get; set; } = 1;
        public int TotalPages { get; set; }


        public async Task OnGetAsync(int pageNumber = 1)
        {
            int pageSize = 6;
            PageNumber = pageNumber;

            // Retrieve the token from the session
            string token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                // Handle session token absence (e.g., redirect to login page)
                return;
            }

            // Get user data from the token
            user = _cookieService.GetCookie<User>(token);

            // Retrieve the list of bookings for the user and order them by the desired field (e.g., booking date)
            var listBooking = _bookingService.getListBookingByUserId(user.Id)
                                               .OrderByDescending(b => b.BookingDate); // Corrected ordering

            // Total records for pagination
            int totalRecords = listBooking.Count();

            // Eager load Room and Hotel information (optional improvement, depending on your data model and service layer)
            foreach (var booking in listBooking)
            {
                booking.Room = _roomService.getRoomByID(booking.RoomId);
                if (booking.Room != null)
                {
                    booking.Room.Hotel = _hotelService.getHotelById(booking.Room.HotelId);
                }
            }

            // Calculate the total number of pages
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            // Apply pagination
            Booking = listBooking
                .Skip((PageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

    }
}
