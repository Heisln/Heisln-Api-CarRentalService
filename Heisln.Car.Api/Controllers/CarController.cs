using Heisln.Api.Attributes;
using Heisln.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Heisln.Api.Security;
using System.ComponentModel.DataAnnotations;
using Heisln.Car.Application;
using Heisln.Car.Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Heisln.Api.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class CarController : ControllerBase
    {
        ICarOperationHandler carOperationHandler;

        public CarController(ICarOperationHandler carOperationHandler)
        {
            this.carOperationHandler = carOperationHandler;
        }

        [HttpPost("book")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<Booking> BookCar([FromBody] Booking booking)
        {
            var result = await carOperationHandler.BookCar(booking.CarId.Value, booking.UserId, booking.StartDate, booking.EndDate);
            return result.ToApiModel();
        }

        [HttpPost("return")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task ReturnCar(Guid bookingId)
        {
            await carOperationHandler.ReturnCar(bookingId);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IEnumerable<CarInfo>> GetCars(string query, string currency = "USD")
        {
            var result = await carOperationHandler.GetCarsByFilter(query, currency);
            return result.Select(car => car.ToApiInfoModel());
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<Api.Models.Car> GetCar(Guid id, string currency = "USD")
        {
            var result = await carOperationHandler.GetCarById(id, currency);
            return result.ToApiModel();
        }
    }
}
