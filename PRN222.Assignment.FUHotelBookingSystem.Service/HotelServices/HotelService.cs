using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN222.Assignment.FUHotelBookingSystem.Repository.Model;
using PRN222.Assignment.FUHotelBookingSystem.Repository.UOW;

namespace PRN222.Assignment.FUHotelBookingSystem.Service.HotelServices
{
    public class HotelService : IHotelService
    {
        private readonly IUnitOfWork _unitOfWork;
        public HotelService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<Hotel> getAllHotel()
        {
            return _unitOfWork.Hotel.GetAll().ToList();
        }

        public Hotel getHotelById(int id)
        {
            return _unitOfWork.Hotel.GetbyId(id);
        }

        public IQueryable<Hotel> GetQueryable()
        {
            return _unitOfWork.Hotel.GetQueryable();
        }

        public void createHotel(Hotel hotel)
        {
            _unitOfWork.Hotel.Add(hotel);
            _unitOfWork.Commit();
        }

        public void updateHotel(Hotel hotel)
        {
            hotel.CreatedAt = DateTime.Now;
            _unitOfWork.Hotel.Update(hotel);
            _unitOfWork.Commit();
        }

        public void deleteHotel(int id)
        {
            var hotel = _unitOfWork.Hotel.GetbyId(id);
            if (hotel != null)
            {
                _unitOfWork.Hotel.Delete(hotel);
                _unitOfWork.Commit();
            }
        }
    }
}
