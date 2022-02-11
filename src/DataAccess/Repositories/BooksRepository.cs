using BusinessLayer.Interfaces.Books;
using BusinessLayer.Models;
using DataAccess.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccess.Repositories;

public class BooksRepository : IBooksRepository
{
    private readonly DataContext _dataContext;
    private readonly ILogger<BooksRepository> _logger;

    public BooksRepository(DataContext dbContext, ILogger<BooksRepository> logger)
    {
        this._dataContext = dbContext;
        this._logger = logger;
    }

    public async Task<Book?> Get(Guid bookId)
    {
        _logger.LogInformation("Get Book with {@BookId}", bookId);

        var bookEntity = await _dataContext.Books
            .Include(b => b.Publisher)
            .Include(b => b.Authors)
            .FirstOrDefaultAsync(b => b.BookId == bookId);

        return bookEntity?.ToBook();
    }

    public async Task<Book> Create(Book book)
    {
        var bookEntity = book.ToBookEntity();

        foreach (var author in bookEntity.Authors!)
        {
            _dataContext.Authors.Attach(author);
        }
            
        await _dataContext.Books.AddAsync(bookEntity);
        await _dataContext.SaveChangesAsync();

        _logger.LogInformation("Create Book with {@BookId}", bookEntity.BookId);

        return bookEntity.ToBook();
    }

    public async Task<Book?> Update(Guid bookId, Book book)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(Guid bookId)
    {
        var bookEntity = await _dataContext.Books.FindAsync(bookId);

        if (bookEntity is not null)
        {
            _dataContext.Books.Remove(bookEntity);

            await _dataContext.SaveChangesAsync();

            _logger.LogInformation("Deleting Book with {@BookId}", bookId);
        }

        _logger.LogInformation("There is no such Book with {@BookId}", bookId);
    }

    public async Task<bool> Contains(Guid bookId)
    {
        return await _dataContext.Books.AnyAsync(b => b.BookId == bookId);
    }
}