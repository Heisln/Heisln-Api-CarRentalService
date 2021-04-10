using Heisln.Car.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heisln.ApiTest
{
    public class DbContextFixture
    {
        public DatabaseContext DatabaseContext { get; }

        public DbContextFixture()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "CarDatabase")
                .Options;
            DatabaseContext = new DatabaseContext(options);
            SeedDatabase();
        }


        void SeedDatabase()
        {
            DatabaseContext.Cars.Add(new Car.Domain.Car(Guid.NewGuid(), "BWM", "43e", 77, 2.0, 1.0));
            DatabaseContext.Cars.Add(new Car.Domain.Car(Guid.NewGuid(), "BWM", "3er", 100, 12.0, 23.0));
            DatabaseContext.SaveChanges();
        }
    }
}
