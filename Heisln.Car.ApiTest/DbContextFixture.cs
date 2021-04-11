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

            SeedDatabase();
        }


        void SeedDatabase()
        {
            // Fill with cars
            DatabaseContext.Cars.Add(new Car.Domain.Car(Guid.NewGuid(), "BWM", "43e", 77, 2.0, 1.0));
            DatabaseContext.Cars.Add(new Car.Domain.Car(Guid.NewGuid(), "BWM", "3er", 100, 12.0, 23.0));

            // Fill with user
            DatabaseContext.Users.Add(new Car.Domain.User(Guid.NewGuid(), "hans.peter@yahoo.at", "asdfg", "Hans", "Peter", DateTime.Now));
            DatabaseContext.Users.Add(new Car.Domain.User(Guid.NewGuid(), "gustav.nimmersatt@yahoo.at", "asdfg123", "Gustav", "Nimmersatt", DateTime.Now));

            DatabaseContext.SaveChanges();
        }

        public void Dispose()
        {
            DatabaseContext.Dispose();
            connection.Close();
        }
    }
}
