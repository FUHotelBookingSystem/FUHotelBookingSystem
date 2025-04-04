
using PRN222.Assignment.FUHotelBookingSystem.RazorPages.Models;

namespace CodeMegaVNPay.Services;
public interface IVnPayService
{
    string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
    PaymentResponseModel PaymentExecute(IQueryCollection collections);
}