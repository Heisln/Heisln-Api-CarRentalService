using Heisln.Car.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heisln.ApiTest
{
    public class DbContextFixture : IDisposable
    {
        readonly string connectionString = "DataSource=:memory:";
        readonly SqliteConnection connection;
        public DatabaseContext DatabaseContext { get; }
        public Car.Domain.Car[] Cars { get; init; }
        public Car.Domain.User[] Users { get; init; }
        public Car.Domain.Booking[] Bookings { get; init; }

        public DbContextFixture()
        {
            connection = new SqliteConnection(connectionString);
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseSqlite(connection)
                .Options;
            DatabaseContext = new DatabaseContext(options);

            DatabaseContext.Database.OpenConnection();
            DatabaseContext.Database.Migrate();
            DatabaseContext.Database.EnsureCreated();

            Cars = new Car.Domain.Car[]
            {
                new Car.Domain.Car(Guid.NewGuid(), "BWM", "43e", 77, 2.0, 1.0),
                new Car.Domain.Car(Guid.NewGuid(), "BWM", "3er", 100, 12.0, 23.0),
                new Car.Domain.Car(Guid.NewGuid(), "Audi", "A5", 120, 2.0, 11.0),
                new Car.Domain.Car(Guid.NewGuid(), "BWM", "A4", 40, 12.0, 32.0)
            };

            Users = new Car.Domain.User[]
            {
                new Car.Domain.User(Guid.NewGuid(), "hans.peter@yahoo.at", "asdfg", "Hans", "Peter", DateTime.Now),
                new Car.Domain.User(Guid.NewGuid(), "gustav.nimmersatt@yahoo.at", "asdfg123", "Gustav", "Nimmersatt", DateTime.Now)
            };

            Bookings = new Car.Domain.Booking[]
            {
                Car.Domain.Booking.Create(Cars[0], Users[0], new DateTime(2022, 1, 1), new DateTime(2022, 1, 12)),
                Car.Domain.Booking.Create(Cars[1], Users[0], new DateTime(2021, 1, 1), new DateTime(2021, 1, 12)),
                Car.Domain.Booking.Create(Cars[2], Users[0], new DateTime(2020, 1, 1), new DateTime(2020, 1, 12))
            };

            SeedDatabase();
        }

        void SeedDatabase()
        {
            // Fill with cars
            DatabaseContext.Cars.Add(Cars[0]);
            DatabaseContext.Cars.Add(Cars[1]);
            DatabaseContext.Cars.Add(Cars[2]);
            DatabaseContext.Cars.Add(Cars[3]);

            // Fill with user
            DatabaseContext.Users.Add(Users[0]);
            DatabaseContext.Users.Add(Users[1]);

            // Fill bookings
            DatabaseContext.Bookings.Add(Bookings[0]);
            DatabaseContext.Bookings.Add(Bookings[1]);
            DatabaseContext.Bookings.Add(Bookings[2]);

            DatabaseContext.SaveChanges();
        }

        

        public void Dispose()
        {
            DatabaseContext.Dispose();
            connection.Close();
        }
    }
}
