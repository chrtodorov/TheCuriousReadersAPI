using BusinessLayer.Enumerations;
using BusinessLayer.Interfaces.BookLoans;
using BusinessLayer.Models;
using BusinessLayer.Requests;
using DataAccess.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookLoansController : ControllerBase
    {
        private readonly IBookLoansService bookLoansService;

        public BookLoansController(IBookLoansService bookLoansService)
        {
            this.bookLoansService = bookLoansService;
        }

        [HttpGet("User/{userId}")]
        [Authorize(Policy = Policies.RequireLibrarianRole)]
        public async Task<IActionResult> GetLoansByUserId(Guid userId, [FromQuery] PagingParameters pagingParameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var pagedList = await bookLoansService.GetLoansById(userId, pagingParameters);
                var responseData = pagedList.Data.Select(l => l.ToBookLoanResponse());
                return Ok(responseData.ToPagedList(pagedList.TotalCount, pagedList.CurrentPage, pagedList.PageSize));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("User", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpGet("Mine")]
        [Authorize(Policy = Policies.RequireCustomerRole)]
        public async Task<IActionResult> GetCurrentUserLoans([FromQuery] PagingParameters pagingParameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value!);
                var pagedList = await bookLoansService.GetLoansById(userId, pagingParameters);
                var responseData = pagedList.Data.Select(l => l.ToBookLoanResponse());
                return Ok(responseData.ToPagedList(pagedList.TotalCount, pagedList.CurrentPage, pagedList.PageSize));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("User", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Authorize(Policy = Policies.RequireLibrarianRole)]
        public async Task<IActionResult> Create(BookLoanRequest bookLoanRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userSpecificId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "RoleSpecificId")?.Value!);
                var createdLoan = await bookLoansService.LoanBook(bookLoanRequest.ToBookLoan(userSpecificId));
                return Ok(createdLoan.ToBookLoanResponse());
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("RequestModel", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Authorize(Policy = Policies.RequireLibrarianRole)]
        public IActionResult GetAll([FromQuery] PagingParameters pagingParameters)
        {
            var pagedList = bookLoansService.GetAll(pagingParameters);
            var responseData = pagedList.Data.Select(l => l.ToBookLoanResponse());
            return Ok(responseData.ToPagedList(pagedList.TotalCount, pagedList.CurrentPage, pagedList.PageSize));
        }

        [HttpGet("expiring")]
        [Authorize(Policy = Policies.RequireLibrarianRole)]
        public IActionResult GetExpiring([FromQuery] PagingParameters pagingParameters)
        {
            var pagedList = bookLoansService.GetExpiring(pagingParameters);
            var responseData = pagedList.Data.Select(l => l.ToBookLoanResponse());
            return Ok(responseData.ToPagedList(pagedList.TotalCount, pagedList.CurrentPage, pagedList.PageSize));
        }

        [HttpPut("{bookLoanId}")]
        [Authorize(Policy = Policies.RequireLibrarianRole)]
        public async Task<IActionResult> ProlongLoan(Guid bookLoanId, [FromBody] ProlongRequest prolongRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var bookloan = await bookLoansService.ProlongLoan(bookLoanId, prolongRequest);
                return Ok(bookloan.ToBookLoanResponse());
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("Prolong", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPut("Complete/{bookLoanId}")]
        [Authorize(Policy = Policies.RequireLibrarianRole)]
        public async Task<IActionResult> CompleteLoan(Guid bookLoanId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await bookLoansService.CompleteLoan(bookLoanId);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("BookLoan", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
