using BusinessLayer.Models;
using BusinessLayer.Requests;
using DataAccess.Entities;

namespace DataAccess.Mappers;

public static class AuthorMapper
{
    public static Author ToAuthor(this AuthorEntity authorEntity)
    {
        return new Author
        {
            AuthorId = authorEntity.AuthorId,
            FirstName = authorEntity.FirstName,
            LastName = authorEntity.LastName,
            Bio = authorEntity.Bio
        };
    }

    public static AuthorEntity ToAuthorEntity(this Author author)
    {
        return new AuthorEntity
        {
            FirstName = author.FirstName,
            LastName = author.LastName,
            Bio = author.Bio
        };
    }

    public static Author ToAuthor(this AuthorsRequest authorsCreateRequest)
    {
        return new Author
        {
            FirstName = authorsCreateRequest.FirstName,
            LastName = authorsCreateRequest.LastName,
            Bio = authorsCreateRequest.Bio
        };
    }
}