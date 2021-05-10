using Heisln.Car.Contract;
using Heisln.Car.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heisln.Car.Application
{
    public class BookingOperationHandler : IBookingOperationHandler
    {
        readonly IBookingRepository bookingRepository;

        public BookingOperationHandler(IBookingRepository bookingRepository)
        {
            this.bookingRepository = bookingRepository;
        }

        public async Task<Booking> GetBookingById(Guid bookingId, string currency)
        {
            return await bookingRepository.GetAsync(bookingId);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByUser(Guid userId, string currency)
        {
            return await bookingRepository.GetBookingsByUser(userId);
        }

        public async Task<Booking> GetBookingFromUser(Guid userId, Guid bookingId, string currency)
        {
            //TODO Validate
            return await bookingRepository.GetAsync(bookingId);
        }
    }
}
