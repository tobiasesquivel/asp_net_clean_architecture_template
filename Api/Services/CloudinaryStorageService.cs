using api.Interfaces;
using api.Options;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ErrorOr;
using Microsoft.Extensions.Options;
using Error = ErrorOr.Error;

namespace api.Services;

public class CloudinaryStorageService : IStorageService
{
    private readonly CloudinaryOptions _options;
    public CloudinaryStorageService(IOptions<CloudinaryOptions> cloudinaryOptions)
    {
        this._options = cloudinaryOptions.Value;
    }
    public async Task<ErrorOr<List<string>>> UploadManyAsync(IFormFile[] files, CancellationToken? cancellationToken = null)
    {
        List<string> uploadedUrls = [];

        Account account = new Account(
            cloud: _options.CloudName,
            apiKey: _options.ApiKey,
            apiSecret: _options.ApiSecret
        );

        Cloudinary cloudinary = new Cloudinary(account);

        foreach (IFormFile file in files)
        {
            using Stream stream = file.OpenReadStream();

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new(file.FileName, stream),
                Folder = "marsgram/posts"
            };

            ImageUploadResult result = await cloudinary.UploadAsync(uploadParams, cancellationToken);

            if (result.Error is not null) return Error.Failure(description: $"Failed to upload image {file.Name}");

            uploadedUrls.Add(result.SecureUrl.ToString());
        }

        return uploadedUrls;
    }
}