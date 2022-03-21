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

        public PagedList<BookRequestModel> GetAllRequests(PagingParameters bookRequestParameters)
             => bookRequestsRepository.GetAllRequests(bookRequestParameters);

        public async Task<PagedList<BookRequestModel>> GetUserRequests(Guid customerId, PagingParameters bookRequestParameters)
            => await bookRequestsRepository.GetUserRequests(customerId, bookRequestParameters);


        public async Task<BookRequestModel> MakeRequest(BookRequestModel bookRequest)
        {
            var hasAvailableBookItems = await bookItemsRepository.HasAvailableItems(bookRequest.BookId);
            if (!hasAvailableBookItems)
            {
                throw new ArgumentNullException($"There are no available copies of a book with id: {bookRequest.BookId}");
            }
            return await bookRequestsRepository.MakeRequest(bookRequest);
        }

        public async Task RejectRequest(Guid bookRequestId)
            => await bookRequestsRepository.RejectRequest(bookRequestId);
    }
}