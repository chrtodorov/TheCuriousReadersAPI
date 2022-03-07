using BusinessLayer.Interfaces.Publishers;
using BusinessLayer.Models;
using DataAccess.Entities;
using DataAccess.Mappers;
using Microsoft.Data.SqlClient;
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
        try
        {
            await _dataContext.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            _logger.LogCritical(e.ToString());
            throw;
        }
        
        _logger.LogInformation("Create publisher with {@PublisherId}", publisherEntity.PublisherId);
        return publisherEntity.ToPublisher();
    }


    public async Task Delete(Guid publisherId)
    {
        var publisherEntity = await GetById(publisherId);

        if (publisherEntity is not null)
        {
            _dataContext.Publishers.Remove(publisherEntity);
            try
            {
                await _dataContext.SaveChangesAsync();
                _logger.LogInformation("Deleting Publisher with {@PublisherId}", publisherId);
            }
            catch (DbUpdateException e)
            {
                var sqlException = e.GetBaseException() as SqlException;

                if (sqlException != null && sqlException.Number == 547)
                {
                    throw new ArgumentException("Must delete all books from this publisher before deleting it.");
                }
            }
            
            
        }
        _logger.LogInformation("There is no such Publisher with {@PublisherId}", publisherId);
    }

    public async Task<Publisher?> Get(Guid publisherId)
    {
        _logger.LogInformation("Get Publisher with {@PublisherId}", publisherId);
        var publisherEntity = await GetById(publisherId, false);
        return publisherEntity?.ToPublisher();
    }

    public async Task<PublisherEntity?> GetById(Guid publisherId, bool tracking = true)
    {
        var query = _dataContext.Publishers
            .Where(p => p.PublisherId == publisherId);
        if (!tracking)
            query.AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    public async Task<List<Publisher>> GetAll()
    {
        _logger.LogInformation("Get all Publishers");

        var publisherEntity = await _dataContext.Publishers
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .Select(p => p.ToPublisher())
            .ToListAsync();

        return publisherEntity;
    }

    public async Task<Publisher?> Update(Guid publisherId, Publisher publisher)
    {
        var publisherEntity = await GetById(publisherId);

        if (publisherEntity is null)
        {
            return null;
        }

        publisherEntity.Name = publisher.Name;

        try
        {
            await _dataContext.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            _logger.LogCritical(e.ToString());
            throw;
        }

        _logger.LogInformation("Updated Publisher with {@PublisherId}", publisherEntity.PublisherId);

        return publisherEntity.ToPublisher();

    }
    public async Task<bool> Contains(Guid publisherId)
    {
        return await _dataContext.Publishers.AnyAsync(p => p.PublisherId == publisherId);
    }

    public async Task<bool> IsPublisherNameExisting(string name)
    {
        return await _dataContext.Publishers.AnyAsync(p => p.Name == name);
    }
}