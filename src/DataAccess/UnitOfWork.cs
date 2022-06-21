using BusinessLayer.Interfaces;
using BusinessLayer.Interfaces.BookItems;
using BusinessLayer.Interfaces.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        public UnitOfWork(DataContext context, 
                          IBookItemsRepository bookItemsRepository, 
                          IBooksRepository booksRepository)
        {
            _context = context;
            _booksRepository = booksRepository;
            _bookItemsRepository = bookItemsRepository;
        }
        public IBooksRepository _booksRepository { get; private set; }

        public IBookItemsRepository _bookItemsRepository { get; private set; }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
