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
    public class IndexModel : PageModel
    {
        private readonly ActiveCarRental.Data.RentalContext _context;

        public IndexModel(ActiveCarRental.Data.RentalContext context)
        {            
            _context = context;

        }

        public IList<RentalCar> RentalCars { get;set; }

        public async Task OnGetAsync()
        {
            RentalCars = await _context.RentalCars.Include(c => c.CarBookings).ToListAsync();
            //RentalCar = new List<RentalCar>();
        }
    }
}
