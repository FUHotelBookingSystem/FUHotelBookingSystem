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
    public class IndexModel : PageModel
    {
        private readonly IHotelService _hotelService;

        public IndexModel(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        public IList<Hotel> Hotel { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; } = string.Empty;

        public int PageNumber { get; set; } = 1;
        public int TotalPages { get; set; }

        public async Task OnGetAsync(int pageNumber = 1)
        {
            int pageSize = 6;
            PageNumber = pageNumber;

            var listHotel =  _hotelService.getAllHotel();

            int totalRecords = listHotel.Count();
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            // Cập nhật danh sách sản phẩm hiển thị theo trang
            Hotel = listHotel
                .Skip((PageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

        }
    }
}
