using BusinessLayer.Enumerations;
using BusinessLayer.Models;
using BusinessLayer.Requests;
using BusinessLayer.Responses;
using DataAccess.Entities;

namespace DataAccess.Mappers
{
    public static class BookRequestsMapper
    {
        public static BookRequestModel ToBookRequest(this BookRequestEntity bookRequestEntity)
        {
            return new BookRequestModel
            {
                BookRequestId = bookRequestEntity.BookRequestId,
                Status = bookRequestEntity.Status,
                AuditedBy = bookRequestEntity.AuditedBy,
                RequestedBy = bookRequestEntity.RequestedBy,
                Book = bookRequestEntity.BookItem?.Book.ToBookWithoutItems(),
                CreatedAt = bookRequestEntity.CreatedAt,
            };
        }

        public static BookRequestEntity ToBookRequestEntity(this BookRequestModel bookRequest, Guid bookItemId)
        {
            return new BookRequestEntity
            {
                Status = bookRequest.Status,
                AuditedBy = bookRequest.AuditedBy,
                RequestedBy = bookRequest.RequestedBy,
                BookItemId = bookItemId
            };
        }

        public static BookRequestModel ToBookRequest(this BookRequestRequest bookRequest, Guid requesterId)
        {
            return new BookRequestModel
            {
                BookId = bookRequest.BookId,
                RequestedBy = requesterId,
                Status = BookRequestStatus.Pending,
            };
        }

        public static BookRequestResponse ToBookRequestResponse(this BookRequestModel bookRequest) 
        {
            return new BookRequestResponse
            {
                CreatedAt = bookRequest.CreatedAt,
                Status = bookRequest.Status,
                Book = bookRequest.Book,
            };
        }

        public static PagedList<BookRequestResponse> ToPagedList(this IEnumerable<BookRequestResponse> data, int totalCount, int currentPage, int pageSize)
        {
            return new PagedList<BookRequestResponse>(data.ToList(), totalCount, currentPage, pageSize);
        }
    }
}
