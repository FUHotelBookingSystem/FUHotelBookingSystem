using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PRN222.Assignment.FUHotelBookingSystem.Service.CookieService
{
    public class CookieService : ICookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetCookie<T>(string key, T value, int? expireTime = null)
        {
            var options = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            if (expireTime.HasValue)
                options.Expires = DateTime.UtcNow.AddMinutes(expireTime.Value);
            else
                options.Expires = DateTime.UtcNow.AddHours(1);

            string jsonValue = JsonSerializer.Serialize(value);
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(key, jsonValue, options);
        }

        public T GetCookie<T>(string key)
        {
            var cookieValue = _httpContextAccessor.HttpContext?.Request.Cookies[key];
            if (cookieValue == null) return default; 


            return JsonSerializer.Deserialize<T>(cookieValue);
        }

        public void RemoveCookie(string key)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete(key);
        }
    }
}
