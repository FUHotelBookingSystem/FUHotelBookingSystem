using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN222.Assignment.FUHotelBookingSystem.Repository.Model;
using PRN222.Assignment.FUHotelBookingSystem.Repository.UOW;

namespace PRN222.Assignment.FUHotelBookingSystem.Service.BookingServices
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool createBooking(Booking booking)
        {
            try
            {
                _unitOfWork.Booking.Add(booking);
                return true;
            }catch(Exception e)
            {
                return false;
            }
        }

        public Booking getBookingByid(int id)
        {
            return _unitOfWork.Booking.GetbyId(id);
        }

        public List<Booking> getListBookingByUserId(int userId)
        {
            return _unitOfWork.Booking.Find(m => m.UserId.Equals(userId)).ToList();
        }

        public bool UpdateBooking(Booking booking)
        {
            try
            {
                _unitOfWork.Booking.Update(booking);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
