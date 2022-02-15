using BusinessLayer.Interfaces.Authors;
using BusinessLayer.Models;

namespace BusinessLayer.Services;

public class AuthorsService : IAuthorsService
{
    private readonly IAuthorsRepository _authorsRepository;

    public AuthorsService(IAuthorsRepository authorsRepository)
    {
        this._authorsRepository = authorsRepository;
    }

    public async Task<Author?> Get(Guid authorId) => await _authorsRepository.Get(authorId);

    public async Task<Author> Create(Author author) => await _authorsRepository.Create(author);

    public async Task<Author?> Update(Guid authorId, Author author) => await _authorsRepository.Update(authorId, author);

    public async Task Delete(Guid authorId) => await _authorsRepository.Delete(authorId);
    public async Task<bool> Contains(Guid id) => await _authorsRepository.Contains(id);

    public async Task<List<Author>> GetAuthors(AuthorParameters authorParameters) => await _authorsRepository.GetAuthors(authorParameters);
}