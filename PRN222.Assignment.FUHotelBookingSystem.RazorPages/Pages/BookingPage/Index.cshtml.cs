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
    public class IndexModel : PageModel
    {
        private readonly PRN222.Assignment.FUHotelBookingSystem.Repository.Model.FuhotelBookingSystemContext _context;

        public IndexModel(PRN222.Assignment.FUHotelBookingSystem.Repository.Model.FuhotelBookingSystemContext context)
        {
            _context = context;
        }

        public IList<Booking> Booking { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Booking = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User).ToListAsync();
        }
    }
}
