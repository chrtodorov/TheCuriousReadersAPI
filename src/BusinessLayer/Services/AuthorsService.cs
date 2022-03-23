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

    public async Task<Author> Create(Author author)
    {
        if (await _authorsRepository.IsAuthorNameExisting(author.Name))
        {
            throw new ArgumentException($"Author with this name: {author.Name} is already existing!");
        }

        return await _authorsRepository.Create(author);
    }

    public async Task<Author?> Update(Guid authorId, Author author)
    {
        if (!await _authorsRepository.Contains(authorId))
        {
            throw new ArgumentNullException(nameof(author), "Author cannot be found!");
        }

        var authorFromDb = await _authorsRepository.Get(authorId);

        if (await _authorsRepository.IsAuthorNameExisting(author.Name) && authorFromDb!.Name != author.Name)
        {
            throw new ArgumentException($"Author with this name: {author.Name} is already existing!");
        }

        return await _authorsRepository.Update(authorId, author);
    }
    public async Task Delete(Guid authorId)
    {
        if (!await _authorsRepository.Contains(authorId))
        {
            throw new ArgumentNullException(nameof(authorId), "Author cannot be found!");
        }
        await _authorsRepository.Delete(authorId);
    }
    public async Task<bool> Contains(Guid id) => await _authorsRepository.Contains(id);
    public async Task<bool> IsAuthorNameExisting(string name) => await _authorsRepository.IsAuthorNameExisting(name);

    public async Task<PagedList<Author>> GetAuthors(AuthorParameters authorParameters) => await _authorsRepository.GetAuthors(authorParameters);
}