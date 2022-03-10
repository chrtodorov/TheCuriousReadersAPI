using BusinessLayer.Enumerations;
using BusinessLayer.Interfaces.Users;
using BusinessLayer.Models.Requests;
using BusinessLayer.Requests;
using DataAccess.Mappers;
using Microsoft.AspNetCore.Authorization;
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

                var cookieOptions = GetRefreshTokenOptions();

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
        public async Task<IActionResult> Register([FromBody] UserRequest user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await usersService.Register(user.ToUser());

                _logger.LogInformation("Registered user: {@email}", user.EmailAddress);
                return Ok(new {message = $"Registered user: {user.EmailAddress}"});
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(user.RoleName, ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken is null)
            {
                return BadRequest(new { message = "Token is required" });
            }

            try
            {
                var authenticatedUser = await usersService.RefreshToken(User);
                var cookieOptions = GetRefreshTokenOptions();

                this.Response.Cookies.Append("refreshToken", authenticatedUser.RefreshToken, cookieOptions);

                return Ok(authenticatedUser);
            }
            catch (ArgumentNullException)
            {
                return BadRequest(new { message = "Invalid refresh token was provided" });

            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message, tokenExpired = true });
            }
        }

        [HttpPut("[action]/{userId}")]
        [Authorize(Policy = Policies.RequireAdministratorOrLibrarianRole)]
        public async Task<IActionResult> Approve(Guid userId)
        {
            try
            {
                var user = await usersService.ApproveUser(userId, User);
                return Ok(user.ToUserResponse());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException)
            {
                return Forbid();
            }
        }

        [HttpPut("[action]/{userId}")]
        [Authorize(Policy = Policies.RequireAdministratorOrLibrarianRole)]
        public async Task<IActionResult> Reject(Guid userId)
        {
            try
            {
                await usersService.RejectUser(userId, User);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException)
            {
                return Forbid();
            }
        }

        [HttpGet("customers/get-pending")]
        [Authorize(Policy = Policies.RequireAdministratorOrLibrarianRole)]
        public async Task<IActionResult> GetPendingCustomers()
        {
            var pendingCustomers = await usersService.GetPendingCustomers();
            return Ok(pendingCustomers.Select(u => u.ToUserResponse()));
        }

        [HttpGet("get-pending")]
        [Authorize(Policy = Policies.RequireAdministratorRole)]
        public async Task<IActionResult> GetPendingUsers()
        {
            var pendingUsers = await usersService.GetPendingUsers();
            return Ok(pendingUsers.Select(u => u.ToUserResponse()));
        }

        private CookieOptions GetRefreshTokenOptions()
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
        }
        [AllowAnonymous]
        [HttpGet("count")]
        public async Task<IActionResult> GetCount()
        {
            var numberOfUsers = await usersService.GetCount();
            return Ok(numberOfUsers);
        }
    }
}
