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
        Task<Domain.Car> GetCarById(Guid carId);

        Task<IEnumerable<Domain.Car>> GetCarsByFilter(string filter);

        Task<Booking> BookCar(Guid id, DateTime startDate, DateTime endDate);
    }
}
