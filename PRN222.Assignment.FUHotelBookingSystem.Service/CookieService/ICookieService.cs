using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Assignment.FUHotelBookingSystem.Service.CookieService
{
    public interface ICookieService
    {
        void SetCookie<T>(string key, T value, int? expireTime);
        T GetCookie<T>(string key);
        void RemoveCookie(string key);
    }
}
