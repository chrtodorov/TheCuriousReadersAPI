using BusinessLayer.Interfaces.BookItems;
using BusinessLayer.Interfaces.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IUnitOfWork
    {
        IBooksRepository _booksRepository { get; }
        IBookItemsRepository _bookItemsRepository { get; }
        Task<int> SaveChanges();
    }
}
