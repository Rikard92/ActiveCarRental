namespace ActiveCarRental.Models
{
    public class Customer
    {
        public Customer()
        {
            CarBookings = new HashSet<CarBooking>();
        }

        public long CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long SSN { get; set; }
        public virtual ICollection<CarBooking> CarBookings { get; set; }

    }
}
