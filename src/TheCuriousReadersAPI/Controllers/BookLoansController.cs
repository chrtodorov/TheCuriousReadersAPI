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
    [Authorize(Policy = Policies.RequireLibrarianRole)]
    public class BookLoansController : ControllerBase
    {
        private readonly IBookLoansService bookLoansService;

        public BookLoansController(IBookLoansService bookLoansService)
        {
            this.bookLoansService = bookLoansService;
        }

        [HttpGet("User/{userId}")]
        public async Task<IActionResult> GetLoansByUserId(Guid userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var bookLoan = await bookLoansService.GetLoanById(userId);
                return Ok(bookLoan.ToBookLoanResponse());
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("User", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
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
        public IActionResult GetAll([FromQuery] PagingParameters pagingParameters)
        {
            var pagedList = bookLoansService.GetAll(pagingParameters);
            var responseData = pagedList.Data.Select(l => l.ToBookLoanResponse());
            return Ok(responseData.ToPagedList(pagedList.TotalCount, pagedList.CurrentPage, pagedList.PageSize));
        }  

        [HttpGet("expiring")]
        public IActionResult GetExpiring([FromQuery] PagingParameters pagingParameters)
        {
            var pagedList = bookLoansService.GetExpiring(pagingParameters);
            var responseData = pagedList.Data.Select(l => l.ToBookLoanResponse());
            return Ok(responseData.ToPagedList(pagedList.TotalCount, pagedList.CurrentPage, pagedList.PageSize));
        }
    }
}
