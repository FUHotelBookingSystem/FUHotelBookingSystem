using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
            catch (Exception e)
            {
                return null;
            }
        }

        public List<Booking> GetAllBookings()
        {
            return _unitOfWork.Booking.GetQueryable()
                .Include(s => s.User)
                .Include(s => s.Room)
                .ThenInclude(r => r.Hotel)
                .ToList();
        }

        public Booking getBookingByid(int id)
        {
            return _unitOfWork.Booking.GetbyId(id);
        }

        public List<Booking> getListBookingByUserId(int userId)
        {
            return _unitOfWork.Booking.Find(m => m.UserId.Equals(userId)).ToList();
        }

        public IQueryable<Booking> GetQueryable()
        {
            return _unitOfWork.Booking.GetQueryable();
        }

        public IQueryable<User> GetUserQueryable()
        {
            return _unitOfWork.User.GetQueryable();
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
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
