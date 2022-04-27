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

namespace ActiveCarRental.Pages.RentalCars
{
    public class CarDetailsModel : PageModel
    {
        private readonly ActiveCarRental.Data.RentalContext _context;

        public CarDetailsModel(ActiveCarRental.Data.RentalContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RentalCar RentalCar { get; set; }
        public IList<CarBooking> CarBookings { get; set; }
        public int TotalDaysRented { get; set; }
        public int TotalKmTraveld { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RentalCar = await _context.RentalCars.Include(c => c.CarBookings).FirstOrDefaultAsync(m => m.RegisterId == id);

            foreach (var CB in RentalCar.CarBookings)
            {
                if (CB.TimeReturned == null)
                {
                    CB.TimeReturned = DateTime.Now;
                }
                TimeSpan dateDiff = (TimeSpan)(CB.TimeReturned - CB.TimeRented);

                TotalDaysRented = TotalDaysRented + dateDiff.Days;
                TotalKmTraveld = (int)(TotalKmTraveld + CB.KilometerReading);
            }

            if (RentalCar == null)
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

            RentalCar = await _context.RentalCars.FindAsync((int)id);

            CarBookings = await _context.CarBookings.ToListAsync();
            foreach (CarBooking CB in CarBookings)
            {
                if (CB.RegisterId == id)
                {
                    CB.RegisterId = null;
                }
            }

            if (RentalCar != null)
            {
                _context.RentalCars.Remove(RentalCar);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
