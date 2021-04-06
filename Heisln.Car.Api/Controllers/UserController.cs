using Heisln.Api.Attributes;
using Heisln.Api.Models;
using Heisln.Api.Security;
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
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserApiController : ControllerBase
    {
        /// <summary>
        /// register a user
        /// </summary>
        /// <param name="newUser">Mail needs to be unique</param>
        /// <response code="200">registered user</response>
        /// <response code="409">login invalid</response>
        [Route("/login")]
        [HttpPost]
        //[Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
        [ValidateModelState]
        [SwaggerOperation("ApiV1UserLoginPost")]
        [SwaggerResponse(statusCode: 200, type: typeof(AuthenticationResponse), description: "registered user")]
        [SwaggerResponse(statusCode: 409, type: typeof(ErrorObject), description: "login invalid")]
        public virtual IActionResult ApiV1UserLoginPost([FromQuery] User newUser)
        {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(AuthenticationResponse));

            //TODO: Uncomment the next line to return response 409 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(409, default(ErrorObject));
            string exampleJson = null;
            exampleJson = "{\n  \"token\" : \"token\"\n}";

            var example = exampleJson != null
            ? JsonConvert.DeserializeObject<AuthenticationResponse>(exampleJson)
            : default(AuthenticationResponse);            //TODO: Change the data returned
            return new ObjectResult(example);
        }

        /// <summary>
        /// register a user
        /// </summary>
        /// <param name="newUser">Mail needs to be unique</param>
        /// <response code="200">registered user</response>
        /// <response code="409">registration invalid</response>
        [Route("/registration")]
        [HttpPost]
        //[Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
        [ValidateModelState]
        [SwaggerOperation("ApiV1UserRegistrationPost")]
        [SwaggerResponse(statusCode: 200, type: typeof(AuthenticationResponse), description: "registered user")]
        [SwaggerResponse(statusCode: 409, type: typeof(ErrorObject), description: "registration invalid")]
        public virtual IActionResult ApiV1UserRegistrationPost([FromQuery] User newUser)
        {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(AuthenticationResponse));

            //TODO: Uncomment the next line to return response 409 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(409, default(ErrorObject));
            string exampleJson = null;
            exampleJson = "{\n  \"token\" : \"token\"\n}";

            var example = exampleJson != null
            ? JsonConvert.DeserializeObject<AuthenticationResponse>(exampleJson)
            : default(AuthenticationResponse);            //TODO: Change the data returned
            return new ObjectResult(example);
        }
    }
}
