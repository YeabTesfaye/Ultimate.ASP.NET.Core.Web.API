using CompanyEmployees.Presentation.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers
{
    /// <summary>
    /// Handles operations related to token management such as refreshing tokens.
    /// </summary>
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IServiceManager _services;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenController"/> class.
        /// </summary>
        /// <param name="services">The service manager for handling token-related operations.</param>
        public TokenController(IServiceManager services) => _services = services;

        /// <summary>
        /// Refreshes an existing JWT token.
        /// </summary>
        /// <param name="tokenDto">The token data transfer object containing the refresh token.</param>
        /// <returns>A new JWT token if the refresh is successful.</returns>
        /// <response code="200">If the token refresh is successful and a new token is returned.</response>
        /// <response code="400">If the refresh token is invalid or expired.</response>
        [HttpPost("refresh")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
        {
            var tokenDtoToReturn = await _services.AuthenticationService.RefreshToken(tokenDto);
            return Ok(tokenDtoToReturn);
        }
    }
}
