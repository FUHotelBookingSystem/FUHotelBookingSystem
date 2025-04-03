using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN222.Assignment.FUHotelBookingSystem.Repository.Model;
using PRN222.Assignment.FUHotelBookingSystem.Service.UserServices;

namespace PRN222.Assignment.FUHotelBookingSystem.RazorPages.Pages.AuhthenticatePage
{
    public class IndexModel : PageModel
    {
        private readonly IUSerCreateService _userCreateService;

        public IndexModel(IUSerCreateService userCreateService)
        {
            _userCreateService = userCreateService;
        }

        public IList<User> User { get;set; } = default!;
        [BindProperty]
        public String Email { get; set; } = string.Empty;
        [BindProperty]
        public string Password { get; set; } = string.Empty;
        public async Task<IActionResult> OnPostAsync()
        {
            var token = _userCreateService.login(Email, Password);
            if (token == null)
            {
                ModelState.AddModelError(string.Empty, "Wrong userName or Passworld");
                return Page();
            }
            else
            {
                HttpContext.Session.SetString("Token", token);
                return RedirectToPage("/HomePage/Index");
            }
        }
    }
}
