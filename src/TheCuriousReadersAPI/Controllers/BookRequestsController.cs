using BusinessLayer.Enumerations;
using BusinessLayer.Interfaces.BookRequests;
using BusinessLayer.Models;
using BusinessLayer.Requests;
using DataAccess.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookRequestsController : ControllerBase
    {
        private readonly IBookRequestsService bookRequestsService;

        public BookRequestsController(IBookRequestsService bookRequestsService)
        {
            this.bookRequestsService = bookRequestsService;
        }

        [Authorize(Policy = Policies.RequireLibrarianRole)]
        [HttpGet]
        public IActionResult GetAll([FromQuery] BookRequestParameters bookRequestParameters)
        {
            var pagedList = bookRequestsService.GetAllRequests(bookRequestParameters);
            var responseData = pagedList.Data.Select(m => m.ToBookRequestResponse());
            return Ok(responseData.ToPagedList(pagedList.TotalCount, pagedList.CurrentPage, pagedList.PageSize));
        }

        [Authorize(Policy = Policies.RequireCustomerRole)]
        [HttpGet("Mine")]
        public async Task<IActionResult> Get([FromQuery] BookRequestParameters bookRequestParameters)
        {
            try
            {
                var userSpecificId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "RoleSpecificId")?.Value!);
                var pagedList = await bookRequestsService.GetUserRequests(userSpecificId, bookRequestParameters);
                var responseData = pagedList.Data.Select(m => m.ToBookRequestResponse());
                return Ok(responseData.ToPagedList(pagedList.TotalCount, pagedList.CurrentPage, pagedList.PageSize));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("Customer", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [Authorize(Policy = Policies.RequireCustomerRole)]
        [HttpPost]
        public async Task<IActionResult> MakeRequest([FromBody] BookRequestRequest bookRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userSpecificId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "RoleSpecificId")?.Value!);
                var createdRequest = await bookRequestsService.MakeRequest(bookRequest.ToBookRequest(userSpecificId));
                return Ok(createdRequest.ToBookRequestResponse());
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("BookItem", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
