using CodeMegaVNPay.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PRN222.Assignment.FUHotelBookingSystem.RazorPages.Models;
using PRN222.Assignment.FUHotelBookingSystem.Repository.Model;
using PRN222.Assignment.FUHotelBookingSystem.Service.BookingServices;
using PRN222.Assignment.FUHotelBookingSystem.Service.CookieService;
using PRN222.Assignment.FUHotelBookingSystem.Service.HotelServices;
using PRN222.Assignment.FUHotelBookingSystem.Service.RoomServices;
using PRN222.Assignment.FUHotelBookingSystem.Service.UserServices;

namespace PRN222.Assignment.FUHotelBookingSystem.RazorPages.Pages.BookingPage
{
    public class DetailsModel : PageModel
    {
        private readonly IRoomService _roomService;
        private readonly IHotelService _hotelService;
        private readonly IUSerCreateService _user;
        private readonly IVnPayService _vnpayService;
        private readonly IBookingService _bookingService;
        public DetailsModel(IRoomService roomService, IHotelService hotelService, IUSerCreateService uSerCreateService,IVnPayService vnPayService,IBookingService bookingService)
        {
            _hotelService = hotelService;
            _roomService = roomService;
            _user = uSerCreateService;
            _vnpayService = vnPayService;
            _bookingService = bookingService;
        }
        public Hotel Hotel { get; set; } = default!;
        public Booking Booking { get; set; } = default!;
        public User user { get; set; } = default!;
        public Room room { get; set; } = default!;
        public Decimal totalPrice { get; set; } = default;

        public void OnGet()
        {
           
            if (TempData["Hotel"] != null)
            {
                Hotel = JsonConvert.DeserializeObject<Hotel>(TempData["Hotel"].ToString());
            }

            if (TempData["Booking"] != null)
            {
                Booking = JsonConvert.DeserializeObject<Booking>(TempData["Booking"].ToString());
            }
            user = _user.getAccountById(Booking.UserId);
            var listRoom = _roomService.getAllRoomByHotelId(Hotel.Id);
            room = listRoom.Where(m => m.Id == Booking.RoomId).FirstOrDefault();

            DateTime checkinDate = (DateTime)Booking.CheckinAt; 
            DateTime checkoutDate = (DateTime)Booking.CheckinOut; 

            int daysDifference = (checkoutDate - checkinDate).Days;


            totalPrice = daysDifference * room.Price;

            HttpContext.Session.SetString("Hotel", JsonConvert.SerializeObject(Hotel));
            HttpContext.Session.SetString("Booking", JsonConvert.SerializeObject(Booking));
            HttpContext.Session.SetString("User", JsonConvert.SerializeObject(user));
            if (Hotel == null || Booking == null)
            {
                Console.WriteLine("Dữ liệu không được truyền đúng.");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var hotel = JsonConvert.DeserializeObject<Hotel>(HttpContext.Session.GetString("Hotel"));
            var booking = JsonConvert.DeserializeObject<Booking>(HttpContext.Session.GetString("Booking"));
            var user = JsonConvert.DeserializeObject<Booking>(HttpContext.Session.GetString("User"));
            var listRoom = _roomService.getAllRoomByHotelId(hotel.Id);
            var roomById = listRoom.Where(m => m.Id == booking.RoomId).FirstOrDefault();


            DateTime checkinDate = (DateTime)booking.CheckinAt;
            DateTime checkoutDate = (DateTime)booking.CheckinOut;

            int daysDifference = (checkoutDate - checkinDate).Days;


            var totalPrice1 = daysDifference * roomById.Price;

            booking.Status = "pending";
            var getUser = _user.getAccountById(user.Id);
            var booking1 =_bookingService.createBooking(booking);
            PaymentInformationModel paymentModel = new PaymentInformationModel();
            

            paymentModel.Name = getUser.FullName;
            paymentModel.Amount = (double)totalPrice1;
            paymentModel.OrderDescription = booking1.Id.ToString();
            paymentModel.OrderType = "Order room: " + roomById.RoomNumber;
            var paymentUrl = _vnpayService.CreatePaymentUrl(paymentModel,HttpContext);

            // Chuyển hướng đến trang thanh toán
            return Redirect(paymentUrl);
        }
    }
}
