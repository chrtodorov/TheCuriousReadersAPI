using BusinessLayer.Models;

namespace BusinessLayer.Interfaces.BookRequests
{
    public interface IBookRequestsService
    {
        PagedList<BookRequestModel> GetAllRequests(BookRequestParameters bookRequestParameters);
        Task<BookRequestModel> MakeRequest(BookRequestModel bookRequest);
        Task<PagedList<BookRequestModel>> GetUserRequests(Guid customerId, BookRequestParameters bookRequestParameters);
    }
}
