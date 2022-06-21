using BusinessLayer.Helpers;
using BusinessLayer.Interfaces;
using BusinessLayer.Interfaces.Books;
using BusinessLayer.Models;
using BusinessLayer.Responses;

namespace BusinessLayer.Services;

public class BooksService : IBooksService
{
    private readonly IBlobService _blobService;
    private readonly IUnitOfWork _unitOfWork;
    public BooksService(IUnitOfWork unitOfWork, IBlobService blobService)
    {
        _unitOfWork = unitOfWork;
        _blobService = blobService;
    }

    public async Task<BookDetailsResponse?> Get(Guid bookId)
    {
        var book = await _unitOfWork._booksRepository.Get(bookId);
        if (book is null)
            throw new KeyNotFoundException("Book not found!");

        return book;
    }

    public async Task<PagedList<Book>> GetBooks(BookParameters booksParameters)
    {
        return await _unitOfWork._booksRepository.GetBooks(booksParameters);
    }

    public async Task<List<Book>> GetLatest()
    {
        return await _unitOfWork._booksRepository.GetLatest();
    }

    public async Task<int> GetNumber()
    {
        return await _unitOfWork._booksRepository.GetNumber();
    }

    public async Task<Book> Create(Book book)
    {
        if (await _unitOfWork._booksRepository.IsIsbnExisting(book.Isbn))
            throw new AppException($"Book with this ISBN: {book.Isbn} already exists!");

        foreach (var bookItem in book.BookItems!)
            if (await _unitOfWork._bookItemsRepository.IsBarcodeExisting(bookItem.Barcode))
                throw new AppException($"Book Copy with Barcode: {bookItem.Barcode} already exists!");

        await _unitOfWork.SaveChanges(); // trqbva li da se mahnat SaveChanges ot repoto i trqbva li da se premesti Create-a
        return await _unitOfWork._booksRepository.Create(book);
    }

    public async Task<Book?> Update(Guid bookId, Book book)
    {
        if (!await _unitOfWork._booksRepository.Contains(bookId)) throw new KeyNotFoundException("Book cannot be found!");

        var bookFromDb = await _unitOfWork._booksRepository.Get(bookId);

        if (await _unitOfWork._booksRepository.IsIsbnExisting(book.Isbn) && bookFromDb!.Isbn != book.Isbn)
            throw new AppException($"Cannot update because this ISBN: {book.Isbn} already exists!");

        await _unitOfWork.SaveChanges();
        return await _unitOfWork._booksRepository.Update(bookId, book);
    }

    public async Task Delete(Guid bookId)
    {
        var book = await Get(bookId);
        if (book is null)
            throw new KeyNotFoundException("Book cannot be found!");
        if (await _unitOfWork._booksRepository.HasLoanedItems(bookId))
            throw new AppException($"There are active book loans for book with id: {bookId}");
        var blobName = book.CoverUrl.Split('/').Last();
        await _unitOfWork._booksRepository.Delete(bookId);
        await _blobService.DeleteAsync(blobName);
        //same 
    }

    public async Task MakeUnavailable(Guid bookId)
    {
        var book = await Get(bookId);
        if (book is null)
            throw new KeyNotFoundException("Book cannot be found!");
        await _unitOfWork._booksRepository.MakeUnavailable(bookId);
        //same
    }

    public async Task<bool> Contains(Guid bookId) => await _unitOfWork._booksRepository.Contains(bookId);
    public async Task<bool> IsIsbnExisting(string isbn) => await _unitOfWork._booksRepository.IsIsbnExisting(isbn);

    public PagedList<Book> GetReadBooks(Guid userId, PagingParameters pagingParameters)
        => _unitOfWork._booksRepository.GetReadBooks(userId, pagingParameters);
}