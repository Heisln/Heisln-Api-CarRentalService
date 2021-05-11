using System;
using System.Runtime.Serialization;
using System.Text;

namespace Heisln.Car.Domain
{
    public class Booking
    {
        public readonly Guid Id;

        public readonly Car Car;

        public readonly DateTime StartDate;

        public readonly DateTime EndDate;

        public Guid User { get; private set; }

        private Booking(Guid id, Car car, User user, DateTime startDate, DateTime endDate)
        {
            Id = id;
            Car = car;
            User = user.Id;
            StartDate = startDate;
            EndDate = endDate;
        }

        public static Booking Create(Car car, User user, DateTime startDate, DateTime endDate)
        {
            var newId = Guid.NewGuid();
            return new Booking(newId, car, user, startDate, endDate);
        }

        public Booking() { }

        public void UpdateUser(User user)
        {
        }
    }
}
