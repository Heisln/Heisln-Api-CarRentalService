using Heisln.Car.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heisln.Car.Infrastructure
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Domain.Car> Cars { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DatabaseContext() : base()
        {

        }

        public DatabaseContext(DbContextOptions options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            base.OnConfiguring(options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(a => a.Id);
            modelBuilder.Entity<User>()
                .Property(a => a.FirstName);
            modelBuilder.Entity<User>()
                .Property(a => a.LastName);
            modelBuilder.Entity<User>()
                .Property(u => u.Email);
            modelBuilder.Entity<User>()
                .Property(u => u.Birthday);
            modelBuilder.Entity<User>()
                .Property("Password");

            modelBuilder.Entity<Booking>()
                .HasKey(a => a.Id);
            modelBuilder.Entity<Booking>()
                .HasOne(a => a.Car)
                .WithMany();
            modelBuilder.Entity<Booking>()
                .Property(a => a.User);
            modelBuilder.Entity<Booking>()
                .Property(a => a.StartDate);
            modelBuilder.Entity<Booking>()
                .Property(a => a.EndDate);

            modelBuilder.Entity<Domain.Car>()
               .HasKey(a => a.Id);
            modelBuilder.Entity<Domain.Car>()
                .Property(c => c.Brand);
            modelBuilder.Entity<Domain.Car>()
                .Property(a => a.Consumption);
            modelBuilder.Entity<Domain.Car>()
                .Property(a => a.Horsepower);
            modelBuilder.Entity<Domain.Car>()
                .Property(a => a.Name);
            modelBuilder.Entity<Domain.Car>()
                .Property(a => a.Priceperday);

        }
    }
}
