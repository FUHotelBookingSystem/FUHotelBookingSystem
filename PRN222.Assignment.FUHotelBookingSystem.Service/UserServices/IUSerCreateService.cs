using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN222.Assignment.FUHotelBookingSystem.Repository.Model;

namespace PRN222.Assignment.FUHotelBookingSystem.Service.UserServices
{
    public interface IUSerCreateService
    {
        bool createAccount(User account);
        bool updateAccount(User account);
        User getAccountById(int id);
        bool deleteAccount(int idAccount);
        String login(string email, string passworld);
    }
}
