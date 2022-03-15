using BusinessLayer.Enumerations;
using BusinessLayer.Interfaces.BookLoans;
using BusinessLayer.Models;
using BusinessLayer.Requests;
using DataAccess.Entities;
using DataAccess.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class BookLoansRepository : IBookLoansRepository
    {
        private readonly DataContext _dbContext;

        public BookLoansRepository(DataContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public PagedList<BookLoan> GetAll(PagingParameters pagingParameters)
        {
            var booksQuery = GetBookLoansQuery().Select(l => l.ToBookLoan());
            return PagedList<BookLoan>.ToPagedList(booksQuery, pagingParameters.PageNumber, pagingParameters.PageSize);
        }

        public PagedList<BookLoan> GetExpiring(PagingParameters pagingParameters)
        {
            var afterTwoWeeks = DateTime.UtcNow.AddDays(14);
            var booksQuery = GetBookLoansQuery()
                .Where(l => l.To <= afterTwoWeeks)
                .Select(l => l.ToBookLoan());
            return PagedList<BookLoan>.ToPagedList(booksQuery, pagingParameters.PageNumber, pagingParameters.PageSize);
        }

        public async Task<BookLoan> GetLoanById(Guid userId)
        {
            var userExists = await _dbContext.Users.AnyAsync(u => u.UserId == userId);
            if (!userExists)
            {
                throw new ArgumentException($"User with id: {userId} does not exist");
            }

            var loan = await GetBookLoansQuery().FirstOrDefaultAsync(l => l.Customer.User.UserId == userId);
            if (loan is null)
            {
                throw new ArgumentException($"User with id: {userId} has not loaned a book");
            }
            return loan.ToBookLoan();
        }

        public async Task<BookLoan> LoanBook(BookLoan bookLoan)
        {
            var customerExists = await _dbContext.Customers.AnyAsync(c => c.CustomerId == bookLoan.LoanedToId);
            if (!customerExists)
            {
                throw new ArgumentException($"Customer with id: {bookLoan.LoanedToId} does not exist");
            }

            var bookItem = await _dbContext.BookItems.FirstOrDefaultAsync(i => i.BookItemId == bookLoan.BookItemId);
            if (bookItem is null)
            {
                throw new ArgumentException($"Book copy with id: {bookLoan.BookItemId} does not exist");
            }
            if (bookItem.BookStatus == BookItemStatusEnumeration.Borrowed)
            {
                throw new ArgumentException($"Book copy with id: {bookLoan.BookItemId} has already been borrowed");
            }
            else if (bookItem.BookStatus == BookItemStatusEnumeration.Available)
            {
                throw new ArgumentException("A book copy must be reserved and approved by librarian");
            }
            else if (bookItem.BookStatus == BookItemStatusEnumeration.NotAvailable)
            {
                throw new ArgumentException($"Book copy with id: {bookLoan.BookItemId} is unavailable");
            }

            var createdEntity = await _dbContext.BookLoans.AddAsync(bookLoan.ToBookLoanEntity());
            bookItem.BookStatus = BookItemStatusEnumeration.Borrowed;
            createdEntity.Entity.BookItem = bookItem;
            var bookRequest = await _dbContext.BookRequests.FirstAsync(i => i.BookItemId == bookLoan.BookItemId);
            bookRequest.Status = BookRequestStatus.Approved;
            await _dbContext.SaveChangesAsync();

            var fullCreatedEntity = await GetBookLoansQuery().FirstAsync(l => l.BookLoanId == createdEntity.Entity.BookLoanId);
            return fullCreatedEntity.ToBookLoan();
        }

        public async Task<BookLoan> ProlongLoan(Guid bookLoanId, ProlongRequest prolongRequest)
        {
            var bookLoanEntity = await GetBookLoansQuery()
                .FirstOrDefaultAsync(l => l.BookLoanId == bookLoanId);

            if (bookLoanEntity is null)
            {
                throw new ArgumentException($"Book loan with id: {bookLoanId} does not exist");
            }

            if (bookLoanEntity.To >= prolongRequest.ExtendedTo)
            {
                throw new ArgumentException("Requested end time is before the actual loan end time");
            }

            bookLoanEntity.To = prolongRequest.ExtendedTo;
            bookLoanEntity.TimesExtended++;
            _dbContext.Update(bookLoanEntity);
            await _dbContext.SaveChangesAsync();

            return bookLoanEntity.ToBookLoan();
        }

        private IQueryable<BookLoanEntity> GetBookLoansQuery()
        {
            return _dbContext.BookLoans
                .Include(l => l.Customer)
                    .ThenInclude(c => c.User)
                .Include(l => l.BookItem)
                    .ThenInclude(i => i.Book)
                .AsNoTracking();
        }
    }
}