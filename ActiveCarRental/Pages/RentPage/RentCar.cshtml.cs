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
using Microsoft.EntityFrameworkCore;

namespace ActiveCarRental.Pages.RentPage
{
    public class RentCarModel : PageModel
    {
        private readonly ActiveCarRental.Data.RentalContext _context;

        public RentCarModel(ActiveCarRental.Data.RentalContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            RentalCar = await _context.RentalCars.FirstOrDefaultAsync(m => m.RegisterId == id);
            
            return Page();
        }
        public RentalCar RentalCar { get; set; }
        [BindProperty]
        public CarBooking CarBooking { get; set; }
        [BindProperty]
        public Customer Customer { get; set; }
        public List<CarBooking> TempCarBookings { get; private set; }

        public List<Customer> TempCustomers { get; private set; } 

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            TempCustomers = await _context.Customers.ToListAsync();

            CarBooking.BookingId = GenrateID(await _context.CarBookings.ToListAsync());
            CarBooking.KilometerReading = 0;
            CarBooking.RegisterId = RentalCar.RegisterId;
            CarBooking.Register = RentalCar;

            if(CarBooking.TimeRented == null)
            {
                CarBooking.TimeRented = DateTime.Now;
            }

            if (CustomerDontExists(Customer))
            {
                
                Customer.CustomerID = GenrateID(TempCustomers);
                CarBooking.CustomerId = Customer.CustomerID;
                _context.Customers.Add(Customer);
                await _context.SaveChangesAsync();
            }
            else
            {
                CarBooking.CustomerId = Customer.CustomerID;
                CarBooking.Customer = Customer;
            }

            _context.CarBookings.Add(CarBooking);
            await _context.SaveChangesAsync();



            return RedirectToPage("./Index");
        }

        public bool CustomerDontExists(Customer Cust)
        {
            foreach (Customer C in _context.Customers)
            {
                if (C.SSN == Customer.SSN)
                {
                    Customer = C;
                    return false;
                }
            }

            return true;
        }

        public long GenrateID(List<Customer> CL)
        {
            long id = 1;
            if (CL.Count > 0)
            {
                foreach (Customer C in CL)
                {
                    if(C.CustomerID == id)
                    {
                        id++;
                    }
                    else
                    {
                        return id;
                    }
                
                }
            }
            return id;
        }
        public long GenrateID(List<CarBooking> CB)
        {
            long id = 1;
            if(CB.Count > 0)
            {
                foreach (CarBooking C in CB)
                {
                    if (C.BookingId == id)
                    {
                        id++;
                    }
                    else
                    {
                        return id;
                    }

                }
            }
            
            return id;
        }
    }
}
