using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heisln.Car.Application
{
    public interface IBookingOperationHandler
    {
        Task<Domain.Booking> GetBookingById(Guid bookingId, string? currency);

        Task<IEnumerable<Domain.Booking>> GetBookingsByUser(Guid userId, string? currency);

        Task<Domain.Booking> GetBoookingFromUser(Guid userId, Guid bookingI, string? currency);
    }
}
