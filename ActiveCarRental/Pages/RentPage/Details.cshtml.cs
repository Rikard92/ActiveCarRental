#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ActiveCarRental.Data;
using ActiveCarRental.Models;

namespace ActiveCarRental.Pages.RentPage
{
    public class DetailsModel : PageModel
    {
        private readonly ActiveCarRental.Data.RentalContext _context;

        public DetailsModel(ActiveCarRental.Data.RentalContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CarBooking CarBooking { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CarBooking = await _context.CarBookings
                .Include(c => c.Customer)
                .Include(c => c.Register).FirstOrDefaultAsync(m => m.BookingId == id);

            if (CarBooking == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CarBooking = await _context.CarBookings.FindAsync(id);

            if (CarBooking != null)
            {
                _context.CarBookings.Remove(CarBooking);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
