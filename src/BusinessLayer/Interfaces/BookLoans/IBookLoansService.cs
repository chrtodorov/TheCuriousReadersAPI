﻿using BusinessLayer.Models;
using BusinessLayer.Requests;

namespace BusinessLayer.Interfaces.BookLoans
{
    public interface IBookLoansService
    {
        Task<BookLoan> LoanBook(BookLoan bookLoan);
        Task<PagedList<BookLoan>> GetLoansById(Guid userId, PagingParameters pagingParameters);
        Task<BookLoan> ProlongLoan(Guid bookLoanId, ProlongRequest prolongRequest);
        PagedList<BookLoan> GetAll(PagingParameters bookRequestParameters);
        PagedList<BookLoan> GetExpiring(PagingParameters pagingParameters);
    }
}