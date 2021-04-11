using Heisln.Car.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heisln.Car.Application
{
    public interface ICarOperationHandler
    {
        Task<Domain.Car> GetCarById(Guid carId, string? currency);

        Task<IEnumerable<Domain.Car>> GetCarsByFilter(string filter, string? currency);

        Task<Booking> BookCar(Guid carId, Guid userId, DateTime startDate, DateTime endDate);
    }
}
