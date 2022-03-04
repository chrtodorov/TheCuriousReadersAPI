using BusinessLayer.Interfaces.Publishers;
using BusinessLayer.Models;

namespace BusinessLayer.Services;

public class PublisherService : IPublishersService
{
    private readonly IPublishersRepository _publisherRepository;

    public PublisherService(IPublishersRepository publishersRepository)
    {
        _publisherRepository = publishersRepository;
    }
    public async Task<bool> Contains(Guid publisherId) => await _publisherRepository.Contains(publisherId);

    public async Task<bool> IsPublisherNameExisting(string name) => await _publisherRepository.IsPublisherNameExisting(name);

    public async Task<Publisher> Create(Publisher publisher)
    {
        if (await _publisherRepository.IsPublisherNameExisting(publisher.Name))
        {
            throw new ArgumentException($"Publisher with this name: {publisher.Name} is already existing!");
        }
        return await _publisherRepository.Create(publisher);
    } 

    public async Task Delete(Guid publisherId)
    {
        if (!await _publisherRepository.Contains(publisherId))
        {
            throw new ArgumentException("Publisher cannot be found!");
        }
        await _publisherRepository.Delete(publisherId);
    } 

    public async Task<Publisher?> Get(Guid publisherId) => await _publisherRepository.Get(publisherId);

    public async Task<List<Publisher>> GetAll() => await _publisherRepository.GetAll();
    public async Task<Publisher?> Update(Guid publisherId, Publisher publisher)
    {
        if (!await _publisherRepository.Contains(publisherId))
        {
            throw new ArgumentException("Publisher cannot be found!");
        }
        if (await _publisherRepository.IsPublisherNameExisting(publisher.Name))
        {
            throw new ArgumentException($"Publisher with this name: {publisher.Name} is already existing!");
        }

        return await _publisherRepository.Update(publisherId, publisher);
    } 
}