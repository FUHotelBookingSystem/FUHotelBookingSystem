using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRN222.Assignment.FUHotelBookingSystem.Repository.Model;
using PRN222.Assignment.FUHotelBookingSystem.Repository.Repositories;

namespace PRN222.Assignment.FUHotelBookingSystem.Repository.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FuhotelBookingSystemContext _context;

        
        public IRepoGeneric<Booking> Booking { get; }

        public IRepoGeneric<Hotel> Hotel { get; }

        public IRepoGeneric<Message> Message { get; }

        public IRepoGeneric<Payment> Payment { get; }

        public IRepoGeneric<Room> Room { get; }

        public IRepoGeneric<User> User { get; }
        public UnitOfWork(FuhotelBookingSystemContext context)
        {
            _context = context;
            Booking = new RepoGeneric<Booking>(_context);
            Hotel = new RepoGeneric<Hotel>(_context);
            Message = new RepoGeneric<Message>(_context);
            Payment = new RepoGeneric<Payment>(_context);
            Room = new RepoGeneric<Room>(_context);
            User = new RepoGeneric<User>(_context);
        }
        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
