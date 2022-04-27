#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ActiveCarRental.Data;
using ActiveCarRental.Models;

namespace ActiveCarRental.Pages.RentalCars
{
    public class CreateModel : PageModel
    {
        private readonly ActiveCarRental.Data.RentalContext _context;

        public CreateModel(ActiveCarRental.Data.RentalContext context)
        {
            _context = context;
        }

        public int TypeId { get; set; }
        public List<string> CarTypes { get; set; }

        public IActionResult OnGet()
        {
            CarTypes = new List<string>();
            CarTypes.Add("Small Car");
            CarTypes.Add("Kombi");
            CarTypes.Add("Truck");
            return Page();
        }

        [BindProperty]
        public RentalCar RentalCar { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.RentalCars.Add(RentalCar);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
