using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN222.Assignment.FUHotelBookingSystem.Repository.Model;
using PRN222.Assignment.FUHotelBookingSystem.Repository.UOW;

namespace PRN222.Assignment.FUHotelBookingSystem.Service.PaymentService
{
    public class PaymentService : IPaymentService
    {
        public readonly IUnitOfWork _unitOfWork;
        public PaymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool createPayment(Payment payment)
        {
            try
            {
                _unitOfWork.Payment.Add(payment);
                return true;

            }catch(Exception e)
            {
                return false;
            }

        }
    }
}
