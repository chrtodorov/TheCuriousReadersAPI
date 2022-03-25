using BusinessLayer.Enumerations;
using BusinessLayer.Interfaces.Books;
using BusinessLayer.Models;
using BusinessLayer.Requests;
using DataAccess.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Policy = Policies.RequireAdministratorOrLibrarianRole)]
[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBooksService _booksService;
    private readonly ILogger<BooksController> _logger;

    public BooksController(IBooksService booksService, ILogger<BooksController> logger)
    {
        _booksService = booksService;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpGet("{bookId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Book))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid bookId)
    {
        _logger.LogInformation("Get Book {@BookId}", bookId);
        return Ok(await _booksService.Get(bookId));
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<Book>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBooks([FromQuery] BookParameters booksParameters)
    {
        _logger.LogInformation("Returned all books from database");
        return Ok(await _booksService.GetBooks(booksParameters));
    }

    [AllowAnonymous]
    [HttpGet("latestbooks")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Book>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLatest()
    {
        _logger.LogInformation("Returned all books from database");
        return Ok(await _booksService.GetLatest());
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Book))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([FromBody] BookRequest bookRequest)
    {
        _logger.LogInformation("Create Book: " + bookRequest);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(await _booksService.Create(bookRequest.ToBook()));
    }

    [HttpPut("{bookId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Book))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Update(Guid bookId, [FromBody] BookRequest bookRequest)
    {
        _logger.LogInformation("Update Book: " + bookRequest);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(await _booksService.Update(bookId, bookRequest.ToBook()));
    }

    [HttpDelete("{bookId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(Guid bookId)
    {
        _logger.LogInformation("Delete Book with {@bookId}", bookId);
        await _booksService.Delete(bookId);
        return Ok();
    }

    [AllowAnonymous]
    [HttpGet("count")]
    public async Task<IActionResult> GetNumber()
    {
        _logger.LogInformation("Get books count");
        return Ok(await _booksService.GetNumber());
    }

    [HttpPut("{bookId}/status")]
    public async Task<IActionResult> MakeUnavailable(Guid bookId)
    {
        await _booksService.MakeUnavailable(bookId);
        return Ok();
    }
}