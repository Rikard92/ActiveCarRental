using System.ComponentModel.DataAnnotations;

namespace ActiveCarRental.Models
{
    public class CarBooking
    {
        [Key]
        public long BookingId { get; set; }
        public int? RegisterId { get; set; }
        public DateTime? TimeRented { get; set; }
        public DateTime? TimeReturned { get; set; }
        public int? KilometerReading { get; set; }
        public long? CustomerId { get; set; }
        public long? Kost { get; set; }
        

        public virtual Customer? Customer { get; set; }
        public virtual RentalCar? Register { get; set; }
    }
}
