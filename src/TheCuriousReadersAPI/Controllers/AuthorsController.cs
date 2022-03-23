using BusinessLayer.Enumerations;
using BusinessLayer.Interfaces.Authors;
using BusinessLayer.Models;
using BusinessLayer.Requests;
using DataAccess.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Policy = Policies.RequireAdministratorOrLibrarianRole)]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorsService _authorsService;
        private readonly ILogger<AuthorsController> _logger;

        public AuthorsController(IAuthorsService authorsService, ILogger<AuthorsController> logger)
        {
            this._authorsService = authorsService;
            this._logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("{authorId}")]
        public async Task<IActionResult> Get(Guid authorId)
        {
            _logger.LogInformation("Get Author {@AuthorId}", authorId);

            var result = await _authorsService.Get(authorId);

            if (result is null)
                return NotFound("Author with such Id is not found!");
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAuthors([FromQuery]AuthorParameters authorParameters)
        {
            try
            {
                _logger.LogInformation($"Returned all authors from the database");
                return Ok(await _authorsService.GetAuthors(authorParameters));
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AuthorsRequest authorRequest)
        {
            _logger.LogInformation("Create Author: " + authorRequest.ToString());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                return Ok(await _authorsService.Create(authorRequest.ToAuthor()));
            }

            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{authorId}")]
        public async Task<IActionResult> Update(Guid authorId, [FromBody] AuthorsRequest authorRequest)
        {
            _logger.LogInformation("Update Author: " + authorRequest.ToString());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                return Ok(await _authorsService.Update(authorId, authorRequest.ToAuthor()));
            }
            catch (ArgumentNullException e)
            {
                return NotFound(e.Message);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{authorId}")]
        public async Task<IActionResult> Delete(Guid authorId)
        {
            _logger.LogInformation("Delete Author with {@authorId}", authorId);

            try
            {
                await _authorsService.Delete(authorId);
                return Ok();
            }
            catch (ArgumentNullException e)
            {
                return NotFound(e.Message);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}