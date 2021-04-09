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

        private Booking(Guid id, Car car, DateTime startDate, DateTime endDate)
        {
            Id = id;
            Car = car;
            StartDate = startDate;
            EndDate = endDate;
        }

        public static Booking Create(Car car, DateTime startDat, DateTime endDate)
        {
            var newId = Guid.NewGuid();
            return new Booking(newId, Car)
        }
    }
}
