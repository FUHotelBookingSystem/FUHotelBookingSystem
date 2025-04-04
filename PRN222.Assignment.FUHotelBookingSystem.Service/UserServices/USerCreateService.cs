using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN222.Assignment.FUHotelBookingSystem.Repository.Model;
using PRN222.Assignment.FUHotelBookingSystem.Repository.UOW;
using PRN222.Assignment.FUHotelBookingSystem.Service.CookieService;
using PRN222.Assignment.FUHotelBookingSystem.Service.helper;
using PRN222.Assignment.FUHotelBookingSystem.Service.RedisService;

namespace PRN222.Assignment.FUHotelBookingSystem.Service.UserServices
{
    public class USerCreateService : IUSerCreateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICookieService _cookie;
        private readonly IRedisCacheService _redisCacheService;
        public USerCreateService(IUnitOfWork unitOfWork, ICookieService cookie, IRedisCacheService redisCacheService)
        {
            _unitOfWork = unitOfWork;
            _cookie = cookie;
            _redisCacheService = redisCacheService;
        }
        public bool createAccount(User account)
        {
            try
            {
                if (account == null) return false;
                string hashedPassword = HashHelper.ComputeSha256Hash(account.PasswordHash);
                account.PasswordHash = hashedPassword;
                _unitOfWork.User.Add(account);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool deleteAccount(int idAccount)
        {
            try
            {
                var findAccount = _unitOfWork.User.GetbyId(idAccount);
                if (findAccount == null) return false;
                
                _unitOfWork.User.Delete(findAccount);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public User findUserByEmail(string email)
        {
            var result = _unitOfWork.User.Find(m => m.Email.Equals(email)).FirstOrDefault();
            return result;
        }

        public User getAccountById(int id)
        {
            var result = _unitOfWork.User.GetbyId(id);

            return result;
        }

        public string login(string email, string passworld)
        {
            if (email == null || passworld == null) return null;

            string hashedPassword = HashHelper.ComputeSha256Hash(passworld);
            var result = _unitOfWork.User.Find(m => m.Email.Equals(email) && m.PasswordHash.Equals(hashedPassword)).FirstOrDefault();
            if (result != null)
            {
                String token = Guid.NewGuid().ToString();
                Debug.WriteLine(token);
                _cookie.SetCookie(token, result, 1);
                _cookie.SetCookie("ReLogin", token, 1);
                _redisCacheService.SetAsync("Relogin", token, TimeSpan.FromMinutes(60));
                _redisCacheService.SetAsync("CookieRelogin", result, TimeSpan.FromMinutes(60));
                return token;
            }

            return null;
        }

        public bool updateAccount(User account)
        {
            try
            {
                if (account == null)
                {
                    throw new ArgumentNullException(nameof(account), "Account không được null.");
                }

                _unitOfWork.User.Update(account);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}
