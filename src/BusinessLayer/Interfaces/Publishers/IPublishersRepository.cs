using BusinessLayer.Models;

namespace BusinessLayer.Interfaces.Publishers;

public interface IPublishersRepository
{
    Task<Publisher?> Get(Guid publisherId);
    Task<Publisher> Create(Publisher publisher);
    Task<Publisher?> Update(Guid publisherId, Publisher publisher);
    Task Delete(Guid publisherId);
    Task<bool> Contains(Guid publisherId);
}