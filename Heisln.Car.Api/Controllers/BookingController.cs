using Heisln.Api.Attributes;
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
        [ValidateModelState]
        public async virtual Task<IActionResult> GetBooking(Guid id, string? currency)
        {
            var result = await bookingOperationHandler.GetBookingById(id, currency);
            return new ObjectResult(result);
        }
    }
}
