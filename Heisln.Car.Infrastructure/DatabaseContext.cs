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
                .Property("Password");
        }
    }
}
