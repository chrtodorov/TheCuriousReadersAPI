using BusinessLayer.Enumerations;
using BusinessLayer.Interfaces.BookItems;
using BusinessLayer.Models;
using DataAccess.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccess.Repositories;

public class BookItemsRepository : IBookItemsRepository
{
    private readonly DataContext _dataContext;
    private readonly ILogger<BookItemsRepository> _logger;

    public BookItemsRepository(DataContext dataContext, ILogger<BookItemsRepository> logger)
    {
        this._dataContext = dataContext;
        this._logger = logger;
    }
    public async Task<BookItem> Create(BookItem bookItem)
    {
        var bookItemEntity = bookItem.ToBookItemEntity();
        await _dataContext.BookItems.AddAsync(bookItemEntity);
        await _dataContext.SaveChangesAsync();

        _logger.LogInformation("Create book item with {@BookItemId}", bookItemEntity.BookItemId);
        return bookItemEntity.ToBookItem();
    }

    public async Task Delete(Guid bookItemId)
    {
        var bookItemEntity = await _dataContext.BookItems.FindAsync(bookItemId);

        if (bookItemEntity is not null)
        {
            _dataContext.BookItems.Remove(bookItemEntity);
            await _dataContext.SaveChangesAsync();
            _logger.LogInformation("Deleting Book Item with {@BookItemId}", bookItemId);
        }
        _logger.LogInformation("There is no such Book Item with {@BookItemId}", bookItemId);
    }

    public async Task<BookItem?> Get(Guid bookItemId)
    {
        _logger.LogInformation("Get Book Item with {@BookItemId}", bookItemId);
        var bookItemEntity = await _dataContext.BookItems.FindAsync(bookItemId);
        return bookItemEntity?.ToBookItem();
    }
    public async Task<BookItem?> Update(Guid bookItemId, BookItem bookItem)
    {
        var bookItemEntity = await _dataContext.BookItems.FindAsync(bookItemId);

        if (bookItemEntity is null)
        {
            return null;
        }

        bookItemEntity.Barcode = bookItem.Barcode;
        bookItemEntity.BorrowedDate = bookItem.BorrowedDate;
        bookItemEntity.ReturnDate = bookItem.ReturnDate;
        bookItemEntity.BookStatus = bookItem.BookStatus;
        bookItemEntity.BookId = bookItem.BookId;

        await _dataContext.SaveChangesAsync();

        _logger.LogInformation("Update Book Item with {@BookItemId}", bookItemEntity.BookItemId);
        return bookItemEntity.ToBookItem();
    }
    public async Task<bool> Contains(Guid bookItemId)
    {
        return await _dataContext.BookItems.AnyAsync(b => b.BookItemId == bookItemId);
    }
        

    public async Task<BookItem?> UpdateBookItemStatus(Guid bookItemid, BookItemStatusEnumeration bookStatus)
    {
        var bookItemStatusEntity = await _dataContext.BookItems.FindAsync(bookItemid);
            
        if (bookItemStatusEntity is null)
        {
            return null;
        }
            
        bookItemStatusEntity.BookStatus = bookStatus;
            
        await _dataContext.SaveChangesAsync();
            
        _logger.LogInformation("Update Book Item Status with {@BookStatus}", bookItemStatusEntity.BookStatus);
        return bookItemStatusEntity.ToBookItem();
    }
}