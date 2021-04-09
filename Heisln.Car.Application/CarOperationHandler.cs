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


        public CarOperationHandler(ICarRepository carRepository, IBookingRepository bookingRepository)
        {
            this.carRepository = carRepository;
            this.bookingRepository = bookingRepository;
        }

        public async Task<Booking> BookCar(Guid carId, DateTime startDate, DateTime endDate)
        {
            var car = await carRepository.GetAsync(carId);
            var booking = Booking.Create(car, startDate, endDate);
            bookingRepository.Add(booking);
            await bookingRepository.SaveAsync();
            return booking;
        }

        public async Task<Domain.Car> GetCarById(Guid carId)
        {
            var car = await carRepository.GetAsync(carId);
            return car;
        }

        public async Task<IEnumerable<Domain.Car>> GetCarsByFilter(string filter)
        {
            var cars = await carRepository.GetAllAsync();
            return cars;
        }
    }
}
