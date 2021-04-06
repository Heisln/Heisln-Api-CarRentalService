using System;
using System.Runtime.Serialization;
using System.Text;

namespace Heisln.Car.Domain
{
    public class Booking
    {
        public readonly Guid Id;

        public readonly Guid CarId;

        public readonly DateTime StartDate;

        public readonly DateTime EndDate;

        public Booking(Guid id, Guid carId, DateTime startDate, DateTime endDate)
        {
            Id = id;
            CarId = carId;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
