#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ActiveCarRental.Data;
using ActiveCarRental.Models;

namespace ActiveCarRental.Pages.RentPage
{
    public class ReturnCarModel : PageModel
    {
        private readonly ActiveCarRental.Data.RentalContext _context;

        public ReturnCarModel(ActiveCarRental.Data.RentalContext context)
        {
            _context = context;
        }

        public static long CarId { get; set; }

        [BindProperty]
        public CarBooking CarBooking { get; set; }

        [BindProperty]
        public RentalCar RentalCar { get; set; }

        [BindProperty]
        public Customer Customer { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CarId = (long)id;
            RentalCar = await _context.RentalCars.Include(c => c.CarBookings).FirstOrDefaultAsync(m => m.RegisterId == CarId);

            foreach (CarBooking CB in RentalCar.CarBookings)
            {
                if (CB.TimeReturned == null)
                {
                    CarBooking = CB;
                    break;
                }
            }
            CarBooking = await _context.CarBookings.Include(c => c.Customer).FirstOrDefaultAsync(m => m.BookingId == CarBooking.BookingId);
            Customer = CarBooking.Customer;

            ViewData["KilometerReading"] = new SelectList(_context.CarBookings, "KilometerReading", "KilometerReading");

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            int KmReading = (int)CarBooking.KilometerReading;
            RentalCar = await _context.RentalCars.Include(c => c.CarBookings).FirstOrDefaultAsync(m => m.RegisterId == CarId);

            foreach (CarBooking CB in RentalCar.CarBookings)
            {
                if (CB.TimeReturned == null)
                {
                    CarBooking = CB;
                    break;
                }
            }
            CarBooking = await _context.CarBookings.Include(c => c.Customer).FirstOrDefaultAsync(m => m.BookingId == CarBooking.BookingId);
            Customer = CarBooking.Customer;

            if (CarBooking.TimeReturned == null)
            {
                CarBooking.TimeReturned = DateTime.Now;
            }
            CarBooking.KilometerReading = KmReading;
            CarBooking.Kost = CalculateKost(CarBooking);
            if(CarBooking.Kost ==-1)
            {
                return Page();
            }
            _context.Attach(CarBooking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarBookingExists(CarBooking.BookingId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CarBookingExists(long id)
        {
            return _context.CarBookings.Any(e => e.BookingId == id);
        }

        public long CalculateKost(CarBooking CB)
        {
            long price = 0;
            TimeSpan dateDiff = (TimeSpan)(CB.TimeReturned - CB.TimeRented);

            if (dateDiff.TotalDays < 0)
            {
                return -1;
            }
            double baseDayPrice = 200;
            double baseKmPrice = 10;
            if (CB.Register.CarType.Contains("Small Car"))
            {
                price = (long)(baseDayPrice * dateDiff.TotalDays);
            }
            else if (CB.Register.CarType.Contains("Kombi"))
            {
                price = (long)(baseDayPrice * dateDiff.TotalDays * 1.3 + baseKmPrice * CB.KilometerReading);
            }
            else if (CB.Register.CarType.Contains("Truck"))
            {
                price = (long)(baseDayPrice * dateDiff.TotalDays * 1.5 + baseKmPrice * CB.KilometerReading *1.5);
            }

                return price;
        }
    }
}
