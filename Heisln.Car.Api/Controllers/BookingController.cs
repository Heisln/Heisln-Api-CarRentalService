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
using System.Web.Http.Cors;

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
        public async Task<Booking> GetBooking(Guid id, Currency currency = Currency.USD)
        {
            var result = await bookingOperationHandler.GetBookingById(id, currency.ToString());
            return result.ToApiModel();
        }

        [HttpGet("{userId}/bookings/{bookingId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<Booking> GetBooking(Guid userId, Guid bookingId, Currency currency = Currency.USD)
        {
            var result = await bookingOperationHandler.GetBookingFromUser(userId, bookingId, currency.ToString());
            return result.ToApiModel();
        }

        [HttpGet("{userId}/bookings")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IEnumerable<Booking>> GetBookings(Guid userId, Currency currency = Currency.USD)
        {
            var result = await bookingOperationHandler.GetBookingsByUser(userId, currency.ToString());
            return result.Select(booking => booking.ToApiModel());
        }
    }
}
