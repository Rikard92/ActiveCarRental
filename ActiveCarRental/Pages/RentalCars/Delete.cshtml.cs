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
    public class DeleteModel : PageModel
    {
        private readonly ActiveCarRental.Data.RentalContext _context;

        public DeleteModel(ActiveCarRental.Data.RentalContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RentalCar RentalCar { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RentalCar = await _context.RentalCars.FirstOrDefaultAsync(m => m.RegisterId == id);

            if (RentalCar == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RentalCar = await _context.RentalCars.FindAsync(id);

            if (RentalCar != null)
            {
                _context.RentalCars.Remove(RentalCar);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
