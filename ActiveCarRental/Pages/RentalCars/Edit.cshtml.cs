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

namespace ActiveCarRental.Pages.RentalCars
{
    public class EditModel : PageModel
    {
        private readonly ActiveCarRental.Data.RentalContext _context;

        public EditModel(ActiveCarRental.Data.RentalContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RentalCar RentalCar { get; set; }
        public List<string> CarTypes { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CarTypes = new List<string>();
            CarTypes.Add("Small Car");
            CarTypes.Add("Kombi");
            CarTypes.Add("Truck");

            RentalCar = await _context.RentalCars.FirstOrDefaultAsync(m => m.RegisterId == id);

            if (RentalCar == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(RentalCar).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentalCarExists(RentalCar.RegisterId))
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

        private bool RentalCarExists(int id)
        {
            return _context.RentalCars.Any(e => e.RegisterId == id);
        }
    }
}
