using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN222.Assignment.FUHotelBookingSystem.Repository.Model;

namespace PRN222.Assignment.FUHotelBookingSystem.Service.HotelServices
{
    public interface IHotelService
    {
        public List<Hotel> getAllHotel();
        public Hotel getHotelById(int id);

        public IQueryable<Hotel> GetQueryable();
        void createHotel(Hotel hotel);
        void updateHotel(Hotel hotel);
        void deleteHotel(int id);
    }
}
