using BusinessLayer.Enumerations;
using BusinessLayer.Interfaces.BookItems;
using BusinessLayer.Models;

namespace BusinessLayer.Services;

public class BookItemsService : IBookItemsService
{
    private readonly IBookItemsRepository _bookItemsRepository;

    public BookItemsService(IBookItemsRepository bookItemsRepository)
    {
        this._bookItemsRepository = bookItemsRepository;
    }

    public async Task<BookItem> Create(BookItem bookItem) => await this._bookItemsRepository.Create(bookItem);

    public async Task Delete(Guid bookItemId) => await this._bookItemsRepository.Delete(bookItemId);

    public async Task<BookItem?> Get(Guid bookItemId) => await this._bookItemsRepository.Get(bookItemId);

    public async Task<BookItem?> Update(Guid bookItemId, BookItem bookItem) => await this._bookItemsRepository.Update(bookItemId, bookItem);
    public async Task<bool> Contains(Guid bookItemId) => await this._bookItemsRepository.Contains(bookItemId);

    public async Task<BookItem?> UpdateBookItemStatus(Guid bookItemId, BookItemStatusEnumeration bookStatus) => await this._bookItemsRepository.UpdateBookItemStatus(bookItemId, bookStatus);
}