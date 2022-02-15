using BusinessLayer.Interfaces.Authors;
using BusinessLayer.Models;
using DataAccess.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccess.Repositories;

public class AuthorsRepository : IAuthorsRepository
{
    private readonly DataContext _dataContext;
    private readonly ILogger<AuthorsRepository> _logger;

    public AuthorsRepository(DataContext dataContext, ILogger<AuthorsRepository> logger)
    {
        this._dataContext = dataContext;
        this._logger = logger;
    }

    public async Task<Author?> Get(Guid authorId)
    {
        _logger.LogInformation("Get Author with {@AuthorId}", authorId);

        var authorEntity = await _dataContext.Authors.FindAsync(authorId);

        return authorEntity?.ToAuthor();
    }

    public async Task<Author> Create(Author author)
    {
        var authorEntity = author.ToAuthorEntity();

        await _dataContext.Authors.AddAsync(authorEntity);

        await _dataContext.SaveChangesAsync();

        _logger.LogInformation("Create Author with {@AuthorId}", authorEntity.AuthorId);
        return authorEntity.ToAuthor();
    }

    public async Task<Author?> Update(Guid authorId, Author author)
    {
        var authorEntity = await _dataContext.Authors.FindAsync(authorId);

        if (authorEntity is null)
            return null;

        authorEntity.Name = author.Name;
        authorEntity.Bio = author.Bio;

        await _dataContext.SaveChangesAsync();

        _logger.LogInformation("Update Author with {@AuthorId}", authorEntity.AuthorId);

        return authorEntity.ToAuthor();
    }

    public async Task Delete(Guid authorId)
    {
        var authorEntity = await _dataContext.Authors.FindAsync(authorId);

        if (authorEntity is not null)
        {
            _dataContext.Authors.Remove(authorEntity);

            await _dataContext.SaveChangesAsync();

            _logger.LogInformation("Deleting Author with {@AuthorId}", authorId);
        }

        _logger.LogInformation("There is no such Author with {@AuthorId}", authorId);
    }

    public async Task<bool> Contains(Guid id)
    {
        return await _dataContext.Authors.AnyAsync(a => a.AuthorId == id);
    }
}