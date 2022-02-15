using BusinessLayer.Models;

namespace BusinessLayer.Interfaces.Publishers;

public interface IPublishersService
{
    Task<Publisher?> Get(Guid publisherId);
    Task<List<Publisher>> GetAll();
    Task<Publisher> Create(Publisher publisher);
    Task<Publisher?> Update(Guid publisherId, Publisher publisher);
    Task Delete(Guid publisherId);
    Task<bool> Contains(Guid publisherId);
}