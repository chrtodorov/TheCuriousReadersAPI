using BusinessLayer.Enumerations;
using BusinessLayer.Interfaces.BookItems;
using BusinessLayer.Requests;
using DataAccess.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Policy = Policies.RequireAdministratorOrLibrarianRole)]
[Route("api/[controller]")]
[ApiController]
public class BookItemsController : ControllerBase
{
    private readonly IBookItemsService _bookItemsService;
    private readonly ILogger<BookItemsController> _logger;

    public BookItemsController(IBookItemsService bookItemsService, ILogger<BookItemsController> logger)
    {
        _bookItemsService = bookItemsService;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpGet("{bookItemId}")]
    public async Task<IActionResult> Get(Guid bookItemId)
    {
        _logger.LogInformation("Get Book Items {@BookItemId}", bookItemId);
        return Ok(await _bookItemsService.Get(bookItemId));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BookItemsRequest bookItemsRequest)
    {
        _logger.LogInformation("Create Book Items" + bookItemsRequest);

        if (!ModelState.IsValid) return BadRequest(ModelState);

        return Ok(await _bookItemsService.Create(bookItemsRequest.ToBookItem()));
    }

    [HttpPut("{bookItemId}")]
    public async Task<IActionResult> Update(Guid bookItemId, [FromBody] BookItemsRequest bookItemsRequest)
    {
        _logger.LogInformation("Update Book Item" + bookItemsRequest);

        if (!ModelState.IsValid) return BadRequest(ModelState);

        return Ok(await _bookItemsService.Update(bookItemId, bookItemsRequest.ToBookItem()));
    }

    [HttpPut("{bookItemId}/{bookItemStatus}")]
    public async Task<IActionResult> UpdateBookItemStatus(Guid bookItemId, BookItemStatusEnumeration bookItemStatus)
    {
        _logger.LogInformation("Update Book Item Status" + bookItemStatus);

        if (!ModelState.IsValid) return BadRequest(ModelState);

        return Ok(await _bookItemsService.UpdateBookItemStatus(bookItemId, bookItemStatus));
    }

    [HttpDelete("{bookItemId}")]
    public async Task<IActionResult> Delete(Guid bookItemId)
    {
        _logger.LogInformation("Delete Book Item with {@BookItemId}", bookItemId);

        await _bookItemsService.Delete(bookItemId);
        return Ok();
    }
}