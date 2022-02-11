using BusinessLayer.Interfaces.Books;
using BusinessLayer.Models;

namespace BusinessLayer.Services;

public class BooksService : IBooksService
{
    private readonly IBooksRepository _repository;

    public BooksService(IBooksRepository repository)
    {
        this._repository = repository;
    }

    public async Task<Book?> Get(Guid bookId) => await _repository.Get(bookId);

    public async Task<Book> Create(Book book) => await _repository.Create(book);

    public async Task<Book?> Update(Guid bookId, Book book) => await _repository.Update(bookId, book);

    public async Task Delete(Guid bookId) => await _repository.Delete(bookId);

    public async Task<bool> Contains(Guid bookId) => await _repository.Contains(bookId);
}