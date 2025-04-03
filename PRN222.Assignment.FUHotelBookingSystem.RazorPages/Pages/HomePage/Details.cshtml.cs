using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN222.Assignment.FUHotelBookingSystem.Repository.Model;
using PRN222.Assignment.FUHotelBookingSystem.Service.HotelServices;

namespace PRN222.Assignment.FUHotelBookingSystem.RazorPages.Pages.HomePage
{
    public class DetailsModel : PageModel
    {
        private readonly IHotelService _hotelService;

        public DetailsModel(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        public Hotel Hotel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = _hotelService.getHotelById((int)id);
            if (hotel == null)
            {
                return NotFound();
            }
            else
            {
                Hotel = hotel;
            }
            return Page();
        }
    }
}
