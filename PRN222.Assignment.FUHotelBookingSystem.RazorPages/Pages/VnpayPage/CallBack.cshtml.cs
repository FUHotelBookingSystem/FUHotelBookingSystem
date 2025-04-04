using CodeMegaVNPay.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using PRN222.Assignment.FUHotelBookingSystem.Repository.Model;
using PRN222.Assignment.FUHotelBookingSystem.Service.BookingServices;
using PRN222.Assignment.FUHotelBookingSystem.Service.HotelServices;
using PRN222.Assignment.FUHotelBookingSystem.Service.RoomServices;
using PRN222.Assignment.FUHotelBookingSystem.Service.UserServices;


namespace PRN222.Assignment.FUHotelBookingSystem.RazorPages.Pages.VnpayPage
{
    public class CallBackModel : PageModel
    {
        private readonly IVnPayService _vnpayService;
        private readonly IBookingService _bookingService;
        private readonly IUSerCreateService _user;
        private readonly IRoomService _roomService;
        private readonly IHotelService _hotelService;

        public CallBackModel(IVnPayService vnpayService,IBookingService bookingService, IUSerCreateService user, IRoomService roomService, IHotelService hotelService)
        {
            _vnpayService = vnpayService;
            _bookingService = bookingService;
            _user = user;
            _hotelService = hotelService;
            _roomService = roomService;
        }

        [BindProperty]
        public string VnpTransactionStatus { get; set; }
        public string TransactionResult { get; set; }
        public User user;
        public Hotel hotel;
        public Room room;
        public String totalPrice;
        public decimal price;
        public void OnGet()
        {
            // Lấy các tham số callback từ query string
            var queryCollection = Request.Query;

            // Kiểm tra mã trạng thái giao dịch từ VNPay
            var vnpResponse = _vnpayService.PaymentExecute(queryCollection);

            if (vnpResponse != null && vnpResponse.VnPayResponseCode == "00")
            {
                var reponse = vnpResponse.OrderDescription.ToString();
                var getIdBooking = reponse.Split(" ")[1].ToString();
                var booking = _bookingService.getBookingByid(int.Parse(getIdBooking));
                user = _user.getAccountById(booking.UserId);
                room = _roomService.getRoomByID(booking.RoomId);
                hotel = _hotelService.getHotelById(room.HotelId);
                price = room.Price;
                totalPrice = reponse.Split(" ")[2].ToString();

                _bookingService.UpdateStatustBooking(booking.Id);
                _roomService.SetRoomBlock(room.Id);
                VnpTransactionStatus = "Transaction successful.";
                // Cập nhật kết quả giao dịch vào cơ sở dữ liệu nếu cần
                // Ví dụ: Cập nhật trạng thái booking hoặc gửi email cho khách hàng
            }
            else
            {
                VnpTransactionStatus = "Transaction failed or canceled.";
            }
        }
    }
}
