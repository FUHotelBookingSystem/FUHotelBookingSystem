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

        public int CountBooking()
        {
            var result = _unitOfWork.Booking.GetAll().Count();
            return result;
        }

        public Booking createBooking(Booking booking)
        {
            try
            {
                

                return _unitOfWork.Booking.Add1(booking);
            }
            catch(Exception e)
            {
                return null;
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

        public bool UpdateStatustBooking(int id)
        {
            try
            {
                var result = _unitOfWork.Booking.GetbyId(id);
                result.Status = "Confirmed";
                _unitOfWork.Booking.Update(result);
                return true;
            }catch(Exception e)
            {
                return false;
            }
        }
    }
}
