using Heisln.Car.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;
using Microsoft.EntityFrameworkCore.InMemory;
using Heisln.Car.Contract;
using Heisln.Car.Application;
using Heisln.Api.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using FluentAssertions;
using Heisln.Api.Models;
using Heisln.ApiTest.Mock;

namespace Heisln.ApiTest
{
    public class CarControllerTest : IClassFixture<DbContextFixture>
    {
        double convertFactor = 1.5;
        CarController carController;
        UserController userController;

        public CarControllerTest(DbContextFixture dbContextFixture)
        {
            var databaseContext = dbContextFixture.DatabaseContext;
            carController = new CarController(
                new CarOperationHandler(
                    carRepository: new CarRepository(databaseContext), 
                    bookingRepository: new BookingRepository(databaseContext),
                    userRepository: new UserRepository(databaseContext),
                    currencyConverterHandler: new MockCurrencyConverterHandler(convertFactor)
                )
            );

            userController = new UserController(
                new UserOperationHandler(
                    userRepository: new UserRepository(databaseContext)
                ),
                new BookingOperationHandler(
                    bookingRepository: new BookingRepository(databaseContext), 
                    userRepository: new UserRepository(databaseContext)
                )
            );
        }

        [Fact]
        public async Task GetCars_WhenGetListOfCars_ThenCarsCountIsGreaterThanZero()
        {
            // When
            var result = await carController.GetCars("");
            var cars = result.ToList();

            // Then
            cars.Should().HaveCountGreaterOrEqualTo(0, "because the database stores a set of cars!");
        }

        [Fact]
        public async Task GetCar_GivenCarId_WhenFindCarById_ThenReceiveSpecificCar()
        {
            // Given
            var resultAllCars = await carController.GetCars("");
            var expectedCar = resultAllCars.ToList().First();
            var id = expectedCar.Id;

            // When
            var specificCar = await carController.GetCar(id);

            // Then
            specificCar.Id.Should().Be(expectedCar.Id, "because they have the same id!");
        }

        public static readonly IEnumerable<object[]> invalidId = new List<object[]>
        {
            new object[] { Guid.Empty },
            new object[] { Guid.NewGuid() }
        };

        [Theory]
        [MemberData(nameof(invalidId))]
        public async Task GetCar_GivenInvalidId_WhenFindCarById_ThenItShouldFail(Guid id)
        {
            await Assert.ThrowsAnyAsync<Exception>(() => carController.GetCar(id));
        }

        public static readonly IEnumerable<object[]> dateTimes = new List<object[]>
        {
            new object[] 
            {
                "gustav.nimmersatt@yahoo.at", 
                "asdfg123",
                new DateTime(2021, 1, 1),
                new DateTime(2021, 1, 12)
            }
        };

        [Theory]
        [MemberData(nameof(dateTimes))]
        public async Task BookCar_GivenANewBooking_WhenBookSpecificCar_ThenSpecificCarShouldBeBooked(string email, string password, DateTime startDate, DateTime endDate)
        {
            // Given
            var resultAllCars = await carController.GetCars("");
            var expectedCar = resultAllCars.ToList().First();
            var authenticationResponse = await userController.UserLogin(
                new AuthenticationRequest() { 
                    Email = email, 
                    Password = password 
                }
            );
            var expectedBooking = CreateBooking(expectedCar.Id, authenticationResponse.UserId, startDate, endDate);

            // When
            var booking = await carController.BookCar(expectedBooking);
            var car = booking.Car;

            // Then
            car.Id.Should().Be(expectedBooking.CarId.ToString(), "because I booked this car!");
        }

        private Booking CreateBooking(Guid carId, Guid userId, DateTime startDate, DateTime endDate)
        {
            return new Booking()
            {
                CarId = carId,
                UserId = userId,
                StartDate = startDate,
                EndDate = endDate
            };
        }

        public static readonly IEnumerable<object[]> invalidBookingInformation = new List<object[]>
        {
            new object[] { Guid.Empty, Guid.NewGuid(), new DateTime(2001, 1, 1), new DateTime(2001, 1, 12) },
            new object[] { Guid.NewGuid(), Guid.NewGuid(), new DateTime(2012, 1, 1), new DateTime(2020, 1, 12) }
        };

        [Theory]
        [MemberData(nameof(invalidBookingInformation))]
        public async Task BookCar_GivenInvalidBooking_WhenBookSpecificCar_ThenItShouldFail(Guid carId, Guid userId, DateTime startDate, DateTime endDate)
        {
            var booking = CreateBooking(carId, userId, startDate, endDate);
            await Assert.ThrowsAnyAsync<Exception>(() => carController.BookCar(booking));
        }
    }
}
