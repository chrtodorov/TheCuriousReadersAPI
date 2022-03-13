﻿using BusinessLayer.Models;
using BusinessLayer.Requests;
using BusinessLayer.Responses;
using DataAccess.Entities;

namespace DataAccess.Mappers;

public static class BookMapper
{
    public static Book ToBook(this BookEntity bookEntity)
    {
        return new Book
        {
            BookId = bookEntity.BookId,
            Isbn = bookEntity.Isbn.Trim(),
            CreatedAt = bookEntity.CreatedAt,
            Title = bookEntity.Title,
            Description = bookEntity.Description,
            Genre = bookEntity.Genre,
            CoverUrl = bookEntity.CoverUrl,
            PublisherId = bookEntity.PublisherId,
            AuthorsIds = bookEntity.Authors?.Select(a => a.AuthorId).ToList(),
            BookItems = bookEntity.BookItems?.Select(a => a.ToBookItem()).ToList()
        };
    }

    public static Book ToBookWithoutItems(this BookEntity bookEntity)
    {
        return new Book
        {
            BookId = bookEntity.BookId,
            Isbn = bookEntity.Isbn.Trim(),
            Title = bookEntity.Title,
            Description = bookEntity.Description,
            Genre = bookEntity.Genre,
            CoverUrl = bookEntity.CoverUrl,
            PublisherId = bookEntity.PublisherId,
            AuthorsIds = bookEntity.Authors?.Select(a => a.AuthorId).ToList(),
        };
    }

    public static BookEntity ToBookEntity(this Book book)
    {
        return new BookEntity
        {
            Isbn = book.Isbn.Trim(),
            Title = book.Title,
            Description = book.Description,
            Genre = book.Genre,
            CoverUrl = book.CoverUrl,
            PublisherId = book.PublisherId,
            Authors = book.AuthorsIds?.Select(a => new AuthorEntity(){ AuthorId = a}).ToList(),
            BookItems = book.BookItems?.Select(a => a.ToBookItemEntity()).ToList()
        };
    }

    public static Book ToBook(this BookRequest bookRequest)
    {
        return new Book
        {
            Isbn = bookRequest.Isbn.Trim(),
            Title = bookRequest.Title,
            Description = bookRequest.Description,
            Genre = bookRequest.Genre,
            CoverUrl = bookRequest.CoverUrl,
            PublisherId = bookRequest.PublisherId,
            AuthorsIds = bookRequest.AuthorsIds,
            BookItems = bookRequest.BookCopies?.Select(a => a.ToBookItem()).ToList()
        };
    }

    public static BookDetailsResponse ToBookDetailsResponse(this BookEntity bookEntity) 
    {
        return new BookDetailsResponse
        {
            BookId = bookEntity.BookId,
            Isbn = bookEntity.Isbn,
            Title = bookEntity.Title,
            Description = bookEntity.Description,
            Genre = bookEntity.Genre,
            CoverUrl = bookEntity.CoverUrl,
            Publisher = bookEntity.Publisher?.ToPublisher(),
            Authors = bookEntity.Authors?.Select(a => a.ToAuthor()).ToList()
        };
    }
}