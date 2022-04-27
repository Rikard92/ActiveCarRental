using System.ComponentModel.DataAnnotations;

namespace ActiveCarRental.Models
{
    public class RentalCar
    {
        public RentalCar()
        {
            CarBookings = new HashSet<CarBooking>();
        }
        [Key]
        public int RegisterId { get; set; }
        public string? CarType { get; set; }
        public virtual ICollection<CarBooking> CarBookings { get; set; }
    }
}
