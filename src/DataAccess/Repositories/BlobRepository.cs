using BusinessLayer.Interfaces;
using BusinessLayer.Requests;
using BusinessLayer.Responses;
using DataAccess.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class BlobRepository : IBlobRepository
    {
        private readonly DataContext _dataContext;

        public BlobRepository(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public async Task<BlobMetadataResponse> Create(BlobMetadataRequest blobRequest)
        {
            var blobMetadata = blobRequest.ToBlobMetadata();

            try
            {
                await _dataContext.BlobsMetadata.AddAsync(blobMetadata);
                await _dataContext.SaveChangesAsync();
                return blobMetadata.ToBlobResponse();
            }
            catch (DbUpdateException e)
            {
                throw new ArgumentException("Error while trying to create BlobMetadata");
            }
        }

        public async Task Delete(string name)
        {
            try
            {
                var blob = await _dataContext.BlobsMetadata
                    .FirstOrDefaultAsync(b => b.BlobName == name);
                if (blob is null)
                    throw new ArgumentException("Cannot find BlobMetadata with this ID: " + name);

                _dataContext.BlobsMetadata.Remove(blob);
                await _dataContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new ArgumentException("Error while trying to delete BlobMetadata");
            }
            
        }
    }
}
