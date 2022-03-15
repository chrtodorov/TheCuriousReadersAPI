using BusinessLayer.Models;

namespace BusinessLayer.Interfaces.BookLoans
{
    public interface IBookLoansRepository
    {
        Task<BookLoan> LoanBook(BookLoan bookLoan);
        Task<BookLoan> GetLoanById(Guid userId);
        PagedList<BookLoan> GetAll(PagingParameters pagingParameters);
        PagedList<BookLoan> GetExpiring(PagingParameters pagingParameters);
    }
}