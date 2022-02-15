using BusinessLayer.Enumerations;
using BusinessLayer.Interfaces.Authors;
using BusinessLayer.Models;
using BusinessLayer.Requests;
using DataAccess.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Policy = Policies.RequireLibrarianRole)]
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

        [HttpGet]
        public async Task<IActionResult> GetAuthors([FromQuery]AuthorParameters authorParameters)
        {

            var authors = await _authorsService.GetAuthors(authorParameters);
            
            if (authors is null)
            {
                return NotFound("No authors found");
            }

            _logger.LogInformation($"Returned {authors.Count} authors from the database");

            return Ok(authors);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AuthorsRequest authorRequest)
        {
            _logger.LogInformation("Create Author: " + authorRequest.ToString());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _authorsService.Create(authorRequest.ToAuthor()));
        }

        [HttpPut("{authorId}")]
        public async Task<IActionResult> Update(Guid authorId, [FromBody] AuthorsRequest authorsRequest)
        {
            _logger.LogInformation("Update Author: " + authorsRequest.ToString());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _authorsService.Contains(authorId))
                return NotFound("Author with such Id cannot be found and updated");

            return Ok(await _authorsService.Update(authorId, authorsRequest.ToAuthor()));

        }

        [HttpDelete("{authorId}")]
        public async Task<IActionResult> Delete(Guid authorId)
        {
            _logger.LogInformation("Delete Author with {@authorId}", authorId);

            if (!await _authorsService.Contains(authorId)) 
                return NotFound("Author with such Id does not exist!");

            await _authorsService.Delete(authorId);
            return Ok("Author deleted ID: " + authorId);
        }
    }
}