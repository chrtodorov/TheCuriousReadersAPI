using System;
using System.Threading.Tasks;
using BusinessLayer.Interfaces;
using BusinessLayer.Requests;
using BusinessLayer.Responses;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace DataAccessTests;

public class BlobRepositoryTests
{
    private IBlobRepository _blobRepository;
    private ILogger<BlobRepository> _logger;
    private DataContext _context;

    #region Test entities
    private readonly BlobMetadata _blobMetadata = new()
    {
        Id = Guid.Parse("9fc0ae59-15cb-4a19-9916-4c431383fab5"),
        BlobName = "image.jpg",
        Url = "https://azureblobstorage/image.jpg",
        ContentType = "image/jpg",
        CreatedOn = new DateTime(2022, 02, 02),
        BookEntity = null
    };

    private readonly BlobMetadataRequest _blobRequest = new()
    {
        BlobName = "image.jpg",
        Url = "https://azureblobstorage/image.jpg",
        ContentType = "image/jpg",
        CreatedOn = new DateTime(2022, 02, 02)
    };

    private readonly BlobMetadataResponse _blobMetadataResponse = new()
    {
        Id = Guid.Parse("9fc0ae59-15cb-4a19-9916-4c431383fab5"),
        BlobName = "image.jpg",
        Url = "https://azureblobstorage/image.jpg",
        ContentType = "image/jpg",
        CreatedOn = new DateTime(2022, 02, 02)
    };
    #endregion

    [SetUp]
    public void Setup()
    {
        _logger = Substitute.For<ILogger<BlobRepository>>();
        _context = DbContextHelper.CreateInMemoryDatabase<DataContext>();

        if (_context is not null)
        {
            _blobRepository = new BlobRepository(_context, _logger);
        }
    }

    #region Tests

    [Test]
    public async Task CreateAsync()
    {
        var resultCreateBlob = await _blobRepository.Create(_blobRequest);

        var testCreatedBlob = await _context.BlobsMetadata.FirstOrDefaultAsync();

        Assert.IsNotNull(resultCreateBlob);

        Assert.AreEqual(resultCreateBlob.Id, testCreatedBlob!.Id);
    }

    [Test]
    public async Task DeleteAsync()
    {
        _context.BlobsMetadata.Add(_blobMetadata);
        await _context.SaveChangesAsync();

        var testBlobMetadata = await _context.BlobsMetadata.FirstOrDefaultAsync();

        await _blobRepository.Delete(testBlobMetadata!.BlobName);

        var testDeleteBlobMetadata = await _context.BlobsMetadata.FirstOrDefaultAsync();

        Assert.IsNull(testDeleteBlobMetadata);
    }
    #endregion
}