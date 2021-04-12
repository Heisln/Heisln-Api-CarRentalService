using Heisln.Api.Attributes;
using Heisln.Api.Models;
using Heisln.Car.Api.Models;
using Heisln.Car.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace Heisln.Car.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        readonly IBookingOperationHandler bookingOperationHandler;

        public BookingController(IBookingOperationHandler bookingOperationHandler)
        {
            this.bookingOperationHandler = bookingOperationHandler;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<Booking> GetBooking(Guid id, string currency = "USD")
        {
            var result = await bookingOperationHandler.GetBookingById(id, currency);
            return result.ToApiModel();
        }
    }
}
