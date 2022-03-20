using BusinessLayer.Enumerations;
using BusinessLayer.Interfaces.Books;
using BusinessLayer.Models;
using BusinessLayer.Responses;
using DataAccess.Entities;
using DataAccess.Mappers;
using Microsoft.Data.SqlClient;
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

    public async Task<BookDetailsResponse?> Get(Guid bookId)
    {
        _logger.LogInformation("Get Book with {@BookId}", bookId);

        var bookEntity = await _dataContext.Books
            .Include(b => b.Authors)
            .Include(b => b.Publisher)
            .Include(b => b.BookItems)
            .Where(b => b.BookId == bookId)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return bookEntity?.ToBookDetailsResponse();
    }

    public async Task<BookEntity?> GetById(Guid bookId, bool tracking = true)
    {
        var book = _dataContext.Books
            .Include(b => b.Authors)
            .Include(b => b.BookItems)
            .Where(b => b.BookId == bookId);
        if (!tracking)
            book.AsNoTracking();

        return await book.FirstOrDefaultAsync();
    }

    public async Task<List<Book>> GetLatest()
    {
        var bookList = await _dataContext.Books
           .Include(b => b.Authors)
           .OrderByDescending(b => b.CreatedAt)
           .Take(20)
           .Select(b => b.ToBook())
           .AsNoTracking()
           .ToListAsync();

        _logger.LogInformation("Get latest books");
        if(bookList.Count == 0)
        {
            _logger.LogInformation("No books to show");
        }

        return bookList;
    }

    public async Task<int> GetNumber()
    {
        var bookCount = await _dataContext.Books
            .CountAsync();
        return bookCount;
    }

    public async Task<PagedList<Book>> GetBooks(BookParameters bookParameters)
    {
        var query = _dataContext.Books.Include(b => b.Authors).AsNoTracking();
        var result = Enumerable.Empty<BookEntity>();

        if (!string.IsNullOrEmpty(bookParameters.Title))
        {
            result = result.Union(query.Where(b => b.Title.Contains(bookParameters.Title)));
        }

        if (!string.IsNullOrEmpty(bookParameters.Author))
        {
            var author = await _dataContext.Authors.FirstOrDefaultAsync(a => a.Name == bookParameters.Author);
            result = result.Union(query.Where(b => b.Authors!.Contains(author!)));
        }

        if (!string.IsNullOrEmpty(bookParameters.Publisher))
        {
            result = result.Union(query.Where(b => b.Publisher!.Name == bookParameters.Publisher));
        }

        if (!string.IsNullOrEmpty(bookParameters.DescriptionKeyword))
        {
            result = result.Union(query.Where(b => b.Description.Contains(bookParameters.DescriptionKeyword)));
        }

        if (!string.IsNullOrEmpty(bookParameters.Genre))
        {
            result = result.Union(query.Where(b => b.Genre == bookParameters.Genre));
        }
        result = result.Count() == 0 ? query : result;
        _logger.LogInformation("Get all books");

        return PagedList<Book>.ToPagedList(result.AsQueryable()
            .OrderBy(b => b.Title)
            .Select(b => b.ToBook()),
            bookParameters.PageNumber,
            bookParameters.PageSize);
    }

    public async Task<Book> Create(Book book)
    {
        var bookEntity = book.ToBookEntity();

        if (!await _dataContext.Genres.AnyAsync(g => g.Name == bookEntity.Genre))
        {
            var genreEntity = new GenreEntity()
            {
                Name = bookEntity.Genre
            };
            _dataContext.Genres.Add(genreEntity);
        }

        foreach (var author in bookEntity.Authors!)
        {
            _dataContext.Authors.Attach(author);
        }
            
        await _dataContext.Books.AddAsync(bookEntity);
        try
        {
            await _dataContext.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            _logger.LogCritical(e.Message);
        }

        _logger.LogInformation("Create Book with {@BookId}", bookEntity.BookId);

        return bookEntity.ToBook();
    }

    public async Task<Book?> Update(Guid bookId, Book book)
    {
        var updatedBook = book.ToBookEntity();

        var bookToUpdate = await GetById(bookId);

        if (bookToUpdate is null) return null;

        bookToUpdate.Isbn = updatedBook.Isbn;
        bookToUpdate.Title = updatedBook.Title;
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

        var copiesToAdd = updatedBook.BookItems?.Where(c => bookToUpdate.BookItems!.All(d => c.Barcode != d.Barcode));

        var copiesToRemove = updatedBook.BookItems?.Where(c => updatedBook.BookItems!.All(d => c.Barcode != d.Barcode));

        foreach (var copy in copiesToAdd!)
        {
            bookToUpdate.BookItems?.Add(copy);
            _dataContext.BookItems.Attach(copy);
        }
        foreach(var copy in copiesToRemove!)
        {
            bookToUpdate.BookItems?.Remove(copy);
        }

        _logger.LogInformation("Update Book with {@BookId}", bookToUpdate.BookId);

        try
        {
            await _dataContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogCritical(e.Message);
        }
        
        return bookToUpdate.ToBook();
    }

    public async Task Delete(Guid bookId)
    {
        var bookEntity = await GetById(bookId);

        if (bookEntity is not null)
        {
            _dataContext.Books.Remove(bookEntity);

            try
            {
                await _dataContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                if (e.GetBaseException() is SqlException {Number: 547})
                {
                    throw new ArgumentException("All book requests and loans must be completed before deleting this book!");
                }
            }

            _logger.LogInformation("Deleting Book with {@BookId}", bookId);
        }

        _logger.LogInformation("There is no such Book with {@BookId}", bookId);
    }

    public async Task<bool> Contains(Guid bookId)
    {
        return await _dataContext.Books.AnyAsync(b => b.BookId == bookId);
    }

    public async Task<bool> IsIsbnExisting(string isbn)
    {
        return await _dataContext.Books.AnyAsync(b => b.Isbn == isbn);
    }

    public async Task MakeUnavailable(Guid bookId)
    {
        var book = await GetById(bookId);
        if (book is null)
        {
            _logger.LogInformation("There is no such Book with { @BookId }", bookId);
        }
        foreach (var bookItem in book.BookItems)
        {
            if (bookItem.BookStatus == BookItemStatusEnumeration.Available)
            {
                bookItem.BookStatus = BookItemStatusEnumeration.NotAvailable;
            }
        }
        try
        {
            await _dataContext.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            _logger.LogCritical(e.Message);
        }
        
    }

    public async Task<bool> HasLoanedItems(Guid bookId)
    {
        var book = await _dataContext.Books
            .Include(b => b.BookItems)
            .AsSplitQuery()
            .FirstOrDefaultAsync(b => b.BookId == bookId);

        if (book is null)
        {
            throw new ArgumentNullException(nameof(bookId), $"Book with id: {bookId} does not exist");
        }
        return book.BookItems!.Any(i => i.BookStatus == BookItemStatusEnumeration.Borrowed);
    }

}