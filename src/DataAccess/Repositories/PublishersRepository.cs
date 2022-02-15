using BusinessLayer.Interfaces.Publishers;
using BusinessLayer.Models;
using DataAccess.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccess.Repositories;

public class PublishersRepository : IPublishersRepository
{
    private readonly DataContext _dataContext;
    private readonly ILogger<PublishersRepository> _logger;

    public PublishersRepository(DataContext dataContext, ILogger<PublishersRepository> logger)
    {
        this._dataContext = dataContext;
        this._logger = logger;
    }

    public async Task<Publisher> Create(Publisher publisher)
    {
        var publisherEntity = publisher.ToPublisherEntity();
        await _dataContext.Publishers.AddAsync(publisherEntity);
        await _dataContext.SaveChangesAsync();

        _logger.LogInformation("Create publisher with {@PublisherId}", publisherEntity.PublisherId);
        return publisherEntity.ToPublisher();
    }


    public async Task Delete(Guid publisherId)
    {
        var publisherEntity = await _dataContext.Publishers.FindAsync(publisherId);

        if (publisherEntity is not null)
        {
            _dataContext.Publishers.Remove(publisherEntity);
            await _dataContext.SaveChangesAsync();
            _logger.LogInformation("Deleting Publisher with {@PublisherId}", publisherId);
        }
        _logger.LogInformation("There is no such Publisher with {@PublisherId}", publisherId);
    }

    public async Task<Publisher?> Get(Guid publisherId)
    {
        _logger.LogInformation("Get Publisher with {@PublisherId}", publisherId);
        var publisherEntity = await _dataContext.Publishers.FindAsync(publisherId);
        return publisherEntity?.ToPublisher();
    }

    public async Task<List<Publisher>> GetAll()
    {
        _logger.LogInformation("Get all Publishers");

        var publisherEntity = await _dataContext.Publishers
            .Where(p => p != null)
            .OrderBy(p => p.Name)
            .Select(p => p.ToPublisher())
            .ToListAsync();

        return publisherEntity;
    }

    public async Task<Publisher?> Update(Guid publisherId, Publisher publisher)
    {
        var publisherEntity = await _dataContext.Publishers.FindAsync(publisherId);

        if (publisherEntity is null)
        {
            return null;
        }

        publisherEntity.Name = publisher.Name;

        await _dataContext.SaveChangesAsync();

        _logger.LogInformation("Updated Publisher with {@PublisherId}", publisherEntity.PublisherId);

        return publisherEntity.ToPublisher();

    }
    public async Task<bool> Contains(Guid publisherId)
    {
        return await _dataContext.Publishers.AnyAsync(p => p.PublisherId == publisherId);
    }
}