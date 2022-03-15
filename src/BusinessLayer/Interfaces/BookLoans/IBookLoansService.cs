using BusinessLayer.Models;

namespace BusinessLayer.Interfaces.BookLoans
{
    public interface IBookLoansService
    {
        Task<BookLoan> LoanBook(BookLoan bookLoan);
        Task<BookLoan> GetLoanById(Guid userId);
        PagedList<BookLoan> GetAll(PagingParameters bookRequestParameters);
        PagedList<BookLoan> GetExpiring(PagingParameters pagingParameters);
    }
}