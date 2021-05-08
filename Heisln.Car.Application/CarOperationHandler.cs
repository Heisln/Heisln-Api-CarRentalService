using Heisln.Car.Contract;
using Heisln.Car.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Heisln.Car.Application
{
    public class CarOperationHandler : ICarOperationHandler
    {
        readonly ICarRepository carRepository;
        readonly IBookingRepository bookingRepository;
        readonly ICurrencyConverterHandler currencyConverterHandler;
        const string emailClaim = "email";


        public CarOperationHandler(ICarRepository carRepository, IBookingRepository bookingRepository, ICurrencyConverterHandler currencyConverterHandler)
        {
            this.carRepository = carRepository;
            this.bookingRepository = bookingRepository;
            this.currencyConverterHandler = currencyConverterHandler;
        }

        public async Task<Booking> BookCar(Guid carId, Guid userId, DateTime startDate, DateTime endDate, string bearer)
        {
            var car = await carRepository.GetAsync(carId);
            var user = await userRepository.GetAsync(userId); //TODO: Get User here?

            var claim = JWTTokenGenerator.GetClaim(bearer, emailClaim);
            if (user.Email != claim)
                throw new InvalidCredentialException("Not authorized!");

            var booking = Booking.Create(car, user, startDate, endDate);
            bookingRepository.Add(booking);
            await bookingRepository.SaveAsync();
            return booking;
        }

        public async Task<Domain.Car> GetCarById(Guid carId, string currency)
        {
            var car = await carRepository.GetAsync(carId);
            var converted = await currencyConverterHandler.Convert(currency, car.Priceperday);
            car.Priceperday = converted;
            return car;
        }

        public async Task<IEnumerable<Domain.Car>> GetCarsByFilter(string filter, string currency)
        {
            var cars = (await carRepository.GetAllAsync()).ToList();
            cars = cars.Where(c => c.ToString().Contains(filter)).ToList();
            var converted = await currencyConverterHandler.Convert(currency, cars.Select(car => car.Priceperday).ToList());
            for(int i = 0; i < cars.Count(); i++)
            {
                cars[i].Priceperday = converted[i];
            }
            return cars;
        }

        public async Task ReturnCar(Guid bookingId, string bearer)
        {
            var booking = await bookingRepository.GetAsync(bookingId);

            var claim = JWTTokenGenerator.GetClaim(bearer, emailClaim);
            if (booking.User.Email != claim)
                throw new InvalidCredentialException("Not authorized!");

            bookingRepository.Remove(booking);
            await bookingRepository.SaveAsync();
        }
    }
}
