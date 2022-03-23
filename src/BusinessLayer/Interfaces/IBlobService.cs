﻿using BusinessLayer.Models;
using BusinessLayer.Responses;
namespace BusinessLayer.Interfaces;

public interface IBlobService
{
    Task<IEnumerable<string>> ListBlobsAsync();
    Task<BlobMetadataResponse> UploadAsync(FileModel model);
    Task<byte[]> GetAsync(string imageName);
    Task DeleteAsync(string blobName);
}