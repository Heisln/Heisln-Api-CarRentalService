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
        public async Task<Booking> BookCar([FromBody] Booking booking, [FromHeader] string authorization)
        {
            var result = await carOperationHandler.BookCar(booking.CarId.Value, booking.UserId, booking.StartDate, booking.EndDate, authorization.Substring(7));
            return result.ToApiModel();
        }

        [HttpPost("return")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task ReturnCar(Guid bookingId, [FromHeader] string authorization)
        {
            await carOperationHandler.ReturnCar(bookingId, authorization.Substring(7));
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IEnumerable<CarInfo>> GetCars(string query, Currency currency = Currency.USD)
        {
            var result = await carOperationHandler.GetCarsByFilter(query, currency.ToString());
            return result.Select(car => car.ToApiInfoModel());
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<Api.Models.Car> GetCar(Guid id, Currency currency = Currency.USD)
        {
            var result = await carOperationHandler.GetCarById(id, currency.ToString());
            return result.ToApiModel();
        }
    }
}
