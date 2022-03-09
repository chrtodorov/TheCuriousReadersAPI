using BusinessLayer.Enumerations;
using BusinessLayer.Interfaces.Books;
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
    public class BooksController : ControllerBase
    {
        private readonly IBooksService _booksService;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBooksService booksService, ILogger<BooksController> logger)
        {
            this._booksService = booksService;
            this._logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("{bookId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Book))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid bookId)
        {
            _logger.LogInformation("Get Book {@BookId}", bookId);

            var result = await _booksService.Get(bookId);

            if (result is null)
                return NotFound("Book with such Id is not found!");
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<Book>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBooks([FromQuery] BookParameters booksParameters)
        {
            _logger.LogInformation($"Returned all books from database");
            return Ok(await _booksService.GetBooks(booksParameters));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Book))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] BookRequest bookRequest)
        {
            _logger.LogInformation("Create Book: " + bookRequest.ToString());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                return Ok(await _booksService.Create(bookRequest.ToBook()));
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{bookId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Book))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update(Guid bookId, [FromBody] BookRequest bookRequest)
        {
            _logger.LogInformation("Update Book: " + bookRequest.ToString());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                return Ok(await _booksService.Update(bookId, bookRequest.ToBook()));
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{bookId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete(Guid bookId)
        {
            _logger.LogInformation("Delete Book with {@bookId}", bookId);

            try
            {
                await _booksService.Delete(bookId);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
