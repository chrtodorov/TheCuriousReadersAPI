using System.Linq;
using BusinessLayer.Interfaces.Books;
using BusinessLayer.Models;
using DataAccess.Entities;
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

    public async Task<PagedList<Book>> GetBooks(BookParameters bookParameters)
    {
        var query = _dataContext.Books.AsQueryable();

        if (!string.IsNullOrEmpty(bookParameters.Title))
        {
            query = query.Where(b => b.Title == bookParameters.Title);
        }

        if (!string.IsNullOrEmpty(bookParameters.Author))
        {
            var author = await _dataContext.Authors.FirstOrDefaultAsync(a => a.Name == bookParameters.Author);
            query = query.Where(b => b.Authors!.Contains(author!));
        }

        if (!string.IsNullOrEmpty(bookParameters.Publisher))
        {
            query = query.Where(b => b.Publisher!.Name == bookParameters.Publisher);
        }

        if (!string.IsNullOrEmpty(bookParameters.DescriptionKeyword))
        {
            query = query.Where(b => b.Description.Contains(bookParameters.DescriptionKeyword));
        }

        if (!string.IsNullOrEmpty(bookParameters.Genre))
        {
            query = query.Where(b => b.Genre == bookParameters.Genre);
        }

        await query
            .Include(b => b.Authors)
            .OrderBy(b => b.Title)
            .ToListAsync();

        _logger.LogInformation("Get all books");

        return PagedList<Book>.ToPagedList(query.Select(b => b.ToBook()),
            bookParameters.PageNumber,
            bookParameters.PageSize);
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
        var updatedBook = book.ToBookEntity();

        var bookToUpdate = await _dataContext.Books
            .Include(b => b.Authors)
            .FirstOrDefaultAsync(b => b.BookId == bookId);

        if (bookToUpdate is null) return null;

        bookToUpdate.Isbn = updatedBook.Isbn;
        bookToUpdate.Title = bookToUpdate.Title;
        bookToUpdate.Genre = updatedBook.Genre;
        bookToUpdate.PublisherId = updatedBook.PublisherId;
        bookToUpdate.CoverUrl = updatedBook.CoverUrl;
        bookToUpdate.Description = updatedBook.Description;

        var authorsToAdd = updatedBook.Authors?.Where(c => bookToUpdate.Authors!.All(d => c.AuthorId != d.AuthorId));

        var authorsToRemove = bookToUpdate.Authors?.Where(c => updatedBook.Authors!.All(d => c.AuthorId != d.AuthorId));

        foreach (var author in authorsToAdd!)
        {
            bookToUpdate.Authors?.Add(author);
            _dataContext.Authors.Attach(author);
        }

        foreach (var author in authorsToRemove!)
        {
            bookToUpdate.Authors?.Remove(author);
        }

        _logger.LogInformation("Update Book with {@BookId}", bookToUpdate.BookId);

        await _dataContext.SaveChangesAsync();
        return bookToUpdate.ToBook();
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