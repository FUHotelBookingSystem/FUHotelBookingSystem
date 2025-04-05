using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using PRN222.Assignment.FUHotelBookingSystem.Repository.Model;
using PRN222.Assignment.FUHotelBookingSystem.Service.HotelServices;
using PRN222.Assignment.FUHotelBookingSystem.Service.RoomServices;
using PRN222.Assignment.FUHotelBookingSystem.Service.helper;
using System.Linq;
using System.Threading.Tasks;

namespace PRN222.Assignment.FUHotelBookingSystem.MVC.Controllers
{
    public class HotelManagementController : Controller
    {
        private readonly IHotelService _hotelService;
        private readonly IRoomService _roomService;
        private readonly IHubContext<FUHub> _hubContext;

        public HotelManagementController(IHotelService hotelService, IRoomService roomService, IHubContext<FUHub> hubContext)
        {
            _hotelService = hotelService;
            _roomService = roomService;
            _hubContext = hubContext;
        }

        public IActionResult Index(string search = "", int page = 1)
        {
            int pageSize = 3;
            var hotels = _hotelService.getAllHotel();

            if (!string.IsNullOrEmpty(search))
            {
                hotels = hotels.Where(h => h.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
                                        || h.Location.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            hotels = hotels.OrderByDescending(h => h.CreatedAt).ToList();

            var paginatedHotels = hotels.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.TotalPages = (int)Math.Ceiling(hotels.Count / (double)pageSize);
            ViewBag.CurrentPage = page;

            return View(paginatedHotels);
        }

        [HttpGet]
        public IActionResult GetHotelDetails(int hotelId)
        {
            var hotel = _hotelService.GetQueryable()
                .Include(r => r.Rooms).FirstOrDefault(r => r.Id == hotelId);
            if (hotel == null)
            {
                return NotFound();
            }

            var hotelDetails = new
            {
                hotel.Id,
                hotel.Name,
                hotel.Location,
                hotel.Description,
                hotel.image,
                Rooms = hotel.Rooms.Select(r => new
                {
                    r.Id,
                    r.RoomNumber,
                    r.Type,
                    r.Price,
                    r.Status
                })
            };

            return Json(hotelDetails);
        }

        [HttpGet]
        public IActionResult GetRoomDetails(int roomId)
        {
            var room = _roomService.GetQueryable().FirstOrDefault(r => r.Id == roomId);
            if (room == null)
            {
                return NotFound();
            }

            var roomDetails = new
            {
                room.Id,
                room.HotelId,
                room.RoomNumber,
                room.Type,
                room.Price,
                room.Status
            };

            return Json(roomDetails);
        }

        [HttpPost]
        public async Task<IActionResult> CreateHotel(Hotel hotel)
        {
            _hotelService.createHotel(hotel);
            await _hubContext.Clients.All.SendAsync("ReceiveUpdate");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateHotel(Hotel hotel)
        {
            _hotelService.updateHotel(hotel);
            await _hubContext.Clients.All.SendAsync("ReceiveUpdate");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteHotel(int hotelId)
        {
            _hotelService.deleteHotel(hotelId);
            await _hubContext.Clients.All.SendAsync("ReceiveUpdate");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(Room room)
        {
            _roomService.createRoom(room);
            await _hubContext.Clients.All.SendAsync("ReceiveUpdate");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRoom(Room room)
        {
            _roomService.updateRoom(room);
            await _hubContext.Clients.All.SendAsync("ReceiveUpdate");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRoom(int roomId)
        {
            _roomService.deleteRoom(roomId);
            await _hubContext.Clients.All.SendAsync("ReceiveUpdate");
            return RedirectToAction("Index");
        }
    }
}
