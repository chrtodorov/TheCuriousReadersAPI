using BusinessLayer.Models;

namespace BusinessLayer.Interfaces.Authors;

public interface IAuthorsService
{
    Task<Author?> Get(Guid authorId);
    Task<Author> Create(Author author);
    Task<Author?> Update(Guid authorId, Author author);
    Task Delete(Guid id);
    Task<bool> Contains(Guid id);
}