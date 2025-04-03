using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN222.Assignment.FUHotelBookingSystem.Repository.Model;

namespace PRN222.Assignment.FUHotelBookingSystem.Service.BookingServices
{
    public interface IBookingService
    {
        public List<Booking> getListBookingByUserId(int userId);
        public bool createBooking(Booking booking);
        public bool UpdateBooking(Booking booking);
        public Booking getBookingByid(int id);
    }
}
