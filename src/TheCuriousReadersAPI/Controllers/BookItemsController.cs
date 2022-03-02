using BusinessLayer.Enumerations;
using BusinessLayer.Interfaces.BookItems;
using BusinessLayer.Requests;
using DataAccess.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Policy = Policies.RequireAdministratorOrLibrarianRole)]
    [Route("api/[controller]")]
    [ApiController]
    public class BookItemsController : ControllerBase
    {
        private readonly IBookItemsService _bookItemsService;
        private readonly ILogger<BookItemsController> _logger;

        public BookItemsController(IBookItemsService bookItemsService, ILogger<BookItemsController> logger)
        {
            this._bookItemsService = bookItemsService;
            this._logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("{bookItemId}")]
        public async Task<IActionResult> Get(Guid bookItemId)
        {
            _logger.LogInformation("Get Book Items {@BookItemId}", bookItemId);

            var result = await _bookItemsService.Get(bookItemId);

            if (result is null)
            {
                return NotFound("Book item with such Id is not found!");
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]BookItemsRequest bookItemsRequest)
        {
            _logger.LogInformation("Create Book Items" + bookItemsRequest.ToString());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(await _bookItemsService.Create(bookItemsRequest.ToBookItem()));
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{bookItemId}")]
        public async Task<IActionResult> Update(Guid bookItemId, [FromBody] BookItemsRequest bookItemsRequest)
        {
            _logger.LogInformation("Update Book Item" + bookItemsRequest.ToString());
        
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(await _bookItemsService.Update(bookItemId, bookItemsRequest.ToBookItem()));
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{bookItemId}/{bookItemStatus}")]
        public async Task<IActionResult> UpdateBookItemStatus(Guid bookItemId, BookItemStatusEnumeration bookItemStatus)
        {
            _logger.LogInformation("Update Book Item Status" + bookItemStatus.ToString());
        
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(await _bookItemsService.UpdateBookItemStatus(bookItemId, bookItemStatus));
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }                                    
        }

        [HttpDelete("{bookItemId}")]
        public async Task<IActionResult> Delete(Guid bookItemId)
        {
            _logger.LogInformation("Delete Book Item with {@BookItemId}", bookItemId);

            try
            {
                await _bookItemsService.Delete(bookItemId);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}