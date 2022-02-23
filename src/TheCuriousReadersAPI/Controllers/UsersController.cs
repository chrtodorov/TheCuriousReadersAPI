using BusinessLayer.Interfaces.Users;
using BusinessLayer.Models;
using BusinessLayer.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService usersService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUsersService usersService, ILogger<UsersController> logger)
        {
            this.usersService = usersService;
            this._logger = logger;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest authenticateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Trying to authenticate user: {@email}", authenticateRequest.Email);
            try
            {
                var authenticatedUser = await usersService.Authenticate(authenticateRequest.Email, authenticateRequest.Password);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddDays(7)
                };

                this.Response.Cookies.Append("refreshToken", authenticatedUser.RefreshToken, cookieOptions);
                return Ok(authenticatedUser);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("Credentials", ex.Message);
                return BadRequest(ModelState);
            }

        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var authenticatedUser = await usersService.Register(user);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddDays(7)
                };

                this.Response.Cookies.Append("refreshToken", authenticatedUser.RefreshToken, cookieOptions);

                _logger.LogInformation("Registered user: {@email}", user.EmailAddress);
                return Ok(authenticatedUser);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(user.RoleName, ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
