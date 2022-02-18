using BusinessLayer.Enumerations;
using BusinessLayer.Interfaces.Users;
using BusinessLayer.Models.Requests;
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
            var token = usersService.Authenticate(authenticateRequest);

            if (token is null)
            {
                _logger.LogInformation("Unable to authenticate user: {@Username}", authenticateRequest.Username);
                return BadRequest(new {Message = "Invalid credentials have been provided"});
            }

            _logger.LogInformation("Authenticated user: {@Username}", authenticateRequest.Username);
            return Ok(new {token});
        }
    }
}
