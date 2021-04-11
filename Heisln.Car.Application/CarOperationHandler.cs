using Heisln.Car.Contract;
using Heisln.Car.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heisln.Car.Application
{
    public class CarOperationHandler : ICarOperationHandler
    {
        readonly ICarRepository carRepository;
        readonly IBookingRepository bookingRepository;
        readonly IUserRepository userRepository;


        public CarOperationHandler(ICarRepository carRepository, IBookingRepository bookingRepository, IUserRepository userRepository)
        {
            this.carRepository = carRepository;
            this.bookingRepository = bookingRepository;
            this.userRepository = userRepository;
        }

        public async Task<Booking> BookCar(Guid carId, Guid userId, DateTime startDate, DateTime endDate)
        {
            var car = await carRepository.GetAsync(carId);
            var user = await userRepository.GetAsync(userId);
            var booking = Booking.Create(car, user, startDate, endDate);
            bookingRepository.Add(booking);
            await bookingRepository.SaveAsync();
            return booking;
        }

        public async Task<Domain.Car> GetCarById(Guid carId, string currency)
        {
            var car = await carRepository.GetAsync(carId);
            return car;
        }

        public async Task<IEnumerable<Domain.Car>> GetCarsByFilter(string filter, string currency)
        {
            var cars = await carRepository.GetAllAsync();
            return cars;
        }
    }
}
