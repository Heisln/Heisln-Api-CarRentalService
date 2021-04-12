using Heisln.Api.Attributes;
using Heisln.Api.Models;
using Heisln.Api.Security;
using Heisln.Car.Api.Models;
using Heisln.Car.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Heisln.Api.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserOperationHandler userOperationHandler;
        IBookingOperationHandler bookingOperationHandler;

        public UserController(IUserOperationHandler userOperationHandler, IBookingOperationHandler bookingOperationHandler)
        {
            this.userOperationHandler = userOperationHandler;
            this.bookingOperationHandler = bookingOperationHandler;
        }

        [HttpPost("login")]
        public async Task<AuthenticationResponse> UserLogin(AuthenticationRequest request)
        {
            var result = await userOperationHandler.Login(request.Email, request.Password);
            return new AuthenticationResponse { Token = result.Item1, UserId = result.Item2 };
        }

        [HttpPost("registration")]
        public async Task<AuthenticationResponse> RegistrateUser([FromBody] User newUser)
        {
            var result = await userOperationHandler.Register(newUser.Email, newUser.Password, newUser.FirstName, newUser.LastName, newUser.Birthday);
            return new AuthenticationResponse() { Token = result.Item1, UserId = result.Item2 } ;
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
