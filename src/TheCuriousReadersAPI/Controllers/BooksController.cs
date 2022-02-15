using System.Text.Json;
using BusinessLayer.Enumerations;
using BusinessLayer.Interfaces.Books;
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
        public async Task<IActionResult> GetBooks([FromQuery] BookParameters booksParameters)
        {
            var books = await _booksService.GetBooks(booksParameters);

            var metadata = new
            {
                books.TotalCount,
                books.PageSize,
                books.CurrentPage,
                books.TotalPages,
                books.HasNext,
                books.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

            _logger.LogInformation($"Returned {books.TotalCount} books from database");

            return Ok(books);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookRequest bookRequest)
        {
            _logger.LogInformation("Create Book: " + bookRequest.ToString());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _booksService.Create(bookRequest.ToBook()));
        }

        [HttpPut("{bookId}")]
        public async Task<IActionResult> Update(Guid bookId, [FromBody] BookRequest bookRequest)
        {
            _logger.LogInformation("Update Book: " + bookRequest.ToString());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _booksService.Contains(bookId))
                return NotFound("Book with such Id cannot be found and updated");

            return Ok(await _booksService.Update(bookId, bookRequest.ToBook()));
        }

        [HttpDelete("{bookId}")]
        public async Task<IActionResult> Delete(Guid bookId)
        {
            _logger.LogInformation("Delete Book with {@bookId}", bookId);

            if (!await _booksService.Contains(bookId))
                return NotFound("Book with such Id does not exist!");

            await _booksService.Delete(bookId);
            return Ok("Book deleted ID: " + bookId);
        }
    }
}
