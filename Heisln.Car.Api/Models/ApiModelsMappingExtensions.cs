using Heisln.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heisln.Car.Api.Models
{
    public static class ApiModelsMappingExtensions
    {
        public static CarInfo ToApiInfoModel(this Domain.Car car)
            => new CarInfo
            {
                Id = car.Id,
                Name = car.Name,
                Brand = car.Brand,
                Priceperday = car.Priceperday
            };

        public static Heisln.Api.Models.Car ToApiModel(this Domain.Car car)
            => new Heisln.Api.Models.Car
            {
                Id = car.Id,
                Name = car.Name,
                Brand = car.Brand,
                Priceperday = car.Priceperday,
                Horsepower = car.Horsepower,
                Consumption = car.Consumption
            };

        public static Heisln.Api.Models.Booking ToApiModel(this Domain.Booking booking)
            => new Heisln.Api.Models.Booking
            {
                Id = booking.Id,
                Car = booking.Car.ToApiInfoModel(),
                UserId = booking.User,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate
            };
    }
}
