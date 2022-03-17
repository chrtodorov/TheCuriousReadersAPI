using BusinessLayer.Interfaces.BookLoans;
using BusinessLayer.Models;
using BusinessLayer.Requests;

namespace BusinessLayer.Services
{
    public class BookLoansService : IBookLoansService
    {
        private readonly IBookLoansRepository bookLoansRepository;

        public BookLoansService(IBookLoansRepository bookLoansRepository)
        {
            this.bookLoansRepository = bookLoansRepository;
        }

        public PagedList<BookLoan> GetAll(PagingParameters pagingParameters) 
            => bookLoansRepository.GetAll(pagingParameters);

        public PagedList<BookLoan> GetExpiring(PagingParameters pagingParameters)
            => bookLoansRepository.GetExpiring(pagingParameters);

        public async Task<PagedList<BookLoan>> GetLoansById(Guid userId, PagingParameters pagingParameters)
            => await bookLoansRepository.GetLoansById(userId, pagingParameters);

        public async Task<BookLoan> LoanBook(BookLoan bookLoan)
            => await bookLoansRepository.LoanBook(bookLoan);

        public async Task<BookLoan> ProlongLoan(Guid bookLoanId, ProlongRequest prolongRequest)
            => await bookLoansRepository.ProlongLoan(bookLoanId, prolongRequest);
    }
}