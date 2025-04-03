using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN222.Assignment.FUHotelBookingSystem.Repository.Model;

namespace PRN222.Assignment.FUHotelBookingSystem.RazorPages.Pages.BookingPage
{
    public class DetailsModel : PageModel
    {
        private readonly PRN222.Assignment.FUHotelBookingSystem.Repository.Model.FuhotelBookingSystemContext _context;

        public DetailsModel(PRN222.Assignment.FUHotelBookingSystem.Repository.Model.FuhotelBookingSystemContext context)
        {
            _context = context;
        }

        public Booking Booking { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }
            else
            {
                Booking = booking;
            }
            return Page();
        }
    }
}
