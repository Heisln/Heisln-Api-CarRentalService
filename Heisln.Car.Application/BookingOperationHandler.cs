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
        readonly IUserRepository userRepository;

        public BookingOperationHandler(IBookingRepository bookingRepository, IUserRepository userRepository)
        {
            this.bookingRepository = bookingRepository;
            this.userRepository = userRepository;
        }

        public async Task<Booking> GetBookingById(Guid bookingId)
        {
            return await bookingRepository.GetAsync(bookingId);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByUser(Guid userId)
        {
            return await bookingRepository.GetBookingsByUser(userId);
        }
    }
}
