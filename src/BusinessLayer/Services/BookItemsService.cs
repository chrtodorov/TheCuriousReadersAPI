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

    public async Task<BookItem> Create(BookItem bookItem)
    {
        if (await _bookItemsRepository.IsBarcodeExisting(bookItem.Barcode))
        {
            throw new ArgumentException($"Book Copy with this barcode: {bookItem.Barcode} already exists.");
        }
        return await this._bookItemsRepository.Create(bookItem);
    }

    public async Task Delete(Guid bookItemId)
    {
        if (!await _bookItemsRepository.Contains(bookItemId))
        {
            throw new ArgumentException("Book copy cannot be found!");
        }
        await this._bookItemsRepository.Delete(bookItemId);
    } 
    

    public async Task<BookItem?> Get(Guid bookItemId) => await this._bookItemsRepository.Get(bookItemId);

    public async Task<BookItem?> Update(Guid bookItemId, BookItem bookItem)
    {
        if (!await _bookItemsRepository.Contains(bookItemId))
        {
            throw new ArgumentException("Book copy cannot be found!");
        }
        if (await _bookItemsRepository.IsBarcodeExisting(bookItem.Barcode))
        {
            throw new ArgumentException($"Book Copy with this barcode: {bookItem.Barcode} already exists.");
        }
        return await this._bookItemsRepository.Update(bookItemId, bookItem);
    }
    public async Task<bool> Contains(Guid bookItemId) => await this._bookItemsRepository.Contains(bookItemId);

    public async Task<BookItem?> UpdateBookItemStatus(Guid bookItemId, BookItemStatusEnumeration bookStatus) => await this._bookItemsRepository.UpdateBookItemStatus(bookItemId, bookStatus);
    public async Task<bool> IsBarcodeExisting(string barcode) => await _bookItemsRepository.IsBarcodeExisting(barcode);
}