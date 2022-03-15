using BusinessLayer.Models;
using BusinessLayer.Requests;

namespace BusinessLayer.Interfaces.BookLoans
{
    public interface IBookLoansRepository
    {
        Task<BookLoan> LoanBook(BookLoan bookLoan);
        Task<BookLoan> GetLoanById(Guid userId);
        Task<BookLoan> ProlongLoan(Guid bookLoanId, ProlongRequest prolongRequest);
        PagedList<BookLoan> GetAll(PagingParameters pagingParameters);
        PagedList<BookLoan> GetExpiring(PagingParameters pagingParameters);
    }
}