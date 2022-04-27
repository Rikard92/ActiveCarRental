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
    public class IndexModel : PageModel
    {
        private readonly ActiveCarRental.Data.RentalContext _context;

        public IndexModel(ActiveCarRental.Data.RentalContext context)
        {
            _context = context;
        }

        public IList<CarBooking> CarBookings { get;set; }

        public IList<RentalCar> RentalCars { get; set; }

        public async Task OnGetAsync()
        {
            CarBookings = await _context.CarBookings
                .Include(c => c.Customer)
                .Include(c => c.Register).ToListAsync();
            RentalCars = await _context.RentalCars.Include(c => c.CarBookings).ToListAsync();

            
        }

        public bool HasReturndDate(RentalCar RC)
        {
            foreach (CarBooking CB in RC.CarBookings)
            {
                if(CB.TimeReturned== null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
