namespace HisuianArchives.Application.Interfaces;

public record FileUploadResult(string Url, string FileName);

public interface IFileStorageService
{
    Task<FileUploadResult> UploadFileAsync(Stream fileStream, string fileName, string contentType);
}