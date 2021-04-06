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

namespace Heisln.Api.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class CarApiController : ControllerBase
    {
        /// <summary>
        /// book a car
        /// </summary>
        /// <param name="booking">startDate has to be before endDate</param>
        /// <response code="200">got cars</response>
        /// <response code="401">unauthorized</response>
        /// <response code="422">invalid booking</response>
        [HttpPost]
        [Route("/car/book")]
        //[Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
        [ValidateModelState]
        [SwaggerOperation("ApiV1CarBookPost")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<Car>), description: "got cars")]
        [SwaggerResponse(statusCode: 401, type: typeof(ErrorObject), description: "unauthorized")]
        [SwaggerResponse(statusCode: 422, type: typeof(ErrorObject), description: "invalid booking")]
        public virtual IActionResult ApiV1CarBookPost([FromQuery] Booking booking)
        {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(List<Car>));

            //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(401, default(ErrorObject));

            //TODO: Uncomment the next line to return response 422 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(422, default(ErrorObject));
            string exampleJson = null;
            exampleJson = "[ {\n  \"horsepower\" : 0,\n  \"name\" : \"name\",\n  \"priceperday\" : 1.4658129805029452,\n  \"consumption\" : 6.027456183070403,\n  \"id\" : \"id\",\n  \"brand\" : \"brand\"\n}, {\n  \"horsepower\" : 0,\n  \"name\" : \"name\",\n  \"priceperday\" : 1.4658129805029452,\n  \"consumption\" : 6.027456183070403,\n  \"id\" : \"id\",\n  \"brand\" : \"brand\"\n} ]";

            var example = exampleJson != null
            ? JsonConvert.DeserializeObject<List<Car>>(exampleJson)
            : default(List<Car>);            //TODO: Change the data returned
            return new ObjectResult(example);
        }

        /// <summary>
        /// get all cars
        /// </summary>
        /// <param name="query"></param>
        /// <response code="200">got cars</response>
        [HttpGet]
        [Route("/car")]
        //[Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
        [ValidateModelState]
        [SwaggerOperation("ApiV1CarGet")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<CarInfo>), description: "got cars")]
        public virtual IActionResult ApiV1CarGet([FromQuery] string query)
        {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(List<CarInfo>));
            string exampleJson = null;
            exampleJson = "[ {\n  \"name\" : \"name\",\n  \"priceperday\" : 0.8008281904610115,\n  \"id\" : \"id\",\n  \"brand\" : \"brand\"\n}, {\n  \"name\" : \"name\",\n  \"priceperday\" : 0.8008281904610115,\n  \"id\" : \"id\",\n  \"brand\" : \"brand\"\n} ]";

            var example = exampleJson != null
            ? JsonConvert.DeserializeObject<List<CarInfo>>(exampleJson)
            : default(List<CarInfo>);            //TODO: Change the data returned
            return new ObjectResult(example);
        }

        /// <summary>
        /// get car by id
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">got car details</response>
        [HttpGet]
        [Route("/v1/car/{id}")]
        //[Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
        [ValidateModelState]
        [SwaggerOperation("ApiV1CarIdGet")]
        [SwaggerResponse(statusCode: 200, type: typeof(Car), description: "got car details")]
        public virtual IActionResult ApiV1CarIdGet([FromRoute][Required] string id)
        {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(Car));
            string exampleJson = null;
            exampleJson = "{\n  \"horsepower\" : 0,\n  \"name\" : \"name\",\n  \"priceperday\" : 1.4658129805029452,\n  \"consumption\" : 6.027456183070403,\n  \"id\" : \"id\",\n  \"brand\" : \"brand\"\n}";

            var example = exampleJson != null
            ? JsonConvert.DeserializeObject<Car>(exampleJson)
            : default(Car);            //TODO: Change the data returned
            return new ObjectResult(example);
        }
    }
}
