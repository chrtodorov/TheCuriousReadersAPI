using BusinessLayer.Interfaces.BookItems;
using BusinessLayer.Interfaces.Books;
using BusinessLayer.Models;
using BusinessLayer.Responses;

namespace BusinessLayer.Services;

public class BooksService : IBooksService
{
    private readonly IBooksRepository _repository;
    private readonly IBookItemsRepository _bookItemsRepository;

    public BooksService(IBooksRepository repository, IBookItemsRepository bookItemsRepository)
    {
        this._repository = repository;
        this._bookItemsRepository = bookItemsRepository;
    }

    public async Task<BookDetailsResponse?> Get(Guid bookId) => await _repository.Get(bookId);

    public async Task<PagedList<Book>> GetBooks(BookParameters booksParameters) => await _repository.GetBooks(booksParameters);

    public async Task<Book> Create(Book book)
    {
        if (await _repository.IsIsbnExisting(book.Isbn))
        {
            throw new ArgumentException($"Book with this ISBN: {book.Isbn} already exists!");
        }

        foreach (var bookItem in book.BookItems!)
        {
            if (await _bookItemsRepository.IsBarcodeExisting(bookItem.Barcode))
            {
                throw new ArgumentException($"Book Copy with Barcode: {bookItem.Barcode} already exists!");
            }
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