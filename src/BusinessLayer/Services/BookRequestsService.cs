using BusinessLayer.Interfaces.BookItems;
using BusinessLayer.Interfaces.BookRequests;
using BusinessLayer.Models;

namespace BusinessLayer.Services
{
    public class BookRequestsService : IBookRequestsService
    {
        private readonly IBookRequestsRepository bookRequestsRepository;
        private readonly IBookItemsRepository bookItemsRepository;

        public BookRequestsService(IBookRequestsRepository bookRequestsRepository, IBookItemsRepository bookItemsRepository)
        {
            this.bookRequestsRepository = bookRequestsRepository;
            this.bookItemsRepository = bookItemsRepository;
        }

        public PagedList<BookRequestModel> GetAllRequests(BookRequestParameters bookRequestParameters)
             => bookRequestsRepository.GetAllRequests(bookRequestParameters);

        public async Task<PagedList<BookRequestModel>> GetUserRequests(Guid customerId, BookRequestParameters bookRequestParameters)
            => await bookRequestsRepository.GetUserRequests(customerId, bookRequestParameters);


        public async Task<BookRequestModel> MakeRequest(BookRequestModel bookRequest)
        {
            var hasAvailableBookItems = await bookItemsRepository.HasAvailableItems(bookRequest.BookId);
            if (!hasAvailableBookItems)
            {
                throw new ArgumentException($"There are no availbale copies of a book with id: {bookRequest.BookId}");
            }
            return await bookRequestsRepository.MakeRequest(bookRequest);
        }
    }
}