using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN222.Assignment.FUHotelBookingSystem.Repository.Model;
using PRN222.Assignment.FUHotelBookingSystem.Repository.Repositories;

namespace PRN222.Assignment.FUHotelBookingSystem.Repository.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        IRepoGeneric<Booking> Booking { get; }
        IRepoGeneric<Hotel> Hotel { get; }
        IRepoGeneric<Message> Message { get; }
        IRepoGeneric<Payment> Payment { get; }
        IRepoGeneric<Room> Room { get; }
        IRepoGeneric<User> User { get; }

        void Commit();
    }
}
