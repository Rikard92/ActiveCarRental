#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ActiveCarRental.Models;

namespace ActiveCarRental.Data
{
    public class RentalContext : DbContext
    {
        public RentalContext (DbContextOptions<RentalContext> options)
            : base(options)
        {
            
        }

        public DbSet<RentalCar> RentalCars { get; set; }

        public DbSet<Customer> Customers { get; set;}

        public DbSet<CarBooking> CarBookings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarBooking>(entity =>
            {
                entity.HasKey(e => e.BookingId)
                    .HasName("PK_AllRentals");

                entity.Property(e => e.BookingId)
                    .ValueGeneratedNever()
                    .HasColumnName("BookingID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.TimeRented).HasColumnType("datetime");

                entity.Property(e => e.TimeReturned).HasColumnType("datetime");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CarBookings)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_AllRentals_Customers");

                entity.HasOne(d => d.Register)
                    .WithMany(p => p.CarBookings)
                    .HasForeignKey(d => d.RegisterId)
                    .HasConstraintName("FK_AllRentals_RentalCars");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerID);

                entity.Property(e => e.CustomerID)
                    .ValueGeneratedNever()
                    .HasColumnName("CustomerID");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(15)
                    .IsFixedLength();

                entity.Property(e => e.LastName)
                    .HasMaxLength(15)
                    .IsFixedLength();

                entity.Property(e => e.SSN).HasColumnName("SSN");
            });

            modelBuilder.Entity<RentalCar>(entity =>
            {
                entity.HasKey(e => e.RegisterId)
                    .HasName("PK_RentalCar");

                entity.Property(e => e.RegisterId).ValueGeneratedNever();

                entity.Property(e => e.CarType)
                    .HasMaxLength(15)
                    .IsFixedLength();
            });

            //    OnModelCreatingPartial(modelBuilder);
        }
    }
}
