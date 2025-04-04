﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Update.Internal;
using PRN222.Assignment.FUHotelBookingSystem.Repository.Model;

namespace PRN222.Assignment.FUHotelBookingSystem.Service.RoomServices
{
    public interface IRoomService
    {
        public Room getRoomByID(int id);
        public List<Room> getAllRoom();
        public List<Room> getAllRoomByHotelId(int hotelId);
        public bool SetRoomBlock(int id);

        public IQueryable<Room> GetQueryable();

        void createRoom(Room room);
        void updateRoom(Room room);
        void deleteRoom(int id);
    }
}
