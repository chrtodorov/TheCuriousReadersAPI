using BusinessLayer.Interfaces.Publishers;
using BusinessLayer.Models;

namespace BusinessLayer.Services;

public class PublisherService : IPublishersService
{
    private readonly IPublishersRepository _publisherRepository;

    public PublisherService(IPublishersRepository publishersRepository)
    {
        this._publisherRepository = publishersRepository;
    }
    public async Task<bool> Contains(Guid publisherId) => await this._publisherRepository.Contains(publisherId);

    public async Task<Publisher> Create(Publisher publisher) => await this._publisherRepository.Create(publisher);

    public async Task Delete(Guid publisherId) => await this._publisherRepository.Delete(publisherId);

    public async Task<Publisher?> Get(Guid publisherId) => await this._publisherRepository.Get(publisherId);

    public async Task<Publisher?> Update(Guid publisherId, Publisher publisher) => await this._publisherRepository.Update(publisherId, publisher);
}