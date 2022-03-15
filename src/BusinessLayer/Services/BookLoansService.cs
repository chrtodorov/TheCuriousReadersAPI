﻿using BusinessLayer.Interfaces.BookLoans;
using BusinessLayer.Models;

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

        public async Task<BookLoan> GetLoanById(Guid userId)
            => await bookLoansRepository.GetLoanById(userId);

        public async Task<BookLoan> LoanBook(BookLoan bookLoan)
            => await bookLoansRepository.LoanBook(bookLoan);
    }
}