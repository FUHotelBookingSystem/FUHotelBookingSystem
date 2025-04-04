using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN222.Assignment.FUHotelBookingSystem.Repository.Model;
using PRN222.Assignment.FUHotelBookingSystem.Repository.UOW;

namespace PRN222.Assignment.FUHotelBookingSystem.Service.RoomServices
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RoomService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<Room> getAllRoom()
        {
            return _unitOfWork.Room.GetAll().ToList();
        }

        public List<Room> getAllRoomByHotelId(int hotelId)
        {
            return _unitOfWork.Room.Find(m => m.HotelId.Equals(hotelId)).ToList();
        }

        public Room getRoomByID(int id)
        {
            return _unitOfWork.Room.GetbyId(id);
        }

        public bool SetRoomBlock(int id)
        {
            try
            {
                var findRoom = _unitOfWork.Room.GetbyId(id);
                findRoom.Status = "Occupied";
                _unitOfWork.Room.Update(findRoom);
                return true;
            }catch(Exception e)
            {
                return false;
            }
        }
    }
}
