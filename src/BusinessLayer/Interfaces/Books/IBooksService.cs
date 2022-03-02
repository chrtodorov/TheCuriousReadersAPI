using BusinessLayer.Models;

namespace BusinessLayer.Interfaces.Books;

public interface IBooksService
{
    Task<Book?> Get(Guid bookId);
    Task<PagedList<Book>> GetBooks(BookParameters booksParameters);
    Task<Book> Create(Book book);
    Task<Book?> Update(Guid bookId, Book book);
    Task Delete(Guid bookId);
    Task<bool> Contains(Guid bookId);
    Task<bool> IsIsbnExisting(string isbn);
}