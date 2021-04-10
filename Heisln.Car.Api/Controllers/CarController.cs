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

        /// <summary>
        /// book a car
        /// </summary>
        /// <param name="booking">startDate has to be before endDate</param>
        /// <response code="200">got cars</response>
        /// <response code="401">unauthorized</response>
        /// <response code="422">invalid booking</response>
        [HttpPost("book")]
        [Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
        [ValidateModelState]
        [SwaggerResponse(statusCode: 401, type: typeof(ErrorObject), description: "unauthorized")]
        [SwaggerResponse(statusCode: 422, type: typeof(ErrorObject), description: "invalid booking")]
        public async virtual Task<IActionResult> BookCar([FromBody] Booking booking)
        {
            var result = await carOperationHandler.BookCar(booking.CarId, booking.StartDate, booking.EndDate);
            return new ObjectResult(result);
        }

        /// <summary>
        /// get all cars
        /// </summary>
        /// <param name="query"></param>
        /// <response code="200">got cars</response>
        [HttpGet]
        [Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
        [ValidateModelState]
        public async virtual Task<IActionResult> GetCars(string query)
        {
            var result = await carOperationHandler.GetCarsByFilter(query);
            return new ObjectResult(result.Select(car => car.ToApiInfoModel()));
        }

        /// <summary>
        /// get car by id
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">got car details</response>
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
        [ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(Car.Domain.Car), description: "got car details")]
        public async virtual Task<IActionResult> GetCar(Guid id)
        {
            var result = await carOperationHandler.GetCarById(id);
            return new ObjectResult(result.ToApiModel());
        }
    }
}
