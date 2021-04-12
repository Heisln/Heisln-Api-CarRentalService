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
    /// <summary>
    /// 
    /// </summary>
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

        /// <summary>
        /// register a user
        /// </summary>
        /// <param name="newUser">Mail needs to be unique</param>
        /// <response code="200">registered user</response>
        /// <response code="409">login invalid</response>
        [HttpPost("login")]
        [ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(AuthenticationResponse), description: "registered user")]
        [SwaggerResponse(statusCode: 409, type: typeof(ErrorObject), description: "login invalid")]
        public async virtual Task<IActionResult> UserLogin(AuthenticationRequest request)
        {
            var result = await userOperationHandler.Login(request.Email, request.Password);
            return new ObjectResult(new AuthenticationResponse { Token = result.Item1, UserId = result.Item2 });
        }

        /// <summary>
        /// register a user
        /// </summary>
        /// <param name="newUser">Mail needs to be unique</param>
        /// <response code="200">registered user</response>
        /// <response code="409">registration invalid</response>
        [HttpPost("registration")]
        [ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(AuthenticationResponse), description: "registered user")]
        [SwaggerResponse(statusCode: 409, type: typeof(ErrorObject), description: "registration invalid")]
        public async virtual Task<IActionResult> RegistrateUser([FromBody] User newUser)
        {
            var result = await userOperationHandler.Register(newUser.Email, newUser.Password, newUser.FirstName, newUser.LastName, newUser.Birthday);
            return new ObjectResult(new AuthenticationResponse() { Token = result } );
        }

        [HttpGet("{userId}/bookings/{bookingId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModelState]
        public async virtual Task<IActionResult> GetBooking(Guid userId, Guid bookingId, string? currency)
        {
            var result = await bookingOperationHandler.GetBookingFromUser(userId, bookingId, currency);
            return new ObjectResult(result.ToApiModel()); 
        }

        [HttpGet("{userId}/bookings")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModelState]
        public async virtual Task<IActionResult> GetBookings(Guid userId, string? currency)
        {
            var result = await bookingOperationHandler.GetBookingsByUser(userId, currency);
            return new ObjectResult(result.Select(booking => booking.ToApiModel()));
        }
    }
}
