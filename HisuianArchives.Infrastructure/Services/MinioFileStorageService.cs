using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace HisuianArchives.Infrastructure.Services;

public class MinioFileStorageService : IFileStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioFileStorageService> _logger;
    private readonly string _bucketName;

    public MinioFileStorageService(IConfiguration configuration, ILogger<MinioFileStorageService> logger)
    {
        _logger = logger;

        var endpoint = configuration["Minio:Endpoint"] ?? throw new InvalidOperationException("Minio Endpoint not configured.");
        var accessKey = configuration["Minio:AccessKey"] ?? throw new InvalidOperationException("Minio AccessKey not configured.");
        var secretKey = configuration["Minio:SecretKey"] ?? throw new InvalidOperationException("Minio SecretKey not configured.");
        _bucketName = configuration["Minio:BucketName"] ?? throw new InvalidOperationException("Minio BucketName not configured.");

        _minioClient = new MinioClient()
            .WithEndpoint(endpoint)
            .WithCredentials(accessKey, secretKey)
            .Build();
    }

    public async Task<FileUploadResult> UploadFileAsync(Stream fileStream, string fileName, string contentType)
    {
        var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";

        try
        {
            await EnsureBucketExistsAsync();

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(uniqueFileName)
                .WithStreamData(fileStream)
                .WithObjectSize(fileStream.Length)
                .WithContentType(contentType);

            await _minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);

            var url = $"{_minioClient.Config.Endpoint}/{_bucketName}/{uniqueFileName}";

            _logger.LogInformation("File {FileName} uploaded to MinIO bucket {BucketName}", uniqueFileName, _bucketName);

            return new FileUploadResult(url, uniqueFileName);
        }
        catch (MinioException e)
        {
            _logger.LogError(e, "An error occurred while uploading to MinIO.");
            throw;
        }
    }

    private async Task EnsureBucketExistsAsync()
    {
        var beArgs = new BucketExistsArgs().WithBucket(_bucketName);
        bool found = await _minioClient.BucketExistsAsync(beArgs).ConfigureAwait(false);
        if (!found)
        {
            var mbArgs = new MakeBucketArgs().WithBucket(_bucketName);
            await _minioClient.MakeBucketAsync(mbArgs).ConfigureAwait(false);

            var policy = $@"{{""Version"":""2012-10-17"",""Statement"":[{{""Effect"":""Allow"",""Principal"":{{""AWS"":[""*""]}},""Action"":[""s3:GetObject""],""Resource"":[""arn:aws:s3:::{_bucketName}/*""]}}]}}";
            var spArgs = new SetPolicyArgs().WithBucket(_bucketName).WithPolicy(policy);
            await _minioClient.SetPolicyAsync(spArgs).ConfigureAwait(false);
        }
    }
}