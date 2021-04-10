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

namespace Heisln.ApiTest
{
    public class CarControllerTest : IClassFixture<DbContextFixture>
    {
        CarController carController;

        public CarControllerTest(DbContextFixture dbContextFixture)
        {
            var dbContext = dbContextFixture.DatabaseContext;
            carController = new CarController(
                new CarOperationHandler(
                    carRepository: new CarRepository(dbContext), 
                    bookingRepository: new BookingRepository(dbContext)
                )
            );
        }

        [Fact]
        public async Task GetCars_WhenGetListOfCars_ThenCarsCountIsGreaterThanZero()
        {
            // When
            var result = await carController.GetCars("");
            var objectResult = result as ObjectResult;
            var cars = ((IEnumerable<CarInfo>) objectResult.Value).ToList();

            // Then
            cars.Should().HaveCountGreaterOrEqualTo(0);
        }

        [Fact]
        public async Task GetCar_GivenCarId_WhenFindCarById_ThenReceiveSpecificCar()
        {
            // Given
            var resultAllCars = (ObjectResult) await carController.GetCars("");
            var expectedCar = ((IEnumerable<CarInfo>)resultAllCars.Value).ToList().First();
            var id = expectedCar.Id;

            // When
            var resultSpecificCar = (ObjectResult) await carController.GetCar(id);
            var specificCar = (Heisln.Api.Models.Car) resultSpecificCar.Value;

            // Then
            specificCar.Id.Should().Be(expectedCar.Id, "because they have the same id");
        }

        [Fact]
        public async Task GetCar_GivenEmptyId_WhenFindCarById_ThenItShouldFail()
        {
            var id = Guid.Empty;
            await Assert.ThrowsAnyAsync<Exception>(() => carController.GetCar(id));

        }

        public static readonly IEnumerable<object[]> dateTimes = new List<object[]>
        {
            new object[] 
            { 
                new DateTime(2021, 1, 1),
                new DateTime(2021, 1, 12)
            }
        };

        [Theory]
        [MemberData(nameof(dateTimes))]
        public async Task BookCar_GivenANewBooking_WhenBookSpecificCar_ThenSpecificCarShouldBeBooked(DateTime startDate, DateTime endDate)
        {
            // Given
            var resultAllCars = (ObjectResult)await carController.GetCars("");
            var expectedCar = ((IEnumerable<CarInfo>)resultAllCars.Value).ToList().First();
            var expectedBooking = CreateBooking(expectedCar.Id, startDate, endDate);

            // When
            var resultFromBooking = (ObjectResult)await carController.BookCar(expectedBooking);
            var booking = (Heisln.Car.Domain.Booking)resultFromBooking.Value;
            var car = booking.Car;

            // Then
            car.Id.Should().Be(expectedBooking.CarId, "because I booked this car!");
        }

        private Booking CreateBooking(Guid id, DateTime startDate, DateTime endDate)
        {
            return new Booking()
            {
                CarId = id,
                StartDate = startDate,
                EndDate = endDate
            };
        }

        [Fact]
        public async Task BookCar_GivenEmptyBooking_WhenBookSpecificCar_ThenItShouldFail()
        {
            var booking = new Booking();
            await Assert.ThrowsAnyAsync<Exception>(() => carController.BookCar(booking));
        }
    }
}
