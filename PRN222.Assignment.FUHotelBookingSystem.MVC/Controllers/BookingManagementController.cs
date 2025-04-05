using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRN222.Assignment.FUHotelBookingSystem.Repository.Model;
using PRN222.Assignment.FUHotelBookingSystem.Service.BookingServices;
using PRN222.Assignment.FUHotelBookingSystem.Service.HotelServices;
using System.Linq;

namespace PRN222.Assignment.FUHotelBookingSystem.MVC.Controllers
{
    public class BookingManagementController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IHotelService _hotelService;

        public BookingManagementController(IBookingService bookingService, IHotelService hotelService)
        {
            _bookingService = bookingService;
            _hotelService = hotelService;
        }

        public IActionResult Index(string search = "", int hotelId = 0, DateTime? filterDate = null, int page = 1)
        {
            int pageSize = 5;
            var bookings = _bookingService.GetAllBookings();

            if (!string.IsNullOrEmpty(search))
            {
                bookings = bookings.Where(b => b.User.FullName.Contains(search, StringComparison.OrdinalIgnoreCase)
                                            || b.User.Email.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (hotelId > 0)
            {
                bookings = bookings.Where(b => b.Room.HotelId == hotelId).ToList();
            }

            if (filterDate.HasValue)
            {
                bookings = bookings.Where(b => b.BookingDate == DateOnly.FromDateTime(filterDate.Value)).ToList();
            }

            bookings = bookings.OrderByDescending(b => b.CreatedAt).ToList();

            var paginatedBookings = bookings.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.TotalPages = (int)Math.Ceiling(bookings.Count / (double)pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.Hotels = _hotelService.getAllHotel();

            return View(paginatedBookings);
        }

        [HttpPost]
        public IActionResult CheckIn(int bookingId)
        {
            var booking = _bookingService.getBookingByid(bookingId);
            if (booking != null && booking.CheckinAt == null)
            {
                booking.CheckinAt = DateTime.Now;
                _bookingService.UpdateBooking(booking);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CheckOut(int bookingId)
        {
            var booking = _bookingService.getBookingByid(bookingId);
            if (booking != null && booking.CheckinAt != null && booking.CheckinOut == null)
            {
                booking.CheckinOut = DateTime.Now;
                _bookingService.UpdateBooking(booking);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetBookingDetails(int bookingId)
        {
            var booking = _bookingService.GetQueryable()
                .Include(b => b.User)
                .Include(b => b.Room)
                .ThenInclude(r => r.Hotel)
                .Include(b => b.Payments)
                .FirstOrDefault(b => b.Id == bookingId);
            if (booking == null)
            {
                return NotFound();
            }

            var bookingDetails = new
            {
                booking.Id,
                booking.User.FullName,
                booking.User.Email,
                booking.User.Phone,
                booking.Room.Hotel.Name,
                booking.Room.Hotel.Location,
                booking.Room.RoomNumber,
                booking.BookingDate,
                booking.CheckinAt,
                booking.CheckinOut,
                booking.Status,
                Payments = booking.Payments.Select(p => new
                {
                    p.Amount,
                    p.PaymentMethod,
                    p.Status,
                    p.CreatedAt
                })
            };

            return Json(bookingDetails);
        }
    }
}
