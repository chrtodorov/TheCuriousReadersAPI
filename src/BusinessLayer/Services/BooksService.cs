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

    public async Task<PagedList<Book>> GetBooks(BookParameters booksParameters) => await _repository.GetBooks(booksParameters);

    public async Task<Book> Create(Book book)
    {
        if (await _repository.IsIsbnExisting(book.Isbn))
        {
            throw new ArgumentException($"Book with this ISBN: {book.Isbn} already exists!");
        }
        return await _repository.Create(book);
    }

    public async Task<Book?> Update(Guid bookId, Book book)
    {
        if (!await _repository.Contains(bookId))
        {
            throw new ArgumentException("Book cannot be found!");
        }

        var bookFromDb = await _repository.Get(bookId);

        if (await _repository.IsIsbnExisting(book.Isbn) && bookFromDb!.Isbn != book.Isbn)
        {
            throw new ArgumentException($"Book with this ISBN: {book.Isbn} already exists!");
        }
        return await _repository.Update(bookId, book);
    }

    public async Task Delete(Guid bookId)
    {
        if (!await _repository.Contains(bookId))
        {
            throw new ArgumentException("Book cannot be found!");
        }
        await _repository.Delete(bookId);
    }
    public async Task<bool> Contains(Guid bookId) => await _repository.Contains(bookId);
    public async Task<bool> IsIsbnExisting(string isbn) => await _repository.IsIsbnExisting(isbn);
}