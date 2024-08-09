using CompanyEmployees.Presentation.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers
{
    /// <summary>
    /// Handles authentication-related operations such as user registration and login.
    /// </summary>
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceManager _services;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationController"/> class.
        /// </summary>
        /// <param name="service">The service manager for handling authentication-related operations.</param>
        public AuthenticationController(IServiceManager service) => _services = service;

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="userForRegistration">The user registration data.</param>
        /// <returns>A status code indicating the result of the registration process.</returns>
        /// <response code="201">If the registration is successful.</response>
        /// <response code="400">If the registration fails due to validation errors.</response>
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            var result = await _services.AuthenticationService.RegisterUser(userForRegistration);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return StatusCode(201);
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token if successful.
        /// </summary>
        /// <param name="user">The user authentication data.</param>
        /// <returns>A JWT token if the authentication is successful.</returns>
        /// <response code="200">If the authentication is successful and the token is created.</response>
        /// <response code="401">If the authentication fails due to invalid credentials.</response>
        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            if (!await _services.AuthenticationService.ValidateUser(user))
                return Unauthorized();
            var tokenDto = await _services.AuthenticationService.CreateToken(populateExp: true);
            return Ok(tokenDto);
        }
    }
}
